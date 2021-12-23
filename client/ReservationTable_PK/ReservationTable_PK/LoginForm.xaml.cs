using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReservationTable_PK
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public static User LoggedInUser = null;
        RestClient client = null;
        public LoginForm(MainWindow Main)
        {
            this.Main = Main;
            string server = ConfigurationSettings.AppSettings["server"];
            int port = int.Parse(ConfigurationSettings.AppSettings["port"]);
            client = new RestClient(string.Format($"http://{server}:{port}/php_pk/users/index.php"));
            InitializeComponent();
        }
        public MainWindow Main { get; set; }
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btn_ExecuteLogin_Click(object sender, EventArgs e)
        {

            if (tb_LoginUsername.Text == "" || tb_LoginPassword.Password == "")
            {
                MessageBox.Show("Username and password is required!");
                return;
            }


            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;

            request.AddObject(new
            {
                username = tb_LoginUsername.Text,
                password = tb_LoginPassword.Password.ToString()

            }) ;

            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }

            try
            {
                LoggedInUser = new JsonSerializer().Deserialize<User>(response);
            }
            catch (System.Runtime.Serialization.SerializationException)
            {

                MessageBox.Show("Incorrect login credentials!");
                return;
            }
            if (LoggedInUser != null)
            {
                MessageBox.Show($"{LoggedInUser.Name} logged in successfully!");
                this.Close();
            }
        }

    }
}
