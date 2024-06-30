using EcommercePro.Hubs;
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

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSignalR();


        #region inject repository 
        builder.Services.AddScoped<IGenaricService<Category>, GenericRepo<Category>>();
        
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddTransient<IFileService, FileService>();
        builder.Services.AddScoped<IBrand, BrandRepository>();
        builder.Services.AddScoped<IContact, RepoContact>();
        builder.Services.AddScoped<IWebsiteReview, WebsiteReviewRepo>();
        builder.Services.AddScoped<IwishList, WishListService>();
        builder.Services.AddScoped<IproductReview, ProductReviewService>();
        builder.Services.AddTransient<IEmailService, EmailService>();
 
        #endregion

        builder.Services.AddDbContext<Context>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("DC"));
        });

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
             });
        });

        #region Authentication
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
        {
            //option.SignIn.RequireConfirmedEmail = true;
        }
        ).AddEntityFrameworkStores<Context>();

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
                ValidIssuer = "http://localhost:5261",
                ValidateAudience = true,
                ValidAudience = "http://localhost:4200",
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("1s3r4e5g6h7j81s3r4e5g6h7j81s3r4e5g6h7j81s3r4e5g6h7j89")
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
        app.UseCors();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapHub<NotificationHub>("/NotificationHub");

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
