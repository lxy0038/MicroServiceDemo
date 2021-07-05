using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Insfratructure.Models
{
    public class ModelBase
    {
        public string ID { get; set; } = Guid.NewGuid().ToString().ToLower();

        public bool IsDelete { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime ModifiedTime { get; set; } = DateTime.Now;
        public string OrgID { get; set; }
    }
}
