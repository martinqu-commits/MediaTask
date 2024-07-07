using Microsoft.AspNetCore.Mvc;
using QuipuMVC.Models;
using QuipuMVC.Utils;
using System.Diagnostics;
using System.Xml.Linq;

namespace QuipuMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<string> _stores;
        private Dictionary<string, string> _storeXmlName;
        private string _htmlTemplatePath;

        public HomeController(ILogger<HomeController> logger)
        {
            _stores = new List<string>();
            _stores.Add("Market store");
            _stores.Add("Book store");

            _storeXmlName = new Dictionary<string, string>();
            _storeXmlName[_stores[0]] = "market.xml";
            _storeXmlName[_stores[1]] = "books.xml";

            _htmlTemplatePath = "template.html";
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MarketStore()
        {
            return View("Store", new { Type = _stores[0] });
        }

        public IActionResult BookStore()
        {
            return View("Store", new { Type = _stores[1] });
        }

        public IActionResult AdminUI()
        {
            return View("AdminStore", new { stores = _stores });
        }

        [HttpPost]
        public ActionResult SendEmails(string type)
        {
            if (_storeXmlName[type] == null)
            {
                return null;
            }
            
            string xmlPath = $"campaign_{_storeXmlName[type]}";
            XDocument doc = XDocument.Load(xmlPath);

            // Assumption was made that campaign with senderEmail and Subject
            // is made beforehand and is stored somewhere in a database

            // get SenderEmail and Subject from a database

            string senderMail = "test@test.com";
            string subject = "Marketing Campaign";

            // Simulation of generating the Queue messages
            Email emails = new Email();
            foreach (var clientElem in doc.Descendants("Client"))
            {
                var templateElement = clientElem.Element("Template");
                if (templateElement == null)
                {
                    return null;
                }
                var marketingData = templateElement.Element("MarketingData")?.Value;
                if (marketingData == null) return null;
                EmailContent content = new EmailContent(_htmlTemplatePath, marketingData);
                if (content == null) return null;
                string receiverMail = content.get_receiver_mail();
                emails.addConfiguration(new EmailConfiguration(senderMail, receiverMail, subject, content));
            }

            emails.start_marketing_campaign(_logger, type); // This function uses Parallel.ForEach()

            return Content($"Sending emails for {type} started. {_storeXmlName[type]}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}