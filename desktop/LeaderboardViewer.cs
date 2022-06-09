using duelsys.ApplicationLayer.Services;

namespace desktop;

public partial class LeaderboardViewer : Form
{
    private TournamentService tService;
    public int TournamentId { get; set; }

    public LeaderboardViewer(TournamentService tService)
    {
        this.tService = tService;
        InitializeComponent();
    }

    private void LeaderboardViewer_Load(object sender, EventArgs e)
    {
        try
        {
            var leaderboard = tService.GetLeaderboard(TournamentId);
            foreach (var user in leaderboard.Select(result => result.Value))
            {
                listBox1.Items.Add(user.FirstName + " " + user.LastName + " - " + user.WonGames + " won, " + user.LostGames + " lost");
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
            Close();
        }
    }
}
