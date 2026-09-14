using System.ComponentModel.DataAnnotations;

namespace home_work_54_lab_work.ViewModels;

public class LoginViewModel
{
    [Microsoft.Build.Framework.Required]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Microsoft.Build.Framework.Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
    
    [Display(Name = "Запомнить?")]
    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }
}