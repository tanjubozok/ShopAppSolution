using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ForgetPasswordModel
    {
        [Required(ErrorMessage = "Email adresi boş olamaz.")]
        [EmailAddress(ErrorMessage = "Email adresi giriniz")]
        [Display(Name = "Email Adres")]
        public string EmailAddress { get; set; }
    }
}
