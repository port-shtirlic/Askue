using Application.Models.Data;
using Application.Models.Options;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Application
{
    public partial class MainForm
    {
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

        /// <summary>
        /// Очистить грид от данны
        /// </summary>
        private void SetDefaultData()
        {
            gridControl.DataSource = null;
            gridView.Columns.Clear();
        }


        /// <summary>
        /// Выполнить запрос, результат вернуть в виде DataTable
        /// </summary>
        /// <param name="sqlExpression"></param>
        /// <returns></returns>
        private async Task<DataTable> GetDataForGrid(string sqlExpression)
        {
            var table = new DataTable(_tableName);
            try
            {
                using (var connection = new SqlConnection(_selectedOptions.ConnectionString))
                {
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
                                table.Columns.Add(new DataColumn
                                {
                                    DataType = typeof(string),
                                    ColumnName = reader.GetName(i),
                                    AutoIncrement = false,
                                    Caption = reader.GetName(i),
                                    ReadOnly = false,
                                    Unique = false
                                });
                            }

                            // построчно считываем данные
                            while (reader.Read()) 
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
                return null;
            }
            return table;
        }

        /// <summary>
        /// Получить скрит из файла скриптов
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetSqlExpressionFromFile(OptionScriptType type)
        {
            string scriptName;
            switch (type)
            {
                case OptionScriptType.Main:
                    scriptName = _selectedOptions.MainScriptName;
                    break;
                case OptionScriptType.Tree:
                    scriptName = _selectedOptions.Tree;
                    break;
                case OptionScriptType.AnalyticHvsLessGvs:
                    scriptName = _selectedOptions.AnalyticHvsLessGvs;
                    break;
                case OptionScriptType.AnalyticNoEe:
                    scriptName = _selectedOptions.AnalyticNoEe;
                    break;
                case OptionScriptType.AnalyticNoMeasures:
                    scriptName = _selectedOptions.AnalyticNoMeasures;
                    break;
                case OptionScriptType.AnalyticNoHvs:
                    scriptName = _selectedOptions.AnalyticNoHvs;
                    break;
                case OptionScriptType.AnalyticAbnormal:
                    scriptName = _selectedOptions.AnalyticAbnormal;
                    break;
                case OptionScriptType.AnalyticNegative:
                    scriptName = _selectedOptions.AnalyticNegative;
                    break;
                case OptionScriptType.GeneralScript:
                    scriptName = _selectedOptions.GeneralScript;
                    break;
                default:
                    scriptName = null;
                    break;
            }

            var path = $"Scripts/{scriptName}.sql";
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                this.labelConnectionStatus.EditValue = $"Отсутствует файл {path}";
                return null;
            }
            string sqlExpression;
            using (StreamReader textReader = new StreamReader(path))
            {
                sqlExpression = await textReader.ReadToEndAsync();
                if (string.IsNullOrEmpty(sqlExpression))
                {
                    this.labelConnectionStatus.EditValue = $"Файл {path} пустой";
                    return null;
                }
            }
            return sqlExpression;
        }

        private void SetBtnEnabled()
        {
            if (!measureUnitTree.GetAllCheckedNodes().Any())
            {
                BtnLoad.Enabled = false;
                return;
            }

            var isMeasure = MeasureType.EditValue as bool?;
            if (isMeasure == null || !isMeasure.Value)
            {
                if (AnalyticType.EditValue == null || !(AnalyticType.EditValue is OptionScriptType))
                {
                    BtnLoad.Enabled = false;
                    return;
                }
            }
            BtnLoad.Enabled = true;
        }
    }
}
