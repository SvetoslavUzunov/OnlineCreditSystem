namespace OnlineCreditSystem.Models.User;

public class UserViewModel
{
    public int Id { get; init; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public int Credits { get; set; }
}
