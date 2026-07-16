using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.User.Query.GetUser
{
    public class GetUserDTOs
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public bool IsActive { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
