using QuipuMVC.Controllers;

namespace QuipuMVC.Utils
{
    public class EmailConfiguration
    {
        public string senderEmail { get; }
        public string receiverEmail { get; }
        public string subject { get; }
        public EmailContent emailBody { get; }
        public EmailConfiguration(string senderEmail, string receiverEmail, string subject, EmailContent emailBody)
        {
            this.senderEmail = senderEmail;
            this.receiverEmail = receiverEmail;
            this.subject = subject;
            this.emailBody = emailBody;
        }

        public void sendEmail(ILogger<HomeController> logger, string type)
        {
            string mailBody = this.emailBody.generateEmailBody();
            if (mailBody == null)
            {
                return;
            }
            try
            {
                // call email service
                logger.LogInformation($"TYPE: {type}");
                logger.LogInformation($"Sending mail to {receiverEmail} from {senderEmail} with subject {subject}. Email body: {mailBody}");
                logger.LogInformation("===============================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
        }
    }
}
