using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using WebApplication1.clases;
using WebApplication1.Entities2;
using WebApplication1.Entities2;

namespace WebApplication1.Models
{
    public class EditProductViewModel
    {
        [Required(ErrorMessage = "не верно заполнен продукт ")]
        public ValidateProduct? product { get; set; }

        public List<ProductType>? types { get; set; }

        public List<Category>? categories { get; set; }
      
        public int? selcat { get; set; }
       
        public IFormFile? ImgFile { get; set; }

    }
}
