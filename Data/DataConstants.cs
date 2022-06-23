namespace OnlineCreditSystem.Data;

public class DataConstants
{
    public class User
    {
        public const int FirstNameMinLength = 1;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMinLength = 1;
        public const int LastNameMaxLength = 50;
        public const int PhoneNumberMinLength = 10;
        public const int PhoneNumberMaxLength = 15;
        public const int CreditsMinValue = 0;
        public const int CreditsMaxValue = 10000;
        public const int AmountCreditsMinValue = 1;
        public const int AmountCreditsMaxValue = 1;
    }

    public class Transaction
    {
        public const int CommentMaxLength = 255;
    }
}
