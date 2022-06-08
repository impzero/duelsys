using duelsys.ApplicationLayer.Services;

namespace desktop
{
    public partial class MatchResultRegister : Form
    {
        private TournamentService tService;
        private MatchService mService;
        public int TournamentId { get; set; }

        public MatchResultRegister(TournamentService t, MatchService m)
        {
            tService = t;
            mService = m;
            InitializeComponent();
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            var mId = Convert.ToInt32(comboBox1.SelectedValue);
            var matchPair = mService.GetMatchPair(TournamentId, mId);

            var firstPlayerResult = textBox1.Text;
            var secondPlayerResult = textBox2.Text;

            var args = new TournamentService.RegisterMatchResultArgs(TournamentId, mId, matchPair.FirstPlayer, matchPair.SecondPlayer, firstPlayerResult, secondPlayerResult);
            tService.RegisterMatchResult(true, args);
        }

        private void MatchResultRegister_Load(object sender, EventArgs e)
        {
            try
            {
                var pairs = tService.GetMatchPairsInTournament(TournamentId);
                comboBox1.DisplayMember = "Stringified";
                comboBox1.ValueMember = "Id";
                comboBox1.DataSource = pairs;
            }
            catch (Exception)
            {
                throw;
                //MessageBox.Show(exception.Message);
            }

        }
    }
}
