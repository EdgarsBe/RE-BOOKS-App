using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MySqlConnector;

namespace Testing
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public class Books
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string URL { get; set; }
        }


        private async void LogoutClick(object sender, EventArgs e)
        {
            UserData.Admin = null;
            UserData.Email = null;
            UserData.ID = 0;
            UserData.Username = null;
            UserData.Password = null;

            await Shell.Current.GoToAsync($"//{nameof(Login)}");
        }

        private bool _isFirstTime = true;

        public async void OnTap(object sender, EventArgs e)
        {
            var tappedImage = (Image)sender;
            if (tappedImage.ClassId != null)
            {
                BookData.BookID = tappedImage.ClassId;
                await Navigation.PushAsync(new BookPage());
            } 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_isFirstTime)
            {
            string connectionString = "Server=2.tcp.eu.ngrok.io;Port=11249;User ID=edgars;Password=0000;Database=re-books";
            List<Books> books = new List<Books>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //Izvelk no datubāzes datus par jaunākajām grāmatām.

                var command = new MySqlCommand("SELECT * FROM books ORDER BY date DESC",connection);
                {
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

                var Title = new Label()
                {
                    Text = "Jaunākais",
                    Padding = new Thickness(30, 10, 30, 5),
                    FontSize = 28
                };
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
                        Padding = new Thickness(5,0,5,0),
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

                Container.Children.Add(Title);
                Container.Children.Add(scrollView);
                books.Clear();
                connection.Close();
            }


                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    //Izvelk no datubāzes datus par jaunākajām grāmatām.

                    var command = new MySqlCommand("SELECT * FROM books ORDER BY clicks DESC", connection);
                    {
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

                    var Title = new Label()
                    {
                        Text = "Populārākais",
                        Padding = new Thickness(30, 10, 30, 5),
                        FontSize = 28
                    };
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

                    Container.Children.Add(Title);
                    Container.Children.Add(scrollView);
                    books.Clear();
                    connection.Close();
                    _isFirstTime = false;
                }
            }
        }
    }
}
