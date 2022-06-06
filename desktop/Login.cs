using duelsys.Services;
using Microsoft.Extensions.DependencyInjection;

namespace desktop
{
    public partial class Login : Form
    {
        public AuthenticationService aService;
        public Login(AuthenticationService aService)
        {
            this.aService = aService;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var email = textBox1.Text;
            var password = textBox2.Text;

            try
            {
                var u = aService.Login(email, password);
                if (!u.IsAdmin)
                    throw new Exception("You must be an admin in order to manage tournaments");

                var tForm = Program.ServiceProvider.GetRequiredService<Tournaments>();

                tForm.FormClosed += (_, _) => Show();

                Hide();
                tForm.ShowDialog();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}