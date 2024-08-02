using WebApplication1.Entities2;
using WebApplication1.Entities2;

namespace WebApplication1.Models
{
    public class HomeModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set;}
        public List<ProductType> hasinkorzina { get; set; }
        public int? SelectedCategoryId { get; set; }
        public string SortOrder { get; set; }
        public string SearchQuery { get; set; }
    }
}
