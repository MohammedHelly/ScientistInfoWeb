using HtmlAgilityPack;
using ScientistInfoWeb.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScientistInfoWeb
{
  public class ScientistScraper
  {
    private static readonly HttpClient _client = new HttpClient();

    public static async Task<(List<ScientistInfo> Results, int TotalRecords)> GetScientistInfo(string scientistName, int start = 0, int count = 5)
    {
      string baseUrl = $"https://scholar.google.com/scholar?hl=en&q={scientistName}&start={start}&count={count}";
      HttpResponseMessage response = await _client.GetAsync(baseUrl);
      string htmlContent = await response.Content.ReadAsStringAsync();

      var doc = new HtmlDocument();
      doc.LoadHtml(htmlContent);

      var resultNodes = doc.DocumentNode.SelectNodes("//div[@class='gs_r gs_or gs_scl']");
      if (resultNodes == null)
      {
        return (new List<ScientistInfo>(), 0);
      }

      var results = new List<ScientistInfo>();

      foreach (var result in resultNodes)
      {
        // Extract the title
        var titleNode = result.SelectSingleNode(".//h3[@class='gs_rt']/a");
        string title = titleNode?.InnerText.Trim() ?? "No title available";

        // Extract the author and publication info
        var authorNode = result.SelectSingleNode(".//div[@class='gs_a']");
        string authorInfo = authorNode?.InnerText.Trim() ?? "No author information available";

        // Extract the snippet
        var snippetNode = result.SelectSingleNode(".//div[@class='gs_rs']");
        string snippet = snippetNode?.InnerText.Trim() ?? "No snippet available";

        // Extract the link
        string link = titleNode?.GetAttributeValue("href", "No link available");

        results.Add(new ScientistInfo
        {
          Title = title,
          AuthorInfo = authorInfo,
          Snippet = snippet,
          Link = link
        });
      }

      // Extract total number of results (simulating the total count for demonstration)
      var totalResultsNode = doc.DocumentNode.SelectSingleNode("//div[@id='gs_ab_md']/div[@class='gs_ab_mdw']");
      int totalRecords = 100; // Replace with actual logic to extract total records

      return (results, totalRecords);
    }
  }
}
