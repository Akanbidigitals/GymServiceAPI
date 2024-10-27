using GymMembershipAPI;
using GymMembershipAPI.DataAccess.DataContext;
using GymMembershipAPI.DataAccess.Interface;
using GymMembershipAPI.DataAccess.Interfaces;
using GymMembershipAPI.DataAccess.Repository;
using GymMembershipAPI.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Connection String 
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("GymString"));
});
builder.Services.AddTransient<IEmailService,EmailServiceRepository>(); //Mail D.I
builder.Services.AddScoped<IRegister_Login, Register_LoginRepository>(); //Logs D.I
builder.Services.AddScoped<ISuperAdminRepository,SuperAdminRepository>();// SuperAdmin D.I
builder.Services.AddScoped<IGymOwnerRepository, GymOwnerRepository>(); // GymOwner D.I


builder.Services.Configure<Setup>(builder.Configuration.GetSection("Setup"));

//Configure Authrntication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateActor = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Setup:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Setup:SecretKey"]!))
        };
    });


//Configure Authorization
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "GymAPIUser", Version = "v1" });
    options.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference =  new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string []{}
        }
    });
});


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
