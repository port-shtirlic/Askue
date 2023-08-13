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

            MeasureType.Enabled = true;

            #region загрузка дерева ПУ
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

            #endregion

            #region Типы аналитики
            analyticRadioGroup.Items.Clear();
            if (!string.IsNullOrEmpty(_selectedOptions.AnalyticHvsLessGvs))
            {
                analyticRadioGroup.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem
                {
                    Value = OptionScriptType.AnalyticHvsLessGvs,
                    Enabled = true,
                    Description = "потребленеие ХВС<ГВС"
                });
            }
            if (!string.IsNullOrEmpty(_selectedOptions.AnalyticNoHvs))
            {
                analyticRadioGroup.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem
                {
                    Value = OptionScriptType.AnalyticNoHvs,
                    Enabled = true,
                    Description = "отсутствие ХВС, при наличии ГВС"
                });
            }
            if (!string.IsNullOrEmpty(_selectedOptions.AnalyticNegative))
            {
                analyticRadioGroup.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem
                {
                    Value = OptionScriptType.AnalyticNegative,
                    Enabled = true,
                    Description = "отрицательная разница ХВС/ГВС"
                });
            }
            if (!string.IsNullOrEmpty(_selectedOptions.AnalyticNoEe))
            {
                analyticRadioGroup.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem
                {
                    Value = OptionScriptType.AnalyticNoEe,
                    Enabled = true,
                    Description = "отсутствие потребленеие ЭЭ"
                });
            }
            if (!string.IsNullOrEmpty(_selectedOptions.AnalyticNoMeasures))
            {
                analyticRadioGroup.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem
                {
                    Value = OptionScriptType.AnalyticNoMeasures,
                    Enabled = true,
                    Description = "отсутствие данных по объекту"
                });
            }
            if (!string.IsNullOrEmpty(_selectedOptions.AnalyticAbnormal))
            {
                analyticRadioGroup.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem
                {
                    Value = OptionScriptType.AnalyticAbnormal,
                    Enabled = true,
                    Description = "аномальный расход"
                });
            }

            if (analyticRadioGroup.Items.Any())
                AnalyticGroupType.Visible = true;
            else
                AnalyticGroupType.Visible = false;
            #endregion
        }

        /// <summary>
        /// Загрузить данные (событие по кнопке)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnLoad_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedDeviceIds = measureUnitTree
                .GetAllCheckedNodes()
                .Select(x => x.GetValue("Device_id").ToString());
            if (!selectedDeviceIds.Any())
                return;

            SetDefaultData();

            var isMeasure = MeasureType.EditValue as bool?;
            var analyticTypeValue = AnalyticType.EditValue as OptionScriptType?;

            if (!isMeasure.HasValue)
                return;

            OptionScriptType? scriptType = null;
            if (isMeasure.Value)
                scriptType = OptionScriptType.Main;
            else
            {
                if (!analyticTypeValue.HasValue)
                {
                    this.labelConnectionStatus.EditValue = "Включен режим аналитики, но не выбран тип";
                    return;
                }
                scriptType = analyticTypeValue;

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
                if (analyticTypeValue.Value == OptionScriptType.AnalyticHvsLessGvs)
                {
                    var paramString = AnalyticParam1.EditValue as string;
                    if (string.IsNullOrEmpty(paramString) || !Decimal.TryParse(paramString, out decimal paramValue))
                    {
                        this.labelConnectionStatus.EditValue = "Не заполнен параметр";
                        return;
                    }
                    sqlExpression = string.Format(sqlExpression,
                        selectedDeviceIdsInSqlForJoinMainScript,
                        $"\r\nset @beginDate = '{dateBegin}' set @endDate = '{dateEnd}' set @delta = {paramValue}"                   );
                }
                else if (analyticTypeValue.Value == OptionScriptType.AnalyticAbnormal)
                {
                    var paramMinString = AnalyticParam5min.EditValue as string;
                    var paramMaxString = AnalyticParam5max.EditValue as string;
                    if (string.IsNullOrEmpty(paramMinString) ||
                        string.IsNullOrEmpty(paramMaxString) ||
                        !Decimal.TryParse(paramMinString, out decimal paramMinValue) ||
                        !Decimal.TryParse(paramMaxString, out decimal paramMaxValue)
                    )
                    {
                        this.labelConnectionStatus.EditValue = "Не заполнен параметр";
                        return;
                    }
                    sqlExpression = string.Format(sqlExpression,
                        selectedDeviceIdsInSqlForJoinMainScript,
                        $"\r\nset @beginDate = '{dateBegin}' set @endDate = '{dateEnd}' set @max = {paramMaxValue} set @min = {paramMinValue}"
                    );
                }
                else
                {
                    sqlExpression = string.Format(sqlExpression,
                        selectedDeviceIdsInSqlForJoinMainScript,
                        $"\r\nset @beginDate = '{dateBegin}' set @endDate = '{dateEnd}'"
                    );
                }
                //else if (analyticTypeValue.Value == 2)
                //{
                //    sqlExpression = string.Format(sqlExpression,
                //        selectedDeviceIdsInSqlForJoinMainScript,
                //        $"\r\nset @beginDate = '{dateBegin}' set @endDate = '{dateEnd}'"
                //    );
                //}
                //else if (analyticTypeValue.Value == 4)
                //{
                //    sqlExpression = string.Format(sqlExpression,
                //        selectedDeviceIdsInSqlForJoinMainScript,
                //        $"\r\nset @beginDate = '{dateBegin}' set @endDate = '{dateEnd}'"
                //    );
                //}

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

            SetBtnEnabled();
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
                AnalyticGroupParams1.Visible = false;
                AnalyticGroupParams2.Visible = false;

                hasGvs.Enabled = true;
                hasHvs.Enabled = true;
                hasTe.Enabled = true;
                hasEe.Enabled = true;

                /*
                +А) потребление ХВС<ГВС более чем на ХХХ значение; (на сколько)
                +-Б) отсутствие потребления ЭЭ при наличии потребления ХВС/ГВС и наоборот;
                -В) отсутствие данных по объекту учета в течении определенного периода времени;
                +Г) отсутствие потребления ХВС, при наличии ГВС;
                +-Д) аномальный расход с возможностью выставления верхнего и нижнего уровня диапазона.
                +E) отрицательная разница ХВС/ГВС
                */
            }
            else
            {
                AnalyticType.Enabled = true;

                AnalyticGroupParams1.Visible = true;
                AnalyticGroupParams2.Visible = true;
                hasGvs.Enabled = false;
                hasHvs.Enabled = false;
                hasTe.Enabled = false;
                hasEe.Enabled = false;                
            }
            SetBtnEnabled();
        }

        /// <summary>
        /// Выбор режима аналитики (событие радиобатон)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnalyticType_EditValueChanged(object sender, EventArgs e)
        {
            var typeValue = AnalyticType.EditValue as OptionScriptType?;
            if (!typeValue.HasValue)
                return;

            SetBtnEnabled();

            if (typeValue.Value == OptionScriptType.AnalyticHvsLessGvs)
            {
                AnalyticParam1.Visibility = BarItemVisibility.Always;
                AnalyticParam5min.Visibility = BarItemVisibility.Never;
                AnalyticParam5max.Visibility = BarItemVisibility.Never;
            }
            else if (typeValue.Value == OptionScriptType.AnalyticAbnormal)
            {
                AnalyticParam1.Visibility = BarItemVisibility.Never;
                AnalyticParam5min.Visibility = BarItemVisibility.Always;
                AnalyticParam5max.Visibility = BarItemVisibility.Always;
            }
            else
            {
                AnalyticParam1.Visibility = BarItemVisibility.Never;
                AnalyticParam5min.Visibility = BarItemVisibility.Never;
                AnalyticParam5max.Visibility = BarItemVisibility.Never;
            }
        }
    }
}
