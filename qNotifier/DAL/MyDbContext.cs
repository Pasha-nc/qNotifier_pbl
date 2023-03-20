using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using qNotifier.Models;
using System.Reflection.Emit;

namespace qNotifier.DAL
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public DbSet<UserRecord> UserRecords { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {                
                b.HasMany(e => e.Records)
                    .WithOne()
                    .IsRequired();
            });

            builder.Entity<User>(b =>
            { b.Navigation(e => e.Records).AutoInclude(); }
                );

            builder.Entity<UserRecord>().ToTable("UserRecords");
        }
    }
}
