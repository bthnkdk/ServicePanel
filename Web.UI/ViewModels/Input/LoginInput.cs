using System.ComponentModel.DataAnnotations;

namespace Web.UI.ViewModels
{
    public class LoginInput
    {
        [Display(Name = "E-Posta")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string Email { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Password { get; set; }

        [Display(Name = "Güvenlik Kodu")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Code { get; set; }
    }

    public class ForgotPasswordInput
    {
        [Display(Name = "E-Posta")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string Email { get; set; }
    }

    public class ResetPasswordInput
    {
        [Display(Name = "E-Posta")]
        [Required(ErrorMessage = "Bu alan gerekli")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string Email { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        public string Password { get; set; }

        [Display(Name = "Parola (tekrar)")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        [Compare("Password", ErrorMessage = "Parola alanları uyuşmuyor")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePasswordInput
    {
        [Display(Name = "Mevcut Parola")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        [UIHint("Password")]
        public string OldPassword { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        [UIHint("Password")]
        public string Password { get; set; }

        [Display(Name = "Parola (tekrar)")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz")]
        [Compare("Password", ErrorMessage = "Parola alanları uyuşmuyor")]
        [UIHint("Password")]
        public string ConfirmPassword { get; set; }
    }
}