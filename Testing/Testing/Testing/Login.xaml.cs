using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

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
            Shell.Current.GoToAsync($"//{nameof(MainPage)}"); /*

            string connectionString = "server=localhost;port=3306;database=re-books;user=root;password=0000;";
            string EnteredUsername = usernameInput.Text;
            string EnteredPassword = passwordInput.Text;


            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM users WHERE email = @EnteredUsername";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string DBpassword = reader.GetString(4);
                            if (BCrypt.Net.BCrypt.Verify(EnteredPassword, DBpassword))
                            {
                                App.GlobalVariables.Email = reader.GetString(2);
                                App.GlobalVariables.Username = reader.GetString(3);
                                App.GlobalVariables.Password = EnteredPassword;
                                App.GlobalVariables.Admin = Convert.ToBoolean(reader.GetString(5));

                                Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                            }
                            else
                            {
                                Error.IsVisible = true;
                            }
                        }
                    }
                }
            }*/
        }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
            await Navigation.PushAsync(new Register());
    }
}
}