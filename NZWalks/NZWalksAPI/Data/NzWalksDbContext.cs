using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Data
{
    public class NzWalksDbContext : DbContext
    {
        public NzWalksDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Walk> Walks { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Seed data for Difficulties with Easy,Meduim and Hard
            var difficulties = new List<Difficulty>
            {
                new Difficulty
                {
                      Id = Guid.Parse("3844aa94-a7c0-4ad7-b3d0-15886afa1fdc"),
                      Name = "Easy"
                },
                new Difficulty
                {
                      Id = Guid.Parse("b02deb7f-733f-49ad-8dd4-8a24d443e505"),
                      Name = "Medium"
                },
                new Difficulty
                {
                     Id= Guid.Parse("3b6a34c8-29be-44eb-95bb-c171fb6a020f"),
                     Name = "Hard"
                }
            };

            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            //Seed Data for Regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("4632dfaa-24db-4009-ba0e-866771b5d78d"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://source.unsplash.com/random/300x200?sig=123"
                },

                new Region
                {
                    Id = Guid.Parse("32bd0cba-48ad-4d72-a1f4-d9e38f927693"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },

                new Region
                {
                    Id = Guid.Parse("abd0d56c-a7ad-44ec-9ae7-d777a18fb2f4"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },

                new Region
                {
                    Id = Guid.Parse("f922ea03-3baa-4e11-8f6f-9e715946225e"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://source.unsplash.com/random/300x200?sig=456"
                },

                new Region
                {
                    Id = Guid.Parse("b6f4b32f-d3ab-4d2d-a97c-69a80da92379"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = null
                },

                new Region
                {
                    Id = Guid.Parse("ad789b77-f798-4acf-b2dc-270517bd9d3f"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = "https://source.unsplash.com/random/300x200?sig=789"
                },
            };

            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
