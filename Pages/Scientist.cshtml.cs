using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScientistInfoWeb.Models;

namespace ScientistInfoWeb.Pages
{
    public class ScientistModel : PageModel
    {
        [BindProperty(SupportsGet = true)] // Allow getting the name from the query string
        public string ScientistName { get; set; }

        public ScientistInfo ScientistInfo { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(ScientistName))
            {
                ScientistInfo = await ScientistScraper.GetScientistInfo(ScientistName);
            }
        }
    }
}
