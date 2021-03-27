using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CityPassContext : DbContext
    {

        public CityPassContext() : base((new DbContextOptionsBuilder())
            //.UseSqlServer("Server=.;Database=CityPass;Trusted_Connection=True;MultipleActiveResultSets= true")
            .UseSqlServer("Server=citypassdb.database.windows.net;Database=CityPassDB;user id=admincitypass;password=CityPass123;Trusted_Connection=True;Integrated Security=false;MultipleActiveResultSets= true")
            .EnableSensitiveDataLogging()
            .Options)
        {
        }

        public DbSet<Attraction> Attraction { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Collection> Collection { get; set; }
        public DbSet<Pass> Pass { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<UserPass> UserPass { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<WorkingTime> WorkingTime { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Attraction>().Property(_ => _.Name).HasMaxLength(100).IsRequired().IsUnicode();
            builder.Entity<Attraction>().Property(_ => _.Description).HasMaxLength(2000).IsUnicode();
            builder.Entity<Attraction>().Property(_ => _.Address).HasMaxLength(500).IsUnicode();
            builder.Entity<Attraction>().Property(_ => _.IsTemporarityClosed).HasDefaultValue(false);

            builder.Entity<Category>().Property(_ => _.Name).IsUnicode().IsRequired().HasMaxLength(100);

            builder.Entity<City>().Property(_ => _.Name).IsUnicode().IsRequired().HasMaxLength(100);

            builder.Entity<Collection>().Property(_ => _.MaxConstrain).IsRequired();

            builder.Entity<Pass>().Property(_ => _.Name).HasMaxLength(100).IsRequired().IsUnicode();
            builder.Entity<Pass>().Property(_ => _.Description).HasMaxLength(2000).IsUnicode();
            builder.Entity<Pass>().Property(_ => _.Price);
            builder.Entity<Pass>().Property(_ => _.IsSelling).HasDefaultValue(false);
            builder.Entity<Pass>().Property(_ => _.ExpireDuration).IsRequired();

            builder.Entity<TicketType>().Property(_ => _.Name).IsUnicode().IsRequired().HasMaxLength(200);
            builder.Entity<TicketType>().Property(_ => _.AdultPrice).IsRequired(false).HasDefaultValueSql(null);
            builder.Entity<TicketType>().Property(_ => _.ChildrenPrice).IsRequired(false).HasDefaultValueSql(null);

            builder.Entity<UserPass>().Property(_ => _.WillExpireAt).IsRequired();
            builder.Entity<UserPass>().Property(_ => _.BoughtAt).IsRequired();
            builder.Entity<UserPass>().Property(_ => _.Feedback).HasMaxLength(1000);
            builder.Entity<UserPass>().Property(_ => _.Rate);

            

            builder.Entity<WorkingTime>().Property(_ => _.DayOfWeek).IsRequired();
            builder.Entity<WorkingTime>().Property(_ => _.StartTime).IsRequired();
            builder.Entity<WorkingTime>().Property(_ => _.EndTime).IsRequired();

            builder.Entity<TicketTypeInCollection>().HasKey(_ => new { _.TicketTypeId, _.CollectionId });

            //builder.Entity<User>().HasMany<UserPass>().WithOne(_ => _.User).HasForeignKey(_ => _.Uid); 

            base.OnModelCreating(builder);
        }

        public async Task<bool> Commit()
        {
            int result = await base.SaveChangesAsync();
            return result > 0;
        }
    }
}
