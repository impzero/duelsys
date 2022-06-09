using duelsys;
using duelsys.ApplicationLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public List<TournamentBase> TournamentBases { get; set; }

        [BindProperty]
        public int TournamentId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Error { get; set; }
        public TournamentService tService { get; set; }
        public UserService uService { get; set; }


        public IndexModel(UserService u, TournamentService t)
        {
            tService = t;
            uService = u;
        }

        public void OnGet()
        {
            TournamentBases = tService.GetTournaments();
        }

        public IActionResult OnPost()
        {
            try
            {
                var user = uService.GetUserById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                tService.Register(TournamentId,
                    new duelsys.ApplicationLayer.Views.UserBase(user.Id, user.FirstName, user.LastName));
            }
            catch (InvalidTournamentException ex)
            {
                return RedirectToPage("/Index", new { Error = ex.Message });
            }
            catch (Exception)
            {
            }

            return RedirectToPage("/Index");
        }
    }
}