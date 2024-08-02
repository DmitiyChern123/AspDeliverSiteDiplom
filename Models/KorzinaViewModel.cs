using System.ComponentModel.DataAnnotations;

using WebApplication1.Entities2;

namespace WebApplication1.Models
{
    public class KorzinaViewModel
    {
        public List<Korzina>? korzinas { get; set; }
        public List<ProductType>? products { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = " ")]
        [EmailAddress(ErrorMessage = "Не верный формат почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = " ")]
        [RegularExpression(@"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$", ErrorMessage = "Не верный формат номера")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Payment type is required")]
        public string PaymentType { get; set; }
        public bool Is_need_devices { get; set; }
        public int order_sum { get; set; } //сумма заказа
        public int cur_bonuses { get; set; } //текущие бонусы
        public int rem_bonuses { get; set; } // списать бонусы
    }
}
