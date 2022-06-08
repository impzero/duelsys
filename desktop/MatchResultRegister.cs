using duelsys.ApplicationLayer.Views;

namespace desktop
{
    public partial class MatchResultRegister : Form
    {
        private List<MatchPair> matches;
        private Tournaments tForm;

        public MatchResultRegister(List<MatchPair> m, Tournaments t)
        {
            matches = m;
            tForm = t;
            InitializeComponent();
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {

        }

        private void MatchResultRegister_Load(object sender, EventArgs e)
        {
            //var tournamentSystems = tsService.GetTournamentSystems();

            //comboBox2.DisplayMember = "Name";
            //comboBox2.ValueMember = "Id";
            //comboBox2.DataSource = tournamentSystems;
        }
    }
}
