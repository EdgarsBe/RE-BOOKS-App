using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;

namespace Testing
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SearchPage : ContentPage
{
    public SearchPage()
    {
        InitializeComponent();
    }
        public class Books
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string URL { get; set; }
        }

        public async void OnTap(object sender, EventArgs e)
        {
            var tappedImage = (Image)sender;
            if (tappedImage.ClassId != null)
            {
                BookData.BookID = tappedImage.ClassId;
                await Navigation.PushAsync(new BookPage());
            }
        }

        string connectionString = "Server=2.tcp.eu.ngrok.io;Port=11249;User ID=edgars;Password=0000;Database=re-books";

        private void SearchButton(object sender, EventArgs e)
        {
            Container.Children.Clear();

            List<Books> books = new List<Books>();
            string SearchField = Search.Text;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM books WHERE title LIKE @SearchField", connection);
                {
                    command.Parameters.AddWithValue("@SearchField", "%" + SearchField + "%");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Books book = new Books
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                URL = reader.GetString(4)
                            };

                            books.Add(book);
                        }
                        reader.Close();
                    }
                }
                var stackLayout = new StackLayout()
                {
                    Padding = new Thickness(20, 5, 20, 5),
                    Orientation = StackOrientation.Horizontal
                };

                foreach (Books item in books)
                {
                    Image image = new Image()
                    {
                        ClassId = Convert.ToString(item.ID),
                        WidthRequest = 100,
                        HeightRequest = 200,
                        Margin = new Thickness(0),
                        Source = ImageSource.FromUri(new Uri("https://1640-85-254-74-231.au.ngrok.io/" + item.URL))
                    };

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += OnTap;

                    image.GestureRecognizers.Add(tapGestureRecognizer);

                    var label = new Label
                    {
                        Text = item.Name,
                    };

                    var view = new StackLayout
                    {
                        Padding = new Thickness(5, 0, 5, 0),
                        Children = { image, label }
                    };

                    stackLayout.Children.Add(view);
                }

                var scrollView = new ScrollView()
                {
                    Content = stackLayout,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                    Orientation = ScrollOrientation.Horizontal
                };

                Container.Children.Add(scrollView);
                books.Clear();
                connection.Close();
            }
        }
    }
}