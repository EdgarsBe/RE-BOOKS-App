using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Testing
{
    public partial class App : Application
    {
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
}
