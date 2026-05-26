using System.Text.RegularExpressions;

namespace mini.ecommerce.api.Domain.Core.Model.VM.Validation
{
    public static partial class MyRegex
    {
        [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public static partial Regex ValidaEmailRegex();

        public static bool ValidaEmail(string email)
        {
            return ValidaEmailRegex().IsMatch(email);
        }
    }
}
