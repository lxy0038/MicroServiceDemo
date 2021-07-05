// <copyright file="EntityBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Lib.EF.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 实体基类.
    /// </summary>
    public class EntityBase
    {

        public EntityBase()
        {
            this.create_time = DateTime.Now;
            this.modify_time = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets 主键ID.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime? create_time { get; set; }

        /// <summary>
        /// 最后修改时间.
        /// </summary>
        public DateTime? modify_time { get; set; }

        /// <summary>
        /// 组织ID.
        /// </summary>
        public string org_id { get; set; }

        /// <summary>
        /// 组织实体类型,用于组织过滤.
        /// </summary>
        public int org_entity_type { get; set; }

        /// <summary>
        /// 是否删除.
        /// </summary>
        public bool is_deleted { get; set; }

        /// <summary>
        /// 行版本.
        /// </summary>
        public int row_version { get; set; }
    }
}
