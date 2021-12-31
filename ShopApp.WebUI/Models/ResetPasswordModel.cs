using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email adresi boş olamaz.")]
        [EmailAddress(ErrorMessage = "Email adresi giriniz")]
        [Display(Name = "Email Adres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre boş olamaz.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
