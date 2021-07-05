using Demo.Models;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Service.Service
{
    public class ProductService: IProductService, ICapSubscribe
    {
        private readonly ILogger<ProductService> _logger;
        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger; 
        }
        [CapSubscribe("order.get")]
        public void GetOrderPulish(OrderParam model)
        {
            _logger.LogInformation("order: "+ DateTime.Now+ "   "+model.Time+" " + model.Id);
        }

    }
}
