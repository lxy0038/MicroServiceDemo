using Common.Insfratructure.Enums;
using Common.Insfratructure.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DataAccess.EF.Extensions
{
    public static class DataBaseTypeExtension
    {
        public static DataBaseType DBType(this DbContext dbcontext)
        {
            DataBaseType type = DataBaseType.Oracle;
            var databaseName = ConfigurationManager.AppSettings["DataBaseType"].Trim().ToLower();
            switch (databaseName)
            {
                case "mysql":
                    type = DataBaseType.MySQL;
                    break;
                case "oracle":
                    type = DataBaseType.Oracle;
                    break;
                case "postgresql":
                    type = DataBaseType.Postgresql;
                    break;
                default:
                    type = DataBaseType.SQL;
                    break;

            }

            return type;
        }
    }
}
