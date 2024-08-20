using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VTTCommonParameters.Dal.Entities.AccountEntities;
using VTTCommonParameters.Dal.Entities.AppEntities;

namespace VTTCommonParameters.Dal
{
    public class VTTCommonParametersContext : DbContext
    {

        public DbSet<Project> Projects { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<ParameterValue> ParameterValues { get; set; }
        public DbSet<User> Users { get; set; }
       
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=HAYRIKURTNB;Database=VTTCommonParametersDB;TrustServerCertificate=True;Trusted_Connection=True;");
            }
        }
    }
}