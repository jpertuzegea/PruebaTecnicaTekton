//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

namespace Infraestructure.Models
{
    public class LoginModel
    {
        public string UserNetwork { get; set; }
        public string? Password { get; set; }
        public bool IsLogued { get; set; }
        public string? Token { get; set; }

        //public string? ExpirationToken { get; set; }

    }
}
