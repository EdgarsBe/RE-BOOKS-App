using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Testing
{
    public partial class App : Application
    {

        public static UserData GlobalVariables { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new Navig();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
    public class UserData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
    }
}
