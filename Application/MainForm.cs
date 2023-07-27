using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList.Nodes;
using Application.Models.Data;
using Application.Models.Options;
using System;
using System.Collections.Generic;
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

        private static List<string> _columnTitles = new List<string>();
        private static List<List<DataItemModel>> _data = new List<List<DataItemModel>>();

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Начальна загрузка
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

            (comboBoxServerSelect.Edit as RepositoryItemComboBox)
                .Items
                .AddRange(_options.Option.Select(x => x.Name).ToList());
        }

        /// <summary>
        /// Выбор сервера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void comboBoxServerSelect_EditValueChanged(object sender, EventArgs e)
        {
            SetDefaultData();
            BtnLoad.Enabled = false;
            btnExportExcel.Enabled = false;

            var serverName = (string)comboBoxServerSelect.EditValue;
            string messageString = null;
            this.labelConnectionStatus.EditValue = "Подключение...";

            var option = _options.Option.FirstOrDefault(x => x.Name == serverName);
            if (option is null)
            {
                this.labelConnectionStatus.EditValue = "Произошла ошибка при выборе строки подключения после выбора сервера";
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
                            {
                                messageString = "Подключено к БД";
                            }
                            else
                            {
                                this.labelConnectionStatus.EditValue = _messageErrorConnection;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.labelConnectionStatus.EditValue = $"{_messageErrorConnection} {ex.Message}";
                return;
            }

            string path = "Scripts/energy_tree.sql";
            if (!File.Exists(path))
            {
                this.labelConnectionStatus.EditValue = $"Отсутствует файл {path}";
                return;
            }

            string sqlExpression;
            using (StreamReader textReader = new StreamReader(path))
            {
                sqlExpression = await textReader.ReadToEndAsync();
                if (string.IsNullOrEmpty(sqlExpression))
                {
                    this.labelConnectionStatus.EditValue = $"Файл {path} пустой";
                    return;
                }
            }

            var columnTitles = new List<string>();
            var data = new List<List<DataItemModel>>();
            var table = new DataTable(_tableName);
            try
            {
                using (var connection = new SqlConnection(_selectedOptions.ConnectionString))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand(sqlExpression, connection)
                    {
                        Connection = connection
                    };
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var column = new DataColumn
                                {
                                    DataType = typeof(string),
                                    ColumnName = reader.GetName(i),
                                    AutoIncrement = false,
                                    Caption = reader.GetName(i),
                                    ReadOnly = false,
                                    Unique = false
                                };
                                table.Columns.Add(column);
                            }
                            while (reader.Read()) // построчно считываем данные
                            {
                                DataRow dataRow;
                                dataRow = table.NewRow();

                                var rowData = new List<DataItemModel>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dataRow[reader.GetName(i)] = reader.GetValue(i).ToString();
                                }
                                table.Rows.Add(dataRow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.labelConnectionStatus.EditValue = $"{_messageErrorConnection} {ex.Message}";
                return;
            }

            var dataSet = new DataSet();
            dataSet.Tables.Add(table);
            measureUnitTree.DataSource = dataSet;
            measureUnitTree.DataMember = _tableName;

            measureUnitTree.KeyFieldName = "Device_id";
            measureUnitTree.ParentFieldName = "MasterID";

            this.labelConnectionStatus.EditValue = messageString;
        }

        /// <summary>
        /// Загрузить данные
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

            //Загрузка данных
            string path = $"Scripts/{_selectedOptions.MainScriptName}.sql";
            if (!File.Exists(path))
            {
                this.labelConnectionStatus.EditValue = $"Отсутствует файл {path}";
                return;
            }

            string sqlExpression;
            using (StreamReader textReader = new StreamReader(path))
            {
                sqlExpression = await textReader.ReadToEndAsync();
                if (string.IsNullOrEmpty(sqlExpression))
                {
                    this.labelConnectionStatus.EditValue = $"Файл {path} пустой";
                    return;
                }
            }

            gridView.ShowLoadingPanel();

            var dateBegin = ((DateTime)DateBegin.EditValue).ToString("yyyyMMdd");
            var dateEnd = ((DateTime)DateEnd.EditValue).ToString("yyyyMMdd");

            var isHvs = ((bool)hasHvs.EditValue);
            var isGvs = ((bool)hasGvs.EditValue);
            var isTe = ((bool)hasTe.EditValue);
            var isEe = ((bool)hasEe.EditValue);

            var selectedDeviceIdsInSqlForJoinMainScript = $"\r\n INNER JOIN (SELECT * FROM (VALUES ({string.Join("), (", selectedDeviceIds)}) ) AS vt(a) ) selectedDevices on D1.Id = selectedDevices.a";
            sqlExpression = string.Format(sqlExpression, 
                selectedDeviceIdsInSqlForJoinMainScript, 
                $"\r\nset @beginDate = '{dateBegin}' set @endDate = '{dateEnd}' set @isHvs = {Convert.ToInt32(isHvs)} set @isGvs = {Convert.ToInt32(isGvs)} set @isTe = {Convert.ToInt32(isTe)} set @isEe = {Convert.ToInt32(isEe)}"
            );

            this.labelConnectionStatus.EditValue = "Загрузка...";
            var sw = Stopwatch.StartNew();

            //загрузка данных
            try
            {
                using (var connection = new SqlConnection(_selectedOptions.ConnectionString))
                {
                    sw.Start();
                    await connection.OpenAsync();
                    var command = new SqlCommand(sqlExpression, connection)
                    {
                        Connection = connection,
                        CommandTimeout = 60 * 60
                    };

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                _columnTitles.Add(reader.GetName(i));
                            }

                            while (reader.Read()) // построчно считываем данные
                            {
                                var rowData = new List<DataItemModel>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var columnData = new DataItemModel
                                    {
                                        DataObject = reader.GetValue(i),
                                        DataType = reader.GetProviderSpecificFieldType(i),
                                        ColumnNumber = i,
                                        ColumnName = reader.GetName(i)
                                    };
                                    rowData.Add(columnData);
                                }
                                _data.Add(rowData);
                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                this.labelConnectionStatus.EditValue = $"{_messageErrorConnection} {ex.Message}";
                gridView.HideLoadingPanel();
                return;
            }

            //данные загружены, пихаем их в грид
            var table = new DataTable(_tableName);
            foreach (var col in _columnTitles)
            {
                var column = new DataColumn
                {
                    DataType = typeof(string),
                    ColumnName = col,
                    AutoIncrement = false,
                    Caption = col,
                    ReadOnly = false,
                    Unique = false
                };
                table.Columns.Add(column);
            }

            foreach (var row in _data)
            {
                var dataRow = table.NewRow();
                foreach (var col in row)
                {
                    dataRow[col.ColumnName] = col.DataObject.ToString();
                }
                table.Rows.Add(dataRow);
            }

            var dataSet = new DataSet();
            dataSet.Tables.Add(table);
            gridControl.DataSource = dataSet;
            gridControl.DataMember = _tableName;

            btnExportExcel.Enabled = true;
            gridView.HideLoadingPanel();
            this.labelConnectionStatus.EditValue = "";


            sw.Stop();
            this.labelConnectionStatus.EditValue = $"Время выполнения запроса {sw.Elapsed.Seconds + sw.Elapsed.Minutes * 60} c";
        }

        /// <summary>
        /// Выгрузка в эксель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_ItemClick(object sender, ItemClickEventArgs e)
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
        private void measureUnitTree_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            TreeListNode node = e.Node;
            if (node.Checked)
            {
                node.UncheckAll();
            }
            else
            {
                node.CheckAll();
            }
            while (node.ParentNode != null)
            {
                node = node.ParentNode;
                bool oneOfChildIsChecked = OneOfChildsIsChecked(node);
                if (oneOfChildIsChecked)
                {
                    node.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    node.CheckState = CheckState.Unchecked;
                }
            }

            if (measureUnitTree.GetAllCheckedNodes().Any())
                BtnLoad.Enabled = true;
            else
                BtnLoad.Enabled = false;
        }


        #region not events
        private bool OneOfChildsIsChecked(TreeListNode node)
        {
            bool result = false;
            foreach (var item in node.Nodes.Cast<TreeListNode>())
            {
                if (item.CheckState == CheckState.Checked)
                {
                    result = true;
                }
            }
            return result;
        }

        private void SetDefaultData()
        {
            _columnTitles = new List<string>();
            _data = new List<List<DataItemModel>>();
        }

        #endregion
    }
}
