using System.ComponentModel.DataAnnotations;
using WebApplication1.Entities2;
using WebApplication1.Entities2;

namespace WebApplication1.clases
{
    public class ValidateProduct
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "имя не может быть пустым")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "имя не может быть пустым")]
        public string? Opis { get; set; }
        [Range(1, 9999)]
        public int? Price { get; set; }

        public int? IdCategory { get; set; }

        public string? Img { get; set; }
        public bool Is_hidden { get; set; }
        public virtual ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();
        public ValidateProduct()
        {


        }
        public ValidateProduct(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.Opis = product.Opis;
            this.Price = product.Price;
            this.IdCategory = product.IdCategory;
            this.Img = product.Img;
            this.ProductTypes = product.ProductTypes;
        }

    }
}
