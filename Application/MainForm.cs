using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using Application.Models.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Application
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private static readonly string _sqlScriptConnectionTest = "select @@version";
        private static readonly string _messageErrorConnection = "Ошибка подключения";
        private static readonly string _tableName = "FirstTable";

        private static Options _options;
        private static Option _selectedOptions;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Начальна загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form1_Load(object sender, EventArgs e)
        {
            string path = "options.json";
            if (!File.Exists(path))
            {
                this.labelConnectionStatus.EditValue = $"Отсутствует файл {path}";
                return;
            }

            string optionText;
            using (StreamReader textReader = new StreamReader(path))
            {
                optionText = await textReader.ReadToEndAsync();
                if (string.IsNullOrEmpty(optionText))
                {
                    this.labelConnectionStatus.EditValue = $"Файл {path} пустой";
                    return;
                }
            }
            try
            {
                _options = JsonSerializer.Deserialize<Options>(optionText);
            }
            catch (Exception ex)
            {
                this.labelConnectionStatus.EditValue = $"Ошибка чтения файла опций: {ex.Message}";
                return;
            }

            DateBegin.EditValue = DateTime.Today.AddMonths(-1);
            DateEnd.EditValue = DateTime.Today;

            (ComboBoxServerSelect.Edit as RepositoryItemComboBox)
                .Items
                .AddRange(_options.Option.Select(x => x.Name).ToList());
        }

        /// <summary>
        /// Выбор сервера (событие комбобокса)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ComboBoxServerSelect_EditValueChanged(object sender, EventArgs e)
        {
            SetDefaultData();
            BtnLoad.Enabled = false;
            BtnExportExcel.Enabled = false;

            var serverName = (string)ComboBoxServerSelect.EditValue;
            string messageString = null;
            this.labelConnectionStatus.EditValue = "Подключение...";

            var option = _options.Option.FirstOrDefault(x => x.Name == serverName);
            if (option is null)
            {
                this.labelConnectionStatus.EditValue = "Произошла ошибка при выборе строки подключения после выбора сервера";
                return;
            }
            _selectedOptions = option;

            //Проверка доступа к БД
            try
            {
                using (var connection = new SqlConnection(_selectedOptions.ConnectionString))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand(_sqlScriptConnectionTest, connection)
                    {
                        Connection = connection
                    };
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            var id = reader.GetValue(0);
                            if (id != null)
                                messageString = "Подключено к БД";
                            else
                                this.labelConnectionStatus.EditValue = _messageErrorConnection;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.labelConnectionStatus.EditValue = $"{_messageErrorConnection} {ex.Message}";
                return;
            }

            string sqlExpression = await GetSqlExpressionFromFile(OptionScriptType.Tree);
            if (string.IsNullOrEmpty(sqlExpression))
                return;

            var table = await GetDataForGrid(sqlExpression);
            if (table == null)
                return;

            var dataSet = new DataSet();
            dataSet.Tables.Add(table);
            measureUnitTree.DataSource = dataSet;
            measureUnitTree.DataMember = _tableName;
            measureUnitTree.KeyFieldName = "Device_id";
            measureUnitTree.ParentFieldName = "MasterID";

            this.labelConnectionStatus.EditValue = messageString;
        }

        /// <summary>
        /// Загрузить данные (событие по кнопке)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnLoad_ItemClick(object sender, ItemClickEventArgs e)
        {
            var generalScript = _selectedOptions.GeneralScript;
            var a = "asd {general}".Replace("{general}", $"/r/n{generalScript}");



            var selectedDeviceIds = measureUnitTree
                .GetAllCheckedNodes()
                .Select(x => x.GetValue("Device_id").ToString());
            if (!selectedDeviceIds.Any())
                return;

            SetDefaultData();

            var isMeasure = MeasureType.EditValue as bool?;
            var analyticTypeValue = AnalyticType.EditValue as int?;

            if (!isMeasure.HasValue || !analyticTypeValue.HasValue)
                return;

            OptionScriptType? scriptType = null;
            if (isMeasure.Value)
                scriptType = OptionScriptType.Main;
            else
            {
                switch(analyticTypeValue.Value)
                {
                    case 1:
                        scriptType = OptionScriptType.Analytic1;
                        break;
                    case 2:
                        scriptType = OptionScriptType.Analytic2;
                        break;
                    case 3:
                        scriptType = OptionScriptType.Analytic3;
                        break;
                    case 4:
                        scriptType = OptionScriptType.Analytic4;
                        break;
                    case 5:
                        scriptType = OptionScriptType.Analytic5;
                        break;
                    case 6:
                        scriptType = OptionScriptType.Analytic6;
                        break;
                }
            }
            if (scriptType == null)
            {
                this.labelConnectionStatus.EditValue = "Ошибка определения типа скрипта";
                return;
            }
            string sqlExpression = await GetSqlExpressionFromFile(scriptType.Value);
            if (string.IsNullOrEmpty(sqlExpression))
                return;
            if (!string.IsNullOrEmpty(_selectedOptions.GeneralScript))
            {
                string sqlGeneralExpression = await GetSqlExpressionFromFile(OptionScriptType.GeneralScript);
                sqlExpression = sqlExpression.Replace("--{GENERAL}", sqlGeneralExpression);
                //var generalScript = _selectedOptions.GeneralScript;
                //sqlExpression = string.Format(sqlExpression, generalScript);

            }
            
            var dateBegin = ((DateTime)DateBegin.EditValue).ToString("yyyyMMdd");
            var dateEnd = ((DateTime)DateEnd.EditValue).ToString("yyyyMMdd");
            var selectedDeviceIdsInSqlForJoinMainScript = $"\r\n INNER JOIN (SELECT * FROM (VALUES ({string.Join("), (", selectedDeviceIds)}) ) AS vt(a) ) selectedDevices on D1.Id = selectedDevices.a";
            if (isMeasure.Value)
            {
                var isHvs = ((bool)hasHvs.EditValue);
                var isGvs = ((bool)hasGvs.EditValue);
                var isTe = ((bool)hasTe.EditValue);
                var isEe = ((bool)hasEe.EditValue);
                sqlExpression = string.Format(sqlExpression,
                    selectedDeviceIdsInSqlForJoinMainScript,
                    $"\r\nset @beginDate = '{dateBegin}' set @endDate = '{dateEnd}' set @isHvs = {Convert.ToInt32(isHvs)} set @isGvs = {Convert.ToInt32(isGvs)} set @isTe = {Convert.ToInt32(isTe)} set @isEe = {Convert.ToInt32(isEe)}"
                );
            }
            else
            {
                if (analyticTypeValue.Value == 1)
                {
                    var paramString = AnalyticParam1.EditValue as string;
                    if (string.IsNullOrEmpty(paramString) || !Decimal.TryParse(paramString, out decimal paramValue))
                    {
                        this.labelConnectionStatus.EditValue = "Не заполнен параметр";
                        return;
                    }
                    sqlExpression = string.Format(sqlExpression,
                        selectedDeviceIdsInSqlForJoinMainScript,
                        $"\r\nset @beginDate = '{dateBegin}' set @endDate = '{dateEnd}' set @delta = {paramValue}"
                    );
                }
                else if (analyticTypeValue.Value == 2)
                {
                    sqlExpression = string.Format(sqlExpression,
                        selectedDeviceIdsInSqlForJoinMainScript,
                        $"\r\nset @beginDate = '{dateBegin}' set @endDate = '{dateEnd}'"
                    );
                }
            } 

            gridView.ShowLoadingPanel();
            this.labelConnectionStatus.EditValue = "Загрузка...";
            var sw = Stopwatch.StartNew();

            var table = await GetDataForGrid(sqlExpression);
            if (table == null)
            {
                gridView.HideLoadingPanel();
                return;
            }

            var dataSet = new DataSet();
            dataSet.Tables.Add(table);
            gridControl.DataSource = dataSet;
            gridControl.DataMember = _tableName;
            BtnExportExcel.Enabled = true;
            gridView.HideLoadingPanel();
            sw.Stop();
            this.labelConnectionStatus.EditValue = $"Время выполнения запроса {sw.Elapsed.Seconds + sw.Elapsed.Minutes * 60} c";
        }

        /// <summary>
        /// Выгрузка в эксель (событие)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExportExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            string path = $"Экспорт {DateTime.Today:dd.MM.yyyy HH.mm}.xlsx";
            gridControl.ExportToXlsx(path);
            Process.Start(path);
        }

        /// <summary>
        /// Событие выбора чекбокса в дереве объектов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeasureUnitTree_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            var node = e.Node;
            if (node.Checked)
                node.UncheckAll();
            else
                node.CheckAll();
            while (node.ParentNode != null)
            {
                node = node.ParentNode;
                node.CheckState = OneOfChildsIsChecked(node) ? CheckState.Indeterminate : CheckState.Unchecked;
            }

            BtnLoad.Enabled = measureUnitTree.GetAllCheckedNodes().Any();
        }

        /// <summary>
        /// Выбор режима показания за период или аналитика (событие радиобатон)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeasureType_EditValueChanged(object sender, EventArgs e)
        {
            var isMeasure = MeasureType.EditValue as bool?;
            if (!isMeasure.HasValue)
                return;
            if (isMeasure.Value)
            {
                AnalyticType.Enabled = false;
                analyticParams.Visible = false;

                hasGvs.Enabled = true;
                hasHvs.Enabled = true;
                hasTe.Enabled = true;
                hasEe.Enabled = true;

                /*
                А) потребление ХВС<ГВС более чем на ХХХ значение; (на сколько)
                Б) отсутствие потребления ЭЭ при наличии потребления ХВС/ГВС и наоборот; -
                В) отсутствие данных по объекту учета в течении определенного периода времени; (дни)
                Г) отсутствие потребления ХВС, при наличии ГВС;-
                Д) аномальный расход с возможностью выставления верхнего и нижнего уровня диапазона.
                */
            }
            else
            {
                AnalyticType.Enabled = true;
                analyticParams.Visible = true;

                hasGvs.Enabled = false;
                hasHvs.Enabled = false;
                hasTe.Enabled = false;
                hasEe.Enabled = false;
            }
        }

        /// <summary>
        /// Выбор режима аналитики (событие радиобатон)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnalyticType_EditValueChanged(object sender, EventArgs e)
        {
            var typeValue = AnalyticType.EditValue as int?;

            if (!typeValue.HasValue)
                return;

            if (typeValue.Value == 1)
            {
                AnalyticParam1.Visibility = BarItemVisibility.Always;
                AnalyticParam3.Visibility = BarItemVisibility.Never;
            }
            else if (typeValue.Value == 3)
            {
                AnalyticParam1.Visibility = BarItemVisibility.Never;
                AnalyticParam3.Visibility = BarItemVisibility.Always;
            } 
            else
            {
                AnalyticParam1.Visibility = BarItemVisibility.Never;
                AnalyticParam3.Visibility = BarItemVisibility.Never;
            }
        }
    }
}
