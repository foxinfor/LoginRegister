using System.Diagnostics.CodeAnalysis;

namespace LoginRegister.Models
{
    public class UserModelView
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        [MaybeNull]
        public string Role { get; set; }
    }
}
