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
    public partial class BookPage : ContentPage
    {
        public BookPage()
        {
            InitializeComponent();
        }

        string connectionString = "Server=2.tcp.eu.ngrok.io;Port=11249;User ID=edgars;Password=0000;Database=re-books";

        public class Book
        {
            public static int ID { get; set; }
            public static string Title { get; set; }
            public static string Author { get; set; }
            public static string Description { get; set; }
            public static string Category { get; set; }
            public static string Image { get; set; }
        }

        private void Saveing(object sender, EventArgs e)
        {
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var BookID = BookData.BookID;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM books WHERE bookID = @BookID", connection);
                {
                    command.Parameters.AddWithValue("@BookID" , BookID);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book.ID = reader.GetInt32(0);
                            Book.Title = reader.GetString(1);
                            Book.Author = reader.GetString(2);
                            Book.Description = reader.GetString(3);
                            Book.Image = reader.GetString(4);
                            Book.Category = reader.GetString(5);

                        }
                        reader.Close();
                    }
                }

                Title.Text = Book.Title;
                Author.Text = Book.Author;
                Description.Text = Book.Description;
            }
        }
    }
}