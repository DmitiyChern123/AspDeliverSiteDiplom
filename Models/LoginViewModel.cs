using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Логин обязателен")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

      
    }
}
