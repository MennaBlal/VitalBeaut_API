
using EcommercePro.Models;
using EcommercePro.Repositiories;
using EcommercePro.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using ProductMiniApi.Repository.Implementation;
using System.Text;

namespace EcommercePro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #region inject repository 
            builder.Services.AddScoped<IGenaricService<Category>, GenericRepo<Category>>();
            builder.Services.AddScoped<IGenaricService<Product>, GenericRepo<Product> > ();
            builder.Services.AddScoped<IBrand, BrandRepository>();

            #endregion
            builder.Services.AddDbContext<Context>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DC"));
            }

           );
            builder.Services.AddTransient<IFileService, FileService>();

            #region Authontication
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<Context>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = "http://localhost:5116",
                    ValidateAudience = true,
                    ValidAudience = "http://localhost:4200",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("1s3r4e5g6h7j81s3r4e5g6h7j81s3r4e5g6h7j81s3r4e5g6h7j8")
                        )


                };

            });
  #endregion


            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
                RequestPath = "/Resources"
            });
            app.MapControllers();

            app.Run();
        }
    }
}
