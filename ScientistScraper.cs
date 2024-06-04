using HtmlAgilityPack;
using ScientistInfoWeb.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScientistInfoWeb // Namespace declaration
{
    public class ScientistScraper // Class declaration within the namespace
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<ScientistInfo> GetScientistInfo(string scientistName)
        {
            string baseUrl = $"https://scholar.google.com/scholar?hl=en&q={scientistName}";
            HttpResponseMessage response = await _client.GetAsync(baseUrl);
            string htmlContent = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var nameNode = doc.DocumentNode.SelectSingleNode("//div[@class='gs_a']/a");
            var affiliationNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'gs_a gs_or gs_scl')]");

            if (nameNode == null || affiliationNode == null)
            {
                return null;
            }

            return new ScientistInfo
            {
                Name = nameNode.InnerText,
                Affiliation = affiliationNode.InnerText.Trim()
            };
        }
    }
}