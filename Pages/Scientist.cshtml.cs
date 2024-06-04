using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScientistInfoWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScientistInfoWeb.Pages
{
  public class ScientistModel : PageModel
  {
    [BindProperty(SupportsGet = true)]
    public string ScientistName { get; set; }
    public List<ScientistInfo> ScientistInfoList { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int PageSize { get; set; } = 5;

    public async Task OnGetAsync(int page = 1)
    {
      if (!string.IsNullOrEmpty(ScientistName))
      {
        CurrentPage = page;
        var start = (CurrentPage - 1) * PageSize;
        var result = await ScientistScraper.GetScientistInfo(ScientistName, start, PageSize);
        ScientistInfoList = result.Results;
        TotalRecords = result.TotalRecords;
        TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
      }
    }
  }
}
