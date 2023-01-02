using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace API.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
        public bool IsAuthenticated { get; set; }
        
        public bool EmailConfirmed { get; set; }
        public List<ClaimsDto>  Claims { get; set; }
    }
}
