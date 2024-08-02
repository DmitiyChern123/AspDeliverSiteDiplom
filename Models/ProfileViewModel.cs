using WebApplication1.Entities2;

namespace WebApplication1.Models
{
    public class ProfileViewModel
    {
        public User user { get; set; }
        public List<Order> orders { get; set; }
    }
}
