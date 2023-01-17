using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Testing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        public void LoginClick(object sender, EventArgs e)
        {
            string connectionString = "Server=192.168.8.108;Port=3306;User ID=armands;Password=password;Database=re-books";
            string EnteredUsername = usernameInput.Text;
            string EnteredPassword = passwordInput.Text;


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM users WHERE username = @EnteredUsername", connection);
                command.Parameters.AddWithValue("@EnteredUsername", EnteredUsername);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string DBpassword = reader.GetString(3);
                        if (BCrypt.Net.BCrypt.Verify(EnteredPassword, DBpassword)) 
                        {
                            UserData.Email = reader.GetString(1);
                            UserData.Username = reader.GetString(2);
                            UserData.Password = EnteredPassword;

                            Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                        }
                        else
                        {
                            Error.IsVisible = true;
                        }
                    }
                }
            }
        }


    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
            await Navigation.PushAsync(new Register());
    }
}

    public class UserData
    {
        public static string Username { get; set; }
        public static string Email { get; set; }
        public static string Password { get; set; }
    }
}
