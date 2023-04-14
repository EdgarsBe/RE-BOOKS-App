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
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }
        private async void RegClick(object sender, EventArgs e)

        //Called variables from xaml entries.
        {
            string connectionString = "Server=2.tcp.eu.ngrok.io;Port=11249;User ID=edgars;Password=0000;Database=re-books";
            string CEmail = Email.Text;
            string CRepEmail = RepEmail.Text;
            string CUser = Username.Text;
            string CPass = Pass.Text;
            string CRepPass = RepPass.Text;
            bool Admin = false;

            if (CEmail == null || CRepEmail == null || CUser == null || CPass == null || CRepPass == null)
            {
                Error.Text = "Laukums ir tukšš!";
                Error.IsVisible = true;
                return;
            }

            //Validation for registration.
            bool ConfirmPass = CPass == CRepPass;
            bool ConfirmEmail = CEmail == CRepEmail;
            bool isEmailValid = Regex.IsMatch(CEmail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            bool isPasswordValid = Regex.IsMatch(CPass, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

            if (!ConfirmPass)
            {
                Error.Text = "Ievadītās paroles nesakrīt!";
                Error.IsVisible = true;
                return;
            }

            if (!ConfirmEmail)
            {
                Error.Text = "Ievadītie e-pasti nesakrīt!";
                Error.IsVisible = true;
                return;
            }

            if (!isEmailValid)
            {
                Error.Text = "Ievadītais epasts nav īsts!";
                Error.IsVisible = true;
                return;
            }

            if (!isPasswordValid)
            {
                Error.Text = "Dotā parole nav pietiekami stipra!";
                Error.IsVisible = true;
                return;
            }

            //Password hashing using bcrypt

            string HashedPass = BCrypt.Net.BCrypt.HashPassword(CPass,BCrypt.Net.BCrypt.GenerateSalt());

            //Creates a connection with db and creates new user.
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @CUser OR email = @CEmail", connection);
                {
                    command.Parameters.AddWithValue("@CUser", CUser);
                    command.Parameters.AddWithValue("@CEmail", CEmail);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        //username or email already exists
                        Error.Text = "Lietotājvārds vai e-pasts jau eksistē mūsu datubāzē!";
                        Error.IsVisible = true;
                        return;
                    }
                    else
                    {
                        //username and email are available
                        var insert = new MySqlCommand("INSERT INTO users (email, username, password, admin) VALUES (@CEmail, @CUser, @HashedPass, @Admin)", connection);
                        {
                            insert.Parameters.AddWithValue("@CEmail", CEmail);
                            insert.Parameters.AddWithValue("@CUser", CUser);
                            insert.Parameters.AddWithValue("@HashedPass", HashedPass);
                            insert.Parameters.AddWithValue("@Admin", Admin);
                            insert.ExecuteNonQuery();
                            await Shell.Current.GoToAsync($"//{nameof(Login)}");
                        }
                        connection.Close();
                    }
                }
            }
        }
    }
}