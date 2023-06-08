using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Octokit;
using ProjetoTerra.Application.Users.Commands;
using ProjetoTerra.Application.Users.Services;
using ProjetoTerra.Domain.Models;
using ProjetoTerra.Domain.Users.Entities;
using ProjetoTerra.Infra.Data;
using ProjetoTerra.Infra.Data.Seeders;
using ProjetoTerra.Shared.HttpServices.Configurations;

namespace ProjetoTerra.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            // Seed
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var seeder = new MongoSeeder(dbContext.MongoDatabase, passwordHasher, userManager);
                await seeder.SeedUserTest();
            }
            
            await host.RunAsync();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(System.IO.Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddControllers();
                    services.AddMediatR(typeof(Program));
                    services.AddMediatR(typeof(LoginCommand), typeof(LoginCommand));
                    services.AddTransient<IUserAuthService, UserAuthService>();
                    services.AddScoped<UserManager<ApplicationUser>>();

                    // Configurar autenticação JWT
                    var configuration = hostContext.Configuration;

                    var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
                    services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

                    var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
                    services.AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(options =>
                        {
                            options.RequireHttpsMetadata = false;
                            options.SaveToken = true;
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = jwtSettings.Issuer,
                                ValidAudience = jwtSettings.Audience,
                                IssuerSigningKey = new SymmetricSecurityKey(key),
                                ClockSkew = TimeSpan.Zero
                            };
                        });
                    
                    var databaseName = configuration.GetSection("MongoDB")["DatabaseName"];
                    var connectionString = configuration.GetConnectionString("MongoDbConnection");
                    
                    // Configurar conexão com o MongoDB
                    services.AddSingleton<ApplicationDbContext>(_ =>
                    {
                        var client = new MongoClient(connectionString);
                        return new ApplicationDbContext(client.GetDatabase(databaseName));
                    });
                    
                    services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
                    services.AddTransient<IMongoDatabase>(provider =>
                    {
                        var client = provider.GetService<IMongoClient>();
                        return client.GetDatabase(databaseName);
                    });

                    services.AddScoped<IUserStore<ApplicationUser>>(provider =>
                    {
                        var database = provider.GetService<IMongoDatabase>();
                        return new CustomMongoUserStore(database);
                    });
                    
                    services.AddSingleton<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();

                    services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                        {
                            // Configurações do Identity
                        })
                        .AddUserStore<CustomMongoUserStore>()
                        .AddRoleStore<RoleStore<IdentityRole>>()
                        .AddDefaultTokenProviders();
                    
                    services.AddScoped<IRoleStore<IdentityRole>>(provider =>
                    {
                        var database = provider.GetService<IMongoDatabase>();
                        return new CustomRoleStore(database);
                    });
                    
                    // Configs Octokit
                    services.AddScoped<GitHubClient>(sp =>
                    {
                        var githubClient = new GitHubClient(new ProductHeaderValue("ProjetoTerra"))
                        {
                            Credentials = new Credentials(configuration.GetValue<string>("GitToken"))
                        };

                        return githubClient;
                    });
                    
                    // Configurar Swagger
                    services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Projeto Terra", Version = "v1" });

                        // Adicionar suporte à autenticação JWT no Swagger

                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description = "JWT Authorization header using the Bearer scheme."
                        });
                        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    In = ParameterLocation.Header,
                                    Description = "Add 'Bearer ' + token'",
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] { }
                            }
                        });
                    });
                    
                    services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            // CORS
                            builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        });
                    });
                })
                .Configure(app =>
                {
                    var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
                    
                    app.UseCors();
                    
                    if (env.IsDevelopment())
                    {
                        app.UseDeveloperExceptionPage();
                    }
                    
                    app.UseCustomExceptionHandler();

                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                    
                    app.UseApiVersioning();
                    
                    app.UseSwagger();

                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Projeto Terra v1");
                    });
                });
    }
}
