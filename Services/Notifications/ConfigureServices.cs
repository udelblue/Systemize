using Systemize.Models.Notifications;

namespace Systemize.Services.Notifications
{
    public class ConfigureServices
    {
        private readonly IConfiguration _configuration;

        public ConfigureServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(IServiceCollection services)
        {
            var emailConfig = _configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddControllers();
        }
    }
}