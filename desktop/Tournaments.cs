﻿using duelsys;
using duelsys.Services;

namespace desktop
{
    public partial class Tournaments : Form
    {
        public TournamentService tService;
        public TournamentSystemService tsService;
        public SportService sService;

        public Tournaments(TournamentService tService, SportService service, TournamentSystemService tsService)
        {
            this.tService = tService;
            sService = service;
            this.tsService = tsService;
            InitializeComponent();
        }


        private void Tournaments_Load(object sender, EventArgs e)
        {
            RefetchHomePage();
        }

        private void RefetchHomePage()
        {
            dataGridView1.DataSource = tService.GetTournaments();
            var source = new BindingSource();

            source.DataSource = typeof(object);
            foreach (var tournament in tService.GetTournaments())
            {
                object t = new
                {
                    Id = tournament.Id,
                    Description = tournament.Description,
                    Location = tournament.Location,
                    Sport = tournament.Sport.Name,
                    SportMinPlayers = tournament.Sport.MinPlayersCount,
                    SportMaxPlayers = tournament.Sport.MaxPlayersCount,
                    TournamentSystem = tournament.TournamentSystem.GetType().Name
                };

                source.Add(t);
                dataGridView1.DataSource = source;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var description = textBox1.Text;
            var location = textBox2.Text;
            var sportId = Convert.ToInt32(comboBox1.SelectedValue);
            var tsId = Convert.ToInt32(comboBox2.SelectedValue);
            var startingDate = dateTimePicker1.Value;
            var endingDate = dateTimePicker2.Value;

            var t = new TournamentBase(description, location, startingDate, endingDate);

            tService.CreateTournament(true, t, sportId, tsId);
            RefetchHomePage();
        }

        private void RefetchCreatePage()
        {
            var sports = sService.GetSports();

            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
            comboBox1.DataSource = sports;

            var tournamentSystems = tsService.GetTournamentSystems();

            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Id";
            comboBox2.DataSource = tournamentSystems;
        }
        private void tabPage1_Enter(object sender, EventArgs e)
        {
            RefetchCreatePage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var sportName = textBox3.Text;
            var minPlayers = Convert.ToInt32(numericUpDown1.Value);
            var maxPlayers = Convert.ToInt32(numericUpDown2.Value);

            try
            {
                sService.CreateSport(sportName, minPlayers, maxPlayers);
                RefetchCreatePage();
                MessageBox.Show("Keep in mind that developers need to implement the rules for the newly created sport, don't yet choose it as a sport for a tournament", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
