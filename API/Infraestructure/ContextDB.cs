//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using Infraestructure.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infraestructure
{
    public class ContextDB : DbContext
    {
        private readonly IConfiguration _configuration;
        private string conection;

        public ContextDB(DbContextOptions<ContextDB> options, IConfiguration configuration) : base(options)
        {
            this._configuration = configuration;
        }

        public ContextDB(IConfiguration _IConfiguration)
        {
            _configuration = _IConfiguration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer(
                _configuration.GetConnectionString("BDConnetion"),
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds)
                );
        }

        
        public DbSet<Users> Users { get; set; }       
        public DbSet<Status> Status { get; set; }
        public DbSet<Products> Products { get; set; }

        
    }
}
