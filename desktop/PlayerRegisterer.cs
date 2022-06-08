using duelsys.ApplicationLayer;
using duelsys.ApplicationLayer.Views;

namespace desktop
{
    public partial class PlayerRegisterer : Form
    {
        private UserService uService;
        private TournamentService tService;
        public int TournamentId { get; set; }

        public PlayerRegisterer(TournamentService t, UserService u)
        {
            uService = u;
            tService = t;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var u = uService.GetUserById(Convert.ToInt32(comboBox1.SelectedValue));
                tService.Register(TournamentId, new UserBase(u.Id, u.FirstName, u.LastName));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                MessageBox.Show(exception.Message);
            }
            Close();
        }

        private void PlayerRegisterer_Load(object sender, EventArgs e)
        {
            var users = uService.GetNonRegisteredPlayersInTournament(true, TournamentId);

            comboBox1.DisplayMember = "Names";
            comboBox1.ValueMember = "Id";
            comboBox1.DataSource = users;
        }
    }
}
