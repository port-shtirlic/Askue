using System;

namespace Application.Models.Data
{
    internal class DataItemModel
    {
        public Type DataType { get; set; }

        public object DataObject { get; set; }

        public int ColumnNumber { get; set; }

        public string ColumnName { get; set; }
    }
}
