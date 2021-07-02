using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AuthDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(u => u.UserName)
                                        .IsRequired()
                                        .HasMaxLength(20);

            modelBuilder.Entity<User>().Property(u => u.Password)
                                        .IsRequired()
                                        .HasMaxLength(50);
        }
    }
}
