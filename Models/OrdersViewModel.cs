
using WebApplication1.Entities2;

namespace WebApplication1.Models
{
    public class OrdersViewModel
    {
        public List<Order> Orders { get; set; }
        public List<StatusOrder> statuses { get; set; }
        public List<Courier> courses { get; set; }
    }
}
