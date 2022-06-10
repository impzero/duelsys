using duelsys;
using duelsys.ApplicationLayer.Services;
using duelsys.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace web.Pages;

public class TournamentModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int TournamentId { get; set; }

    public Tournament Tournament { get; set; }
    public List<LeaderboardUser> TournamentLeaderboard { get; set; }
    public Dictionary<int, UserBase> TournamentMatchWinners { get; set; }
    public TournamentService tService { get; set; }
    public string Error { get; set; }
    public TournamentModel(TournamentService t)
    {
        TournamentLeaderboard = new List<LeaderboardUser>();
        TournamentMatchWinners = new Dictionary<int, UserBase>();
        tService = t;
    }

    public void OnGet()
    {
        try
        {
            Tournament = tService.GetTournamentById(TournamentId);
            var kv = tService.GetLeaderboard(TournamentId);
            foreach (var kvp in kv)
                TournamentLeaderboard.Add(kvp.Value);

            TournamentMatchWinners = tService.GetMatchWinners(TournamentId);
        }
        catch (InvalidTournamentLeaderboardException)
        {
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }
}

