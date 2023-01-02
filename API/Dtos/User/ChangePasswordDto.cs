using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ChangePasswordDto
    {
        public int id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public bool isReset { get; set; }

    }
    public class ResetPasswordDto
    {
        public string Email { get; set; }

    }
}
