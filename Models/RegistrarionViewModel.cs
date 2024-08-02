using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RegistrarionViewModel
    {
        [MinLength(2, ErrorMessage = "Имя не может быть короче двух символов")]
        [MaxLength(50, ErrorMessage = "Максимальная длина имени составляет 50 знаков")]
        public string Name { get; set; }
        [MinLength(2, ErrorMessage = "Имя не может быть короче двух символов")]
        [MaxLength(50, ErrorMessage = "Максимальная длина имени составляет 50 знаков")]
        public string Login { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Повторный пароль обязателен")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}
