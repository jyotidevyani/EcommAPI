using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace EcommerceAPI.Models
{
    public class clsParameter
    {
        public DbType DbType { get; set; }
        public ParameterDirection Direction { get; set; }
        public bool IsNullable { get; }
        public string ParameterName { get; set; }
        public string SourceColumn { get; set; }
        public DataRowVersion SourceVersion { get; set; }
        public object Value { get; set; }
        public int size { get; set; }

    }
}