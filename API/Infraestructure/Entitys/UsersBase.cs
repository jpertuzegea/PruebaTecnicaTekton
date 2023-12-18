//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Infraestructure.Entitys
{
    public class UsersBase
    {
        [Key]
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? UserNetwork { get; set; }// Usuario de Red 
        public string? Password { get; set; } 
 

    }

}
