using Newtonsoft.Json.Linq;

namespace QuipuMVC.Utils
{
    public class EmailContent
    {
        public string htmlTemplatePath { get; set; }
        public string jsonData { get; set; }

        public EmailContent(string htmlTemplatePath, string jsonData)
        {
            this.htmlTemplatePath = htmlTemplatePath; // We assume that template uses {{ xyz }} as keywords where xyz is prop from the json object 
            this.jsonData = jsonData;
        }

        private string parseHtmlAsString()
        {
            try
            {
                return File.ReadAllText(this.htmlTemplatePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private JObject parseJson()
        {
            try
            {
                JObject jsonObject = JObject.Parse(this.jsonData);
                return jsonObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string get_receiver_mail()
        {
            JObject marketingData = parseJson();
            if (marketingData == null)
            {
                return null;
            }

            return marketingData["Email"].ToString(); // Assumption is made that every JSON will contain the email
        }

        public string generateEmailBody()
        {
            JObject marketingData = parseJson();
            string parsedHtml = parseHtmlAsString();

            if (marketingData == null || parsedHtml == null)
            {
                return null;
            }


            foreach (JProperty property in marketingData.Properties())
            {
                parsedHtml = parsedHtml.Replace("{{" + property.Name + "}}", marketingData[property.Name].ToString());
            }

            return parsedHtml;
        }
    }
}
