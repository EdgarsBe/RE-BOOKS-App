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

            bool NULLEntry = EnteredUsername == "" && EnteredPassword == "";

            if (NULLEntry)
            {
                Error.Text = "Laukums ir tukšš!";
                Error.IsVisible = true;
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM users WHERE username = @EnteredUsername", connection);
                {
                    command.Parameters.AddWithValue("@EnteredUsername", EnteredUsername);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string DBpassword = reader.GetString(3);
                            if (BCrypt.Net.BCrypt.Verify(EnteredPassword, DBpassword))
                            {
                                UserData.Email = reader.GetString(1);
                                UserData.Username = reader.GetString(2);
                                UserData.ID = reader.GetInt32(0);
                                if (reader.GetInt32(4) == 0)
                                {
                                    UserData.Admin = "User";
                                }
                                else
                                {
                                    UserData.Admin = "Admin";
                                }
                                Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                            }
                            else
                            {
                                Error.Text = "Jūs nepareizi esat ievadijuši e-pastu / lietotājvārdu!";
                                Error.IsVisible = true;
                            }
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
        public static string Admin { get; set; }
        public static int ID { get; set; }
    }
}
