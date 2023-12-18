//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entitys
{
    public class Products
    { 
        [Key]
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public int? StatusId { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }

        [NotMapped]
        public decimal? Discount { get; set; }
        [NotMapped]
        public decimal? FinalPrice { get; set; } 
         
    }
}
