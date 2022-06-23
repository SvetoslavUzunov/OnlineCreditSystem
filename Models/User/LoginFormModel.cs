namespace OnlineCreditSystem.Models.User;

using System.ComponentModel.DataAnnotations;

public class LoginFormModel
{
    public int Id { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Password not match!")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }
}
