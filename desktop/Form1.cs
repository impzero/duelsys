using duelsys.Services;

namespace desktop
{
    public partial class Form1 : Form
    {
        public AuthenticationService aService;
        public Form1(AuthenticationService aService)
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
                if (u.IsAdmin)
                {
                    MessageBox.Show("Hello, " + u.FirstName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}