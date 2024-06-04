//using HtmlAgilityPack;
//using ScientistInfoWeb.Models;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace ScientistInfoWeb
//{
//  public class ScientistScraper
//  {
//    private static readonly HttpClient _client = new HttpClient();

//    public static async Task<(List<ScientistInfo> Results, int TotalRecords)> GetScientistInfo(string scientistName, int start = 0, int pageSize = 10)
//    {
//      string baseUrl = $"https://scholar.google.com/scholar?hl=en&q={scientistName}&start={start}";
//      HttpResponseMessage response = await _client.GetAsync(baseUrl);
//      string htmlContent = await response.Content.ReadAsStringAsync();

//      var doc = new HtmlDocument();
//      doc.LoadHtml(htmlContent);

//      var resultNodes = doc.DocumentNode.SelectNodes("//div[@class='gs_r gs_or gs_scl']");
//      if (resultNodes == null)
//      {
//        return (new List<ScientistInfo>(), 0);
//      }

//      var results = new List<ScientistInfo>();

//      foreach (var result in resultNodes)
//      {
//        // Extract the title
//        var titleNode = result.SelectSingleNode(".//h3[@class='gs_rt']/a");
//        string title = titleNode?.InnerText.Trim() ?? "No title available";

//        // Extract the author and publication info
//        var authorNode = result.SelectSingleNode(".//div[@class='gs_a']");
//        string authorInfo = authorNode?.InnerText.Trim() ?? "No author information available";

//        // Extract the snippet
//        var snippetNode = result.SelectSingleNode(".//div[@class='gs_rs']");
//        string snippet = snippetNode?.InnerText.Trim() ?? "No snippet available";

//        // Extract the link
//        string link = titleNode?.GetAttributeValue("href", "No link available");

//        results.Add(new ScientistInfo
//        {
//          Title = title,
//          AuthorInfo = authorInfo,
//          Snippet = snippet,
//          Link = link
//        });
//      }

//      // Extract total number of results
//      var totalResultsNode = doc.DocumentNode.SelectSingleNode("//div[@id='gs_ab_md']/div[@class='gs_ab_mdw']");
//      int totalRecords = 0;
//      if (totalResultsNode != null)
//      {
//        var totalText = totalResultsNode.InnerText;
//        var match = System.Text.RegularExpressions.Regex.Match(totalText, @"\d+");
//        if (match.Success)
//        {
//          totalRecords = int.Parse(match.Value);
//        }
//      }

//      return (results, totalRecords);
//    }
//  }
//}


using HtmlAgilityPack;
using ScientistInfoWeb.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ScientistInfoWeb
{
  public class ScientistScraper
  {
    private static readonly HttpClient _client = new HttpClient();

    public static async Task<(List<ScientistInfo> Results, int TotalRecords)> GetScientistInfo(string scientistName, int start = 0, int pageSize = 10, string asSdt = "0,5")
    {
      string baseUrl = $"https://scholar.google.com/scholar?hl=en&q={scientistName}&start={start}&as_sdt={asSdt}";
      HttpResponseMessage response = await _client.GetAsync(baseUrl);
      string htmlContent = await response.Content.ReadAsStringAsync();

      var doc = new HtmlDocument();
      doc.LoadHtml(htmlContent);

      var resultNodes = doc.DocumentNode.SelectNodes("//div[@class='gs_r gs_or gs_scl']");
      var results = new List<ScientistInfo>();

      if (resultNodes != null)
      {
        foreach (var result in resultNodes)
        {
          var titleNode = result.SelectSingleNode(".//h3[@class='gs_rt']/a");
          string title = titleNode?.InnerText.Trim() ?? "No title available";

          var authorNode = result.SelectSingleNode(".//div[@class='gs_a']");
          string authorInfo = authorNode?.InnerText.Trim() ?? "No author information available";

          var snippetNode = result.SelectSingleNode(".//div[@class='gs_rs']");
          string snippet = snippetNode?.InnerText.Trim() ?? "No snippet available";

          string link = titleNode?.GetAttributeValue("href", "No link available");

          results.Add(new ScientistInfo
          {
            Title = title,
            AuthorInfo = authorInfo,
            Snippet = snippet,
            Link = link
          });
        }
      }

      var totalResultsNode = doc.DocumentNode.SelectSingleNode("//div[@id='gs_ab_md']/div[@class='gs_ab_mdw']");
      int totalRecords = 0;
      if (totalResultsNode != null)
      {
        var totalText = totalResultsNode.InnerText;
        var match = Regex.Match(totalText, @"\d{1,3}(,\d{3})*(\.\d+)?");
        if (match.Success)
        {
          totalRecords = int.Parse(match.Value.Replace(",", ""));
        }
      }

      return (results, totalRecords);
    }
  }
}
