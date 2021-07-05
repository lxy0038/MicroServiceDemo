using System;

namespace Demo.Models
{
    public class ProductParam
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string OrgId { get; set; }

        public string Remark { get; set; }
    }

    public class ProductModel
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string OrgId { get; set; }

        public string Remark { get; set; }

    }


    public class OrderParam
    {
        public OrderParam()
        {
            this.Time = DateTime.Now;
        }

        public string Id { get; set; }

        public string Code { get; set; }

        public string OrgId { get; set; }
        public string Remark { get; set; }

        public DateTime Time { get; set; }
    }

    public class OrderModel
    {
        public string Id { get; set; }

        public string Code { get; set; } 

        public string OrgId { get; set; }

        public string ProductId { get; set; }

        public string Remark { get; set; }

        public ProductModel Product { get; set; }

    }
}
