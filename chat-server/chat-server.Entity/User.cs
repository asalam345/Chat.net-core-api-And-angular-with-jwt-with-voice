using System;

namespace chat_server.Entity
{
    public partial class UserVM
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool ForLogin { get; set; }
    }
}
