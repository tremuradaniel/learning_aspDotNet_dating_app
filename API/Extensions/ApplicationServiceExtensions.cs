using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        ) 
        {
            services.AddDbContext<DataContext>(opt => 
            {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();

            // AddTransient -- too short lived for a http request - created and disposed
            // of as and when it's used

            // AddSingleton -- create a service when the application first starts and is 
            // never disposed until the application has closed down

            // AddScoped -- when we create a controller, as in a request hit the endpoint
            // so the framework instantiates a new instance of that controller,
            // the controller looks at its depencencies or the framework does and determines
            // that it must create these services and create new instances of these services
            // when the controllers are created 
            // WHEN the controller is disposed of at the end of the HTTP request, then
            // any depencennt services are also disposed 

            // AddScoped would have worked with TokenService directly, but using the 
            // interface has an advantage when testing - easier to mock stuff
            services.AddScoped<ITokenService, Services.TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();

            return services;
        }
    }
}
