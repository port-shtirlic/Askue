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
                case OptionScriptType.Analytic1:
                    scriptName = _selectedOptions.Analytic1;
                    break;
                case OptionScriptType.Analytic2:
                    scriptName = _selectedOptions.Analytic2;
                    break;
                case OptionScriptType.Analytic3:
                    scriptName = _selectedOptions.Analytic3;
                    break;
                case OptionScriptType.Analytic4:
                    scriptName = _selectedOptions.Analytic4;
                    break;
                case OptionScriptType.Analytic5:
                    scriptName = _selectedOptions.Analytic5;
                    break;
                case OptionScriptType.Analytic6:
                    scriptName = _selectedOptions.Analytic6;
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
    }
}
