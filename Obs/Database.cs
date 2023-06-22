using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Obs.Models;
using System;
using System.Collections.Generic;

namespace Obs
{
    public class DatabaseContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        /*
        Nesneyi veritabaný tablosuna dönüþtürmek için
        1. <Foo> yerine sýnýfýnýn adýný girin 
        2. Genel isimlendirme kurallarýna göre 
            sýnýfýnýzýn ismi Car ise property ismi Cars olmalýdýr.
        */
        public DbSet<User> Ogrenciler { get; set; }

        public DbSet<Akademisyen> Akademisyenler { get; set; }
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<Not> Notlar { get; set; }
        public DbSet<Yariyil> Yariyillar { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*
             * bu yorum satýrýný açmayý açtýktan sonra MyDatabase yerine
             * kendi kullanacaðýnýz database ismini yazmayý unutmayýn!
             */
            optionsBuilder.UseSqlServer("Server=.;Database=MyDatabaseTu;Integrated Security=True;");
            base.OnConfiguring(optionsBuilder);
        }

        
        }
    }