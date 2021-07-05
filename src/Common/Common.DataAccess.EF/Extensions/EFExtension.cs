using Common.DataAccess.EF.DBUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.DataAccess.EF.Extensions
{
    public static class EFExtension
    {

        public static bool IsWin()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static DataTable ToDataTable<T>(this IList<T> entitys, bool hasKey = false)
        {
            // 检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }

            // 取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties().Where(o =>
                                !o.PropertyType.Name.ToLower().Contains("icollection") &&
                                o.PropertyType.BaseType.Name != "ModelBase").ToArray();

            // 生成DataTable的structure
            // 生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                Type colType = entityProperties[i].PropertyType;

                if (colType.IsGenericType && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }

                DataColumn dc = new DataColumn(entityProperties[i].Name, colType);
                if (hasKey && entityProperties[i].CustomAttributes.Where(p => p.AttributeType == typeof(KeyAttribute)).Any())
                {
                    dc.Unique = true;
                }

                dt.Columns.Add(dc);
            }

            // 将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                // 检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }

                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }

                dt.Rows.Add(entityValues);
            }

            return dt;
        }

        public static string ToCsv(this DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0)
                    {
                        sb.Append(",");
                    }

                    if (colum.DataType == typeof(string))
                    {
                        var vStr = row[colum].ToString();
                        if (vStr.Contains("\""))
                        {
                            vStr = vStr.Replace("\"", "\"\"");
                        }
                        if (vStr.Contains(",") || vStr.Contains("\r\n") || vStr.Contains("\n"))
                        {
                            vStr = $"\"{vStr}\"";
                        }
                        sb.Append(vStr);
                    }
                    else
                    {
                        sb.Append(row[colum]);
                    }
                }

                sb.Append(EFExtension.IsWin() ? "\r\n" : "\n");
            }

            return sb.ToString();
        }

        public static EntityMap GetEntityMap(Type type, DbContext dbContext)
        {
            var entityMap = new EntityMap();
            var mapping = dbContext.Model.FindEntityType(type);
            string tableName = mapping.GetTableName();
            var properties = mapping.GetDeclaredProperties();
            foreach (var item in properties)
            {
                Property property = (Property)item;
                if (property.Keys != null && property.Keys.Count > 0)
                {
                    entityMap.KeyMaps.Add(new PropertyMap() { PropertyName = item.Name, ColumnName = item.GetColumnName() });
                }

                entityMap.PropertyMaps.Add(new PropertyMap() { PropertyName = item.Name, ColumnName = item.GetColumnName() });
            }

            entityMap.TableName = tableName;
            return entityMap;
        }

        public static string GetMysqlUpdateSqls<T>(List<T> entities, CommonContext dbContext)
        {
            string sql = string.Empty;
            if (entities == null || entities.Count == 0)
            {
                return sql;
            }

            var entityMap = GetEntityMap(typeof(T), dbContext);

            var listSql = new List<string>();
            foreach (var entity in entities)
            {
                string pkName = entityMap.KeyMaps.FirstOrDefault().ColumnName;

                if (string.IsNullOrEmpty(pkName))
                {
                    return string.Empty;
                }

                string pkValue = string.Empty;

                StringBuilder sb = new StringBuilder();
                sb.Append("update ");
                sb.Append("IOTMaterial." + entityMap.TableName);
                sb.Append(" set ");
                Type type = entity.GetType();
                PropertyInfo[] props = type.GetProperties();
                List<string> paraList = new List<string>();
                foreach (var prop in props)
                {
                    if (prop.Name == (string)pkName)
                    {
                        pkValue = (string)prop.GetValue(entity);
                    }
                    else
                    {
                        paraList.Add(GetUpdatePara(prop, entity));
                    }
                }

                if (paraList.Count == 0)
                {
                    return string.Empty;
                }

                sb.Append(string.Join(",", paraList));

                if (string.IsNullOrEmpty(pkValue))
                {
                    throw new Exception("主键不能为空");
                }

                sb.Append(" where ");
                sb.Append(pkName);
                sb.Append(" = ");
                sb.AppendFormat("'{0}';", pkValue);

                listSql.Add(sb.ToString());
            }

            sql = string.Join("\r", listSql);
            return sql;
        }

        private static string GetUpdatePara<T>(PropertyInfo property, T entity)
        {
            StringBuilder sb = new StringBuilder();
            var propertyValue = property.GetValue(entity);
            if (property.PropertyType.Name == "Boolean")
            {
                if (propertyValue.ToString().ToLower() == "false")
                {
                    propertyValue = 0;
                }
                else
                {
                    propertyValue = 1;
                }
            }

            sb.AppendFormat(" {0}='{1}' ", property.Name, propertyValue);
            return sb.ToString();
        }
    }
}
