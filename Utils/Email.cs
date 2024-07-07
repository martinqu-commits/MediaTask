using QuipuMVC.Controllers;

namespace QuipuMVC.Utils
{
    public class Email
    {
        public List<EmailConfiguration> emailConfigurations { get; }
        public Email()
        {
            this.emailConfigurations = new List<EmailConfiguration>();
        }

        public void addConfiguration(EmailConfiguration configuration)
        {
            this.emailConfigurations.Add(configuration);
        }

        public void start_marketing_campaign(ILogger<HomeController> logger, string type)
        {
            if (this.emailConfigurations.Count == 0) return;

            Parallel.ForEach<EmailConfiguration>(this.emailConfigurations, config =>
            {
                config.sendEmail(logger, type);
            });
        }
    }
}
