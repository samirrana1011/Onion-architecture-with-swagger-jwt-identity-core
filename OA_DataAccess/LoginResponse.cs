using System;
using System.Collections.Generic;
using System.Text;

namespace OA_DataAccess
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
