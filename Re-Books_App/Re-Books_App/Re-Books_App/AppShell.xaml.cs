using System;
using System.Collections.Generic;
using Re_Books_App.ViewModels;
using Re_Books_App.Views;
using Xamarin.Forms;

namespace Re_Books_App
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
