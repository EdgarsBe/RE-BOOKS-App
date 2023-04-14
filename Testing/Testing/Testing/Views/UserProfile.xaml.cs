using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Text.RegularExpressions;
using BCrypt;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Testing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile : ContentPage
    {
        public UserProfile()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            user.Text = UserData.Username;
            email.Text = UserData.Email;
            Admin.Text = UserData.Admin;
        }

        private async void DeleteProfile(object sender, EventArgs e)
        {
            string Email = UserData.Email;
            string ConfirmPass = await DisplayPromptAsync("Dzēst Profilu", "Apstipriniet vai tiešam vēlaties dzēst profilu.");

            string connectionString = "Server=2.tcp.eu.ngrok.io;Port=11249;User ID=edgars;Password=0000;Database=re-books";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var check = connection.CreateCommand())
                {
                    check.CommandText = "SELECT * FROM users WHERE email = @Email";
                    check.Parameters.AddWithValue("@Email", Email);
                    using (var reader = check.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserData.Password = reader.GetString(3);
                        }
                        reader.Close();
                    }
                    if (BCrypt.Net.BCrypt.Verify(ConfirmPass, UserData.Password))
                    {
                        check.CommandText = "DELETE FROM users WHERE email = @Email";
                        check.ExecuteNonQuery();
                        UserData.Password = "";
                        await Shell.Current.GoToAsync($"//{nameof(Login)}");
                    }
                    else
                    {
                        await DisplayAlert("Kļūda dzēšot", "Jums neizdevās izdzēst profilu", "OK");
                    }
                }
                connection.Close();
            }
        }

        private async void EditProfile(object sender, EventArgs e)
        {

            string connectionString = "Server=2.tcp.eu.ngrok.io;Port=11249;User ID=edgars;Password=0000;Database=re-books";

            int ID = UserData.ID;
            string action = await DisplayActionSheet("Ko jūs vēlaties mainīt?", "Cancel", null, "Lietotājvārdu", "Paroli");

            if (action == "Cancel")
            {
                return;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var check = connection.CreateCommand())
                {
                    if (action == "Lietotājvārdu")
                    {
                        string NewUser = await DisplayPromptAsync("Jauns Lietotājvārds", "Ievadiet jauno lietotājvārdu.");
                        if (NewUser == null) 
                        {
                            return;
                        }

                        check.CommandText = "SELECT COUNT(*) FROM users WHERE username = @NewUser";
                        check.Parameters.AddWithValue("@NewUser", NewUser);
                        int count = Convert.ToInt32(check.ExecuteScalar());
                        if (count > 0)
                        {
                            //username already exists
                            await DisplayAlert("Kļūda", "Ievadītais lietotājvādrs ir jau aizņemts.", "Labi");
                            return;
                        }
                        check.CommandText = "UPDATE users SET username = @NewUser WHERE userID = @ID";
                        check.Parameters.AddWithValue("@ID", ID);
                        check.ExecuteNonQuery();
                        UserData.Username = NewUser;
                        user.Text = UserData.Username;
                    }
                    if (action == "Paroli")
                    {
                        string Confirm = await DisplayPromptAsync("Parole", "Ievadiet šobrīdējo paroli.");
                        if (Confirm == null)
                        {
                            return;
                        }
                        check.CommandText = "SELECT * FROM users WHERE userID = @ID";
                        check.Parameters.AddWithValue("@ID", ID);
                        using (var reader = check.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserData.Password = reader.GetString(3);
                            }
                            reader.Close();
                        }
                        if (BCrypt.Net.BCrypt.Verify(Confirm, UserData.Password))
                        {
                            string NewPass = await DisplayPromptAsync("Parole", "Ievadiet jauno paroli.");
                            bool isPasswordValid = Regex.IsMatch(NewPass, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

                            if (!isPasswordValid)
                            {
                                await DisplayAlert("Kļūda", "Ievadīta parole ir pārāk vāja (8 simbolus garš un jaiekļauj A-Z, a-z, 0-9 un simbolus @$!%*?&)", "Labi");
                                return;
                            }

                            if (NewPass == null)
                            {
                                return;
                            }

                            //Password hashing using bcrypt

                            string HashedPass = BCrypt.Net.BCrypt.HashPassword(NewPass, BCrypt.Net.BCrypt.GenerateSalt());
                            check.CommandText = "UPDATE users SET password = @HashedPass WHERE userID = @ID";
                            check.Parameters.AddWithValue("@HashedPass", HashedPass);
                            check.ExecuteNonQuery();

                        }
                    }
                }
            }
        }
    }
}