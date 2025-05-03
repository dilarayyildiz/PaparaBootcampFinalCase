using System.Reflection;
using System.Text;
using AutoMapper;
using ExpenseManager.Api.Entities;
using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Api.Mapper;
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

        services.AddControllers().AddFluentValidation(x =>
        {
            x.RegisterValidatorsFromAssemblyContaining<CustomerValidator>();
        });

       /* services.AddControllersWithViews(options =>
        {
            options.CacheProfiles.Add("Default45",
                new CacheProfile
                {
                    Duration = 45,
                    Location = ResponseCacheLocation.Any,
                    NoStore = false
                });
        });*/
     
        

        services.AddSingleton(new MapperConfiguration(x => x.AddProfile(new MapperConfig())).CreateMapper());

        services.AddDbContext<ExpenseManagerDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection"));
        });
           
        
        /*
         * Senin projende:
           
           Kendi User entity’in var.
           Kendi JWT üretimini yazıyorsun.
           Kendi şifre hash kontrolünü yazıyorsun.
           ApplicationUser kullanmıyorsun.
           Yani sen ASP.NET Identity’ye bağlı değilsin → bu satıra ihtiyacın yok.
           
           Bu blok sadece ApplicationUser + Identity bazlı sistemlerde gereklidir.
           
         */
        
        /*services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 1;            
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
        }).AddEntityFrameworkStores<ExpenseManagerDbContext>();*/

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateAuthorizationTokenCommand).Assembly);
        });

        //services.AddScoped<ScopedService>();
        //services.AddTransient<TransientService>();
       // services.AddSingleton<SingletonService>();
        //services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITokenService, TokenService>();
        //services.AddScoped<IUnitOfWork, UnitOfWork>();
       // services.AddSingleton<INotificationService, NotificationService>();

        services.AddResponseCaching();
        services.AddMemoryCache();

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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Expense Manager Api Management", Version = "v1.0" });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
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
// CORS (frontend bağlıysa)
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        
        services.AddScoped<IAppSession>(provider =>
        {
            var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
            AppSession appSession = JwtExtension.GetSession(httpContextAccessor.HttpContext);
            return appSession;
        });

       /*Redis kullanacak mıyım düşüneceğim!!!!!!
       var resdisConnection = new ConfigurationOptions();
        resdisConnection.EndPoints.Add(Configuration["Redis:Host"], Convert.ToInt32(Configuration["Redis:Port"]));
        resdisConnection.DefaultDatabase = 0;
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = resdisConnection;
            options.InstanceName = Configuration["Redis:InstanceName"];
        });
       */
       
        
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseMiddleware<HeartBeatMiddleware>();
        //app.UseMiddleware<ErrorHandlerMiddleware>();

        // Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
        // {
        //     Log.Information("-------------Request-Begin------------");
        //     Log.Information(requestProfilerModel.Request);
        //     Log.Information(Environment.NewLine);
        //     Log.Information(requestProfilerModel.Response);
        //     Log.Information("-------------Request-End------------");
        // };
        // app.UseMiddleware<RequestLoggingMiddleware>(requestResponseHandler);

        
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseResponseCaching();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        
        

    }
}