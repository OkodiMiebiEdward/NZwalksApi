
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NZWalksAPI.Data;
using NZWalksAPI.Mappings;
using NZWalksAPI.Repositories;

namespace NZWalksAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var nzWalksConnectionstring = builder.Configuration.GetConnectionString("NZwalksConnectionString");
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services
               .AddDbContext<NzWalksDbContext>(options => options.UseSqlServer(nzWalksConnectionstring));
            builder.Services.AddScoped<IRegionRepositories, SqlReqionRepository>();
            builder.Services.AddAutoMapper(typeof(AutomapperProfiles));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}