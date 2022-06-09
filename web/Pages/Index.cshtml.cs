using duelsys;
using duelsys.ApplicationLayer.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace web.Pages
{
    public class IndexModel : PageModel
    {
        public List<TournamentBase> TournamentBases { get; set; }
        public TournamentService tService { get; set; }


        public IndexModel(TournamentService t)
        {
            tService = t;
        }

        public void OnGet()
        {
            TournamentBases = tService.GetTournaments();
        }
    }
}