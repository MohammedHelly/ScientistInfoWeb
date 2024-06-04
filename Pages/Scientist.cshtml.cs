using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScientistInfoWeb.Models;

namespace ScientistInfoWeb.Pages
{
    public class ScientistModel : PageModel
    {
        [BindProperty(SupportsGet = true)] // Allow getting the name from the query string
        public string ScientistName { get; set; }
        public List<ScientistInfo> ScientistInfoList { get; set; }

        public async Task OnGetAsync()
        {
            //ScientistName = "Albert Einstein";
            if (!string.IsNullOrEmpty(ScientistName))
            {
                ScientistInfoList = await ScientistScraper.GetScientistInfo(ScientistName);
            }
        }
    }
}
