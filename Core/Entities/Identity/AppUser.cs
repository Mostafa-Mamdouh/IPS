using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("AspNetUsers")]
    public partial class AppUser : IdentityUser<int>
    {
    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
