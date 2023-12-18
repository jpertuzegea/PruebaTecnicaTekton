//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Entitys
{
    public class Status
    { 
        [Key]
        public int? StatusId { get; set; }
        public string? StatusName { get; set; } 
    }
}
