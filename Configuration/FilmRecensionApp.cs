using FilmRecensioner.Data;
using FilmRecensioner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FilmRecensioner.Configuration
{
    public class FilmRecensionApp
    {
        private readonly WebApplicationBuilder _builder;
        private readonly WebApplication _app;

        public FilmRecensionApp(string[] args)
        {
            _builder = WebApplication.CreateBuilder(args);

            ConfigureServices();

            _app = _builder.Build();

            ConfigureMiddleWares();
        }

        private void ConfigureServices()
        {
            _builder
                .Services
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(
                        _builder.Configuration.GetConnectionString("DefaultConnection")
                    );
                });

            AddAuthApi();

            _builder
                .Services
                .AddControllers()
                .AddNewtonsoftJson(
                    options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                            .Json
                            .ReferenceLoopHandling
                            .Ignore
                );

            _builder.Services.AddEndpointsApiExplorer();
            _builder.Services.AddSwaggerGen(CustomSwaggerGenOptions);
        }

        private void AddAuthApi()
        {
            _builder.Services.AddAuthorization();

            _builder
                .Services
                .AddIdentityApiEndpoints<CustomUser>(CustomIdentityOptions)
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }
        private void CustomIdentityOptions(IdentityOptions options)
        {
            if (_builder.Environment.IsDevelopment())
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 4;
            }
        }

        private void CustomSwaggerGenOptions(SwaggerGenOptions options)
        {
            options.OrderActionsBy((apiDesc) => apiDesc.RelativePath);
            options.AddSecurityDefinition(
                "oauth2",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                }
            );

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        }

        private void ConfigureMiddleWares()
        {
            // Configure the HTTP request pipeline.
            if (_app.Environment.IsDevelopment())
            {
                _app.UseSwagger();
                _app.UseSwaggerUI();
            }
            _app.MapControllers();

            _app.MapIdentityApi<CustomUser>();

            _app.UseHttpsRedirection();

            _app.UseAuthorization();
        }

        public void Run()
        {
            _app.Run();
        }
    }
}
