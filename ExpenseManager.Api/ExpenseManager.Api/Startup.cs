using System.Reflection;
using System.Text;
using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Command;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Mapper;
using ExpenseManager.Api.Services.AccountHistory;
using ExpenseManager.Api.Services.Token;
using ExpenseManager.Base;
using ExpenseManager.Base.Token;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ExpenseManager.Api;

public class Startup
{
    
    public IConfiguration Configuration { get; }
    public static JwtConfig JwtConfig { get; private set; }
    public Startup(IConfiguration configuration) => Configuration = configuration;
    public void ConfigureServices(IServiceCollection services)
    {
        JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        services.AddSingleton<JwtConfig>(JwtConfig);
        services.AddAutoMapper(typeof(MapperConfig));
        services.AddControllers(); 
        services.AddDbContext<ExpenseManagerDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("ExpenseManagerContext"));
        });
        //services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });
        //services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(AuthorizationCommandHandler).GetTypeInfo().Assembly));
        
        /* services.AddControllers().AddFluentValidation(x =>
        {
            x.RegisterValidatorsFromAssemblyContaining<CustomerValidator>();
        });*!!!!açılacak*/
        
        // TokenService
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAccountHistoryService, AccountHistoryService>();

        // HttpContextAccessor + AppSession
        services.AddHttpContextAccessor();
        services.AddScoped<IAppSession>(provider =>
        {
            var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
            AppSession appSession = JwtExtension.GetSession(httpContextAccessor.HttpContext);
            return appSession;
        });
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = JwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfig.Secret)),
                ValidAudience = JwtConfig.Audience,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExpenseManager Api Management", Version = "v1.0" });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Para Management for IT Company",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
        });
        
        // CORS (if needed)
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        /////////////services.AddSwaggerGen(); 
    }
   

    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            
        }
        
   
        app.UseHttpsRedirection();
        
        app.UseRouting();
        
        app.UseCors("AllowAll"); // if CORS policy is used
        
        app.UseAuthentication();
        app.UseAuthorization(); 
        
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
