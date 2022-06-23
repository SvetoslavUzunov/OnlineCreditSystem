namespace OnlineCreditSystem.Models.User;

using System.ComponentModel.DataAnnotations;
using static Data.DataConstants.User;
using static Data.DataConstants.Transaction;

public class UserTransactionFormModel
{
    [Required]
    [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }

    [Range(AmountCreditsMinValue, AmountCreditsMaxValue)]
    [Display(Name = "Credits")]
    public int AmountCredits { get; set; }

    [MaxLength(CommentMaxLength)]
    public string Comment { get; set; }
}
