namespace OnlineCreditSystem.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Data.DataConstants.User;

public class User
{
    public int Id { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Password not match!")]
    public string ConfirmPassword { get; set; }

    public int Credits { get; set; }

    public virtual ICollection<Transaction>? Transactions { get; set; } = new HashSet<Transaction>();
}
