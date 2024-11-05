using System.Text.RegularExpressions;

namespace app.Domain.Agregate.ObjectValues
{
    public class Email
    {
        private string _email;
        public Email(string email)
        {
            if (IsValidEmail(email))
            {
                _email = email;
            }
            else
            {
                throw new ArgumentException("Email is not valid");
            }
        }
        public string Value => _email;
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@(gmail\.com|hotmail\.com)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
