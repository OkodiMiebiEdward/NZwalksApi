using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data
{
    public class NzWalksAuthDbContext : IdentityDbContext
    {
        public NzWalksAuthDbContext(DbContextOptions<NzWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //seeding roles into the database

            //The reason we did not use Guid.NewGuid().ToString() is because it will create a  new guid each time
            var readerRoleId = "0b7d983e-2406-45ef-b322-8c5ba8f0201b";
            var writerRoleId = "e6904823-81e3-480e-87bb-9583c642c941";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    NormalizedName = "Reader",
                    Name = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    NormalizedName = "Writer",
                    Name = "Writer".ToUpper()
                }
            };


            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
