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
            var users = leaderboard.Select(result => result.Value).ToList();

            for (int i = 0; i < users.Count; i++)
            {
                listBox1.Items.Add($"{i + 1}. " + users[i].Stringify);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
            Close();
        }
    }
}
