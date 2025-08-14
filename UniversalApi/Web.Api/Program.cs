using Application;
using Application.Common.Interfaces;
using HealthChecks.UI.Client;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using System.Text;
using Web.Api.Handlers;
using Web.Api.Helper;
using Web.Api.Service;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Debug()
       .WriteTo.Logger(c => c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
       .WriteTo.File($"E:/Logs/serilog/DEBUG.log", rollingInterval: RollingInterval.Day))
       .WriteTo.Logger(c => c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
       .WriteTo.File($"E:/Logs/serilog/Info.log", rollingInterval: RollingInterval.Day))
       .WriteTo.Logger(c => c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
       .WriteTo.File($"E:/Logs/serilog/ERROR.log", rollingInterval: RollingInterval.Day))
       .CreateLogger();

builder.Services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());

var jwtSettings = builder.Configuration.GetSection("JWTSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
    };
});

builder.Services.AddControllers();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddApplication();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// With this line:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OnlineResturantManagement",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
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
app.UseSwagger();
app.UseSwaggerUI();
//app.UseHealthChecks("/health", new HealthCheckOptions
//{
//    ResponseWriter = async (context, report) =>
//    {
//        var result = JsonConvert.SerializeObject(
//            new
//            {
//                status = report.Status.ToString(),
//                errors = report.Entries.Select(e => new {
//                    key = e.Key,
//                    value =
//                e.Value.Status.ToString()
//                })
//            });
//        context.Response.ContentType = "application/json";
//        await context.Response.WriteAsync(result);
//    }
//});
//app.UseHealthChecks("/health", new HealthCheckOptions
//{
//    Predicate = _=> true,
//    ResponseWriter =UIResponseWriter.WriteHealthCheckUIResponse
//    //ResponseWriter = async (context, report) =>
//    //{

//    //    var result = JsonConvert.SerializeObject(
//    //        new
//    //        {
//    //            status = report.Status.ToString(),
//    //            errors = report.Entries.Select(e => new
//    //            {
//    //                key = e.Key,
//    //                value = e.Value.Status.ToString(),
//    //                message = e.Value.Description // Include the description/message from health check
//    //            })
//    //        });
//    //    context.Response.ContentType = "application/json";
//    //    await context.Response.WriteAsync(result);
//    //}
//}); ;

app.UseHttpsRedirection();

app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true)
               .AllowCredentials());
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuditLogMiddleware>();
app.UseStaticFiles();

app.MapControllers();

app.Run();
