using System.ComponentModel.DataAnnotations;

namespace home_work_54_lab_work.ViewModels;

public class RegisterViewModel
{
    [Microsoft.Build.Framework.Required]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Microsoft.Build.Framework.Required]
    [Display(Name = "Username")]
    public string Username { get; set; }
    
    [Microsoft.Build.Framework.Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    
    [Microsoft.Build.Framework.Required]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string PasswordConfirm { get; set; }
}