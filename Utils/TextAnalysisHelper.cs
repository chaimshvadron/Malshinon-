namespace Malshinon.Utils
{
    public static class TextAnalysisHelper
    {
        public static string GenerateSecretCode()
        {
            // Generate a random 6-character alphanumeric code
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string SecetCode = "";
            for (int i = 0; i < 6; i++)
            {
                SecetCode += chars[random.Next(chars.Length)];
            }
            return SecetCode;
       }
    }
}