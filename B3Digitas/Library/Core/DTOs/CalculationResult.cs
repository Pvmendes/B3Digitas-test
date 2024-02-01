using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.DTOs
{
    public class CalculationResult
    {
        public Guid Id { get; set; }
        public List<Order> UsedOrders { get; set; }
        public float RequestedQuantity { get; set; }
        public string OperationType { get; set; }
        public decimal TotalCost { get; set; }
    }
}
