//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

namespace Infraestructure.Models
{
    public class JWTAuthentication
    {
        public int ExpirationInMinutes { get; set; }
        public string Secret { get; set; }
        public string[] HostOriginPermited { get; set; }

    }
     
    public class Cache
    {
        public int ExpirationCacheInHours { get; set; }
    }
  

    public class LengFileConfiguration
    {
        public int MaxLengFileInMb { get; set; }        
    }
}