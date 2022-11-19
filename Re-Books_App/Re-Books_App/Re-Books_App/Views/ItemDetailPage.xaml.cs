using System.ComponentModel;
using Xamarin.Forms;
using Re_Books_App.ViewModels;

namespace Re_Books_App.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}