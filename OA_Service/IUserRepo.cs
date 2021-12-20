using Microsoft.AspNetCore.Identity;
using OA_DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OA_Service
{
   public interface IUserRepo
    {
        Task<IdentityResult> SignUpAsync(Signup signup);

        Task<string> LoginAsync(Login s);
    }
}
