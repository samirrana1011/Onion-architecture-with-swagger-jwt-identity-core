using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OA_DataAccess
{
    public class User : IdentityUser
    {
        public string FName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
