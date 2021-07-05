using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DataAccess.EF.DBUtility
{
    public class EntityMap
    {
        public EntityMap()
        {
            KeyMaps = new List<PropertyMap>();
            PropertyMaps = new List<PropertyMap>();
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<PropertyMap> PropertyMaps { get; set; }
        /// <summary>
        /// 主键列表
        /// </summary>
        public List<PropertyMap> KeyMaps { get; set; }
    }

    public class PropertyMap
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 数据库字段名称
        /// </summary>
        public string ColumnName { get; set; }
    }
}
