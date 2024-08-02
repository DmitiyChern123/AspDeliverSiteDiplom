using WebApplication1.Entities2;
using WebApplication1.Entities2;

namespace WebApplication1.Models
{
    public class EditTypesViewModel
    {
        public List<ProductType>? types { get; set; }
        public ProductType ? newtype { get; set; }
        public int productid { get; set; }
    }
}
