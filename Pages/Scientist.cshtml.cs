using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScientistInfoWeb.Models;

namespace ScientistInfoWeb.Pages
{
  public class ScientistModel : PageModel
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ScientistModel(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }
    [BindProperty(SupportsGet = true)]
    public string ScientistName { get; set; } = "Khaled el Bahnasy";

    [BindProperty(SupportsGet = true)]
    public int Start { get; set; } = 0;

    public List<ScientistInfo> ScientistInfoList { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int PageSize { get; set; } = 10;

    //public async Task OnGetAsync()
    //{
    //  if (!string.IsNullOrEmpty(ScientistName))
    //  {
    //    CurrentPage = (Start / PageSize) + 1;
    //    var (results, totalRecords) = await ScientistScraper.GetScientistInfo(ScientistName, Start, PageSize, "0,5");
    //    ScientistInfoList = results;
    //    TotalRecords = totalRecords;
    //    TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
    //  }
    //}
    //
    public async Task OnGetAsync()
    {
      if (!string.IsNullOrEmpty(ScientistName))
      {
        CurrentPage = (Start / PageSize) + 1;

        // Use 'var' to let the compiler infer the type 
        var results = default((List<ScientistInfo>, int));

        // Check if totalRecords is already in session
        if (!_httpContextAccessor.HttpContext.Session.TryGetValue("TotalRecords", out byte[] totalRecordsBytes))
        {
          // If not, fetch from Google Scholar and store in session
          results = await ScientistScraper.GetScientistInfo(ScientistName, Start, PageSize, "0,5");

          totalRecordsBytes = BitConverter.GetBytes(results.Item2);
          _httpContextAccessor.HttpContext.Session.Set("TotalRecords", totalRecordsBytes);

          TotalRecords = results.Item2;
          TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

          ScientistInfoList = results.Item1;
        }
        else
        {
          // If already in session, retrieve it
          TotalRecords = BitConverter.ToInt32(totalRecordsBytes);
          TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);

          // Fetch the results for the current page
          results = await ScientistScraper.GetScientistInfo(ScientistName, Start, PageSize, "0,5");
          ScientistInfoList = results.Item1;
        }
      }
    }
  }
}
