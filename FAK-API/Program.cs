using FAK.GLOBAL.App.Infrastructure;
using FAK.Persistance;
using MediatR.Pipeline;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation.AspNetCore;
using FAK.GLOBAL.App;
using Microsoft.AspNetCore.Identity;
using FAK.Domain.Entities;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using FAK.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using FAK.Infrastructure.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddControllers(opt =>
//{
//    var policy = new AuthorizationPolicyBuilder("Bearer").RequireAuthenticatedUser().Build();
//    opt.Filters.Add(new AuthorizeFilter(policy));
//});
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidIssuer = builder.Configuration["JWTConfig:Issuer"],//.GetSection("JWTConfig:Issuer").Value,
    ValidAudience = builder.Configuration["JWTConfig:Audience"],//.GetSection("JWTConfig:Audience").Value,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                    (builder.Configuration["JWTConfig:Key"])), //.GetSection("JWTConfig:Key").Value))
};
builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }
    ).AddJwtBearer(opt =>
        {
            //var jwtConfg = builder.Configuration.GetSection("JWTConfig");
            //var key = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTConfig:Key"));
            opt.SaveToken = true;
            opt.TokenValidationParameters = tokenValidationParameters;
        });

builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FAK API", Version = "v1" });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please insert your token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",

//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
//      {
//        {
//          new OpenApiSecurityScheme
//          {
//            Reference = new OpenApiReference
//              {
//                Type = ReferenceType.SecurityScheme,
//                Id = "Bearer"
//              },
//              Scheme = "oauth2",
//              Name = "Bearer",
//              In = ParameterLocation.Header,

//            },
//            new List<string>()
//          }
//        });
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    c.IncludeXmlComments(xmlPath);
//    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    //{
//    //    {
//    //        new OpenApiSecurityScheme
//    //        {
//    //            Reference = new OpenApiReference
//    //            {
//    //                Type=ReferenceType.SecurityScheme,
//    //                Id="Bearer"
//    //            }
//    //        },
//    //        new string[]{}

//    //    }
//    //});
//});
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnStr"));
});
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWTConfig"));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<FAK.Infrastructure.Services.IEmailSender, EmailSender>();
//builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
//builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
builder.Services.AddMediatR(z=>z.RegisterServicesFromAssemblies(typeof(FAK.GLOBAL.App.LoginModel).Assembly));
builder.Services.AddMediatR(z => z.RegisterServicesFromAssemblies(typeof(FAK.Common.Model.StatusModel).Assembly));
//builder.Services.AddMediatR(typeof(FAK.GLOBAL.App.LoginModel));
//builder.Services.AddMediatR(typeof(FAK.Common.Model.StatusModel).GetTypeInfo().Assembly);

builder.Services
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginCommandValidator>());

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("MyPolicy", builder =>
//    {
//        builder.WithOrigins("http://localhost:5173")
//        .AllowAnyMethod()
//        .AllowAnyHeader()
//        .AllowCredentials();
//    });
//});
//}
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnetClaimAuthorization v1");
        //c.RoutePrefix = string.Empty;
        }) ;
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
