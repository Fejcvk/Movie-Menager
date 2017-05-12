using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using lab5.Annotations;

namespace lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Movie> movies { get; set; }

        public MainWindow()
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            movies = new ObservableCollection<Movie>();
            this.DataContext = movies;
            InitializeComponent();
            ScoreComboBox.ItemsSource = Enum.GetValues(typeof(Score)).Cast<Score>();
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(MovieType)).Cast<MovieType>();
        }

        #region //MenuItem

        private void Add(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            string[] titles =
            {
                "Test1", "Test2", "Test3", "Test4", "Test5", "Test6", "Test7", "Test8", "Test9", "Test10"
            };
            string[] directors =
            {
                "TestD1", "TestD2", "TestD3", "TestD4", "TestD5", "TestD6", "TestD7", "TestD8", "TestD9", "TestD10"
            };

            for (int i = 0; i < 10; i++)
            {
                movies.Add(new Movie()
                {
                    Title = titles[rand.Next(0, 9)],
                    Director = directors[rand.Next(0, 9)],
                    Score = (Score) rand.Next(0, 5),
                    Type = (MovieType) rand.Next(0, 4)
                });
            }
        }

        private void Help(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("There is no help", "Help", MessageBoxButton.OK);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            movies.Clear();
        }

        private void Import(object sender, RoutedEventArgs e)
        {
            //TODO : Deserializacja XML i zaladowanie tego na liste
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            //TODO : Serializacja zawartosci listy i zapisanie tego w pliku XML
        }
        #endregion

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            ;
        }
    }
    public class Movie
    { 
        public string Title { get; set; }
        public string Director { get; set; }
        public Score Score { get; set; }
        public MovieType Type { get; set; }
    }

    #region Enums
    public enum Score
    {
        Terrible,
        Bad,
        Ok,
        Good,
        Awesome
    };

    public enum MovieType
    {
        Thriller,
        Comedy,
        Drama,
        Horror
    };
    #endregion
}