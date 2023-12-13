
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWalksAPI.Data;
using NZWalksAPI.Mappings;
using NZWalksAPI.Middlewares;
using NZWalksAPI.Repositories;
using Serilog;
using System.Text;

namespace NZWalksAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var nzWalksConnectionstring = builder.Configuration.GetConnectionString("NZwalksConnectionString");
            var logger = new LoggerConfiguration()
                                       .WriteTo.Console()
                                       //.WriteTo.File("Logs/Nzwalks_Log.txt", rollingInterval: RollingInterval.Minute)
                                       .MinimumLevel.Information()
                                       .CreateLogger();

           
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Nz Walks Api",
                    Version = "v1"
                });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme { 
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "Oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services
               .AddDbContext<NzWalksDbContext>(options => options.UseSqlServer(nzWalksConnectionstring));
            
            builder.Services.AddDbContext<NzWalksAuthDbContext>(options => options.UseSqlServer(builder.Configuration
                .GetConnectionString("NZwalksAuthConnectionString")));

            builder.Services.AddScoped<IRegionRepositories, SqlReqionRepository>();
            builder.Services.AddScoped<IWalkRepositories,SqlWalkRepository>();
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();
            builder.Services.AddScoped<IImageRepository,LocalImageRepository>();

            builder.Services.AddAutoMapper(typeof(AutomapperProfiles));

            builder.Services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NzWalks")
                .AddEntityFrameworkStores<NzWalksAuthDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                }) ;


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Custom Middleware Thrown When Their Is An Exception
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images"
            });
            app.MapControllers();

            app.Run();
        }
    }
}