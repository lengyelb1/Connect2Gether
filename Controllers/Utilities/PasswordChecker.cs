namespace Connect2Gether_API.Controllers.Utilities
{
    public static class PasswordChecker
    {

        public static bool CheckPassword(string password)
        {
            string specialCharacters = "!@#$%^&*()-_+={}[]|\\:;\"'<>,.?/";

            if (password.Length == 0 || password.Length < 8)
                return false;

            int countUpperLetters = 0;
            int countSpecialCharacters = 0;
            int countNumbers = 0;

            foreach (char c in password)
            {
                if (Char.IsUpper(c))
                {
                    countUpperLetters++;
                }
                if (specialCharacters.Contains(c))
                {
                    countSpecialCharacters++;
                }
                if (Char.IsDigit(c))
                {
                    countNumbers++;
                }
            }
            if (countUpperLetters < 1 || countSpecialCharacters < 1 || countNumbers < 1)
            {
                return false;
            }


            return true;
        }
    }
}
