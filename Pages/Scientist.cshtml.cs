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

    [BindProperty(SupportsGet = true)]
    public int Start { get; set; } = 0;

    public List<ScientistInfo> ScientistInfoList { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int PageSize { get; set; } = 10;

    public async Task OnGetAsync()
    {
      if (!string.IsNullOrEmpty(ScientistName))
      {
        CurrentPage = (Start / PageSize) + 1;
        var (results, totalRecords) = await ScientistScraper.GetScientistInfo(ScientistName, Start, PageSize, "0,5");
        ScientistInfoList = results;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
      }
    }
  }
}
