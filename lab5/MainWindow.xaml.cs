using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
using System.Xml.Serialization;
using lab5.Annotations;
using Microsoft.Win32;

namespace lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Movie> movies { get; set; }
        public ObservableCollection<Movie> searchList { get; set; }

        public MainWindow()
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            BooleanAndConverter booleanAndConverter = new BooleanAndConverter();
            InitializeComponent();
            movies = new ObservableCollection<Movie>();
            searchList = new ObservableCollection<Movie>();
            this.DataContext = movies;
            SeachList.ItemsSource = searchList;
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
            XmlSerializer SerializerObj = new XmlSerializer(movies.GetType());
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML documents (.xml) |*.xml";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var path = openFileDialog.FileName;
                using (StreamReader reader = new StreamReader(path))
                {
                    ObservableCollection<Movie> moviesTemp = new ObservableCollection<Movie>();
                    moviesTemp = (ObservableCollection<Movie>) SerializerObj.Deserialize(reader);
                    foreach (Movie movie in moviesTemp)
                        movies.Add(movie);
                }
            }
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            //TODO : Serializacja zawartosci listy i zapisanie tego w pliku XML
            XmlSerializer SerializerObj = new XmlSerializer(movies.GetType());
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Movie list"; // default file name
            saveFileDialog.DefaultExt = ".xml"; // default file extension
            saveFileDialog.Filter = "XML documents (.xml) |*.xml"; // filter by extension
            Nullable<bool> result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var path = saveFileDialog.FileName;
                using (StreamWriter writer = new StreamWriter(path))
                {
                    SerializerObj.Serialize(writer, movies);
                }
            }
        }

        #endregion

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            searchList.Clear();
            if (authorCBox.IsChecked == false && titleCBox.IsChecked == false &&
                scoreCBox.IsChecked == false && typeCBox.IsChecked == false)
            {
                Console.WriteLine("Warrning, none of the criteria was specified");
                MessageBox.Show("You must specifie some criteria", "Warrning", MessageBoxButton.OK);
            }
            else
            {

                string title = null;
                string director = null;
                Score score = Score.Terrible;
                MovieType movietype = MovieType.Comedy;
                if (titleBox.IsEnabled)
                    title = titleBox.Text;
                if (directorBox.IsEnabled)
                    director = directorBox.Text;
                if (ScoreComboBox.IsEnabled)
                    score = (Score)ScoreComboBox.SelectedValue;
                if (TypeComboBox.IsEnabled)
                    movietype = (MovieType)TypeComboBox.SelectedValue;

                //only title box enabled
                if (titleBox.IsEnabled)
                {
                    var newsearchList = movies.Where(x => x.Title.Contains(title)).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Title);
                        searchList.Add(movie);
                    }
                }
                //only director box enabled
                if (directorBox.IsEnabled)
                {
                    var newsearchList = movies.Where(x => x.Director.Contains(director)).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Director);
                        searchList.Add(movie);
                    }
                }
                //only score box enabled
                if (ScoreComboBox.IsEnabled)
                {
                    var newsearchList = movies.Where(x => x.Score == score).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Score);
                        searchList.Add(movie);
                    }
                }
                //only type box enabled
                if (TypeComboBox.IsEnabled)
                {
                    var newsearchList = movies.Where(x => x.Type == movietype).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Type);
                        searchList.Add(movie);
                    }
                }
                //only title and director enabled
                if (titleBox.IsEnabled && directorBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Title.Contains(title) && x.Director.Contains(director)).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Title + " " + movie.Director);
                        searchList.Add(movie);
                    }
                }
                //only title and score enabled
                if (ScoreComboBox.IsEnabled && titleBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Score == score && x.Title.Contains(title)).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Title + " " + movie.Score);
                        searchList.Add(movie);
                    }
                }
                //only title and type enabled
                if (TypeComboBox.IsEnabled && titleBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Type == movietype && x.Title.Contains(title)).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Title + " " + movie.Type);
                        searchList.Add(movie);
                    }
                }
                //only director and score enabled
                if (ScoreComboBox.IsEnabled && directorBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Score == score && x.Director.Contains(director)).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Score + " " + movie.Director);
                        searchList.Add(movie);
                    }

                }
                //only director and type enabled
                if (TypeComboBox.IsEnabled && directorBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Type == movietype && x.Director.Contains(director)).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Type + " " + movie.Director);
                        searchList.Add(movie);
                    }
                }
                //only score and type enabled
                if (ScoreComboBox.IsEnabled && TypeComboBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Score == score && x.Type == movietype).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Score + " " + movie.Type);
                        searchList.Add(movie);
                    }
                }
                //only title director score enabled
                if (titleBox.IsEnabled && directorBox.IsEnabled && ScoreComboBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Score == score && x.Title.Contains(title) && x.Director.Contains(director)).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Title + " " + movie.Director + " " + movie.Score);
                        searchList.Add(movie);
                    }
                }
                //only title score type enabled
                if (titleBox.IsEnabled && ScoreComboBox.IsEnabled && TypeComboBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Score == score && x.Title.Contains(title) && x.Type == movietype).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Title + " " + movie.Type + " " + movie.Score);
                        searchList.Add(movie);
                    }
                }
                //only  title director type
                if (titleBox.IsEnabled && directorBox.IsEnabled && TypeComboBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Type == movietype && x.Title.Contains(title) && x.Director.Contains(director)).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Title + " " + movie.Director + " " + movie.Type);
                        searchList.Add(movie);
                    }
                }
                //all enabled
                if(titleBox.IsEnabled && directorBox.IsEnabled && TypeComboBox.IsEnabled && ScoreComboBox.IsEnabled)
                {
                    searchList.Clear();
                    var newsearchList = movies.Where(x => x.Score == score && x.Title.Contains(title) && x.Director.Contains(director) && x.Type == movietype).ToList();
                    foreach (Movie movie in newsearchList)
                    {
                        Console.WriteLine(movie.Title + " " + movie.Director + " " + movie.Score + " " + movie.Type);
                        searchList.Add(movie);
                    }

                }
                SeachList.ItemsSource = searchList;
                SeachList.Visibility = Visibility.Visible;
            }
        }

    #region GhettoFabSolution
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T) child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        private void viewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var Item in FindVisualChildren<ListBoxItem>(viewList))
            {
                if (Item.IsSelected == true)
                {
                    foreach (var tb in FindVisualChildren<TextBlock>(Item))
                    {
                        if (tb.Name.ToString() == "Hidden" || tb.Name.ToString() == "Hidden2")
                        {
                            tb.Visibility = Visibility.Visible;
                            Console.WriteLine("Details visible");
                        }
                    }
                }
                else
                {
                    foreach (var tb in FindVisualChildren<TextBlock>(Item))
                    {
                        if (tb.Name.ToString() == "Hidden" || tb.Name.ToString() == "Hidden2")
                        {
                            tb.Visibility = Visibility.Collapsed;
                            Console.WriteLine("Details hidden");
                        }
                    }
                }
            }
        }
        #endregion

        private void Delete(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Movie> movietodel = new ObservableCollection<Movie>();
            foreach (Movie movie in movies)
            {
                foreach (Movie movietodelete in searchList)
                {
                    if(movietodelete == movie)
                        movietodel.Add(movietodelete);
                }
            }
            foreach (Movie movie in movietodel)
            {
                movies.Remove(movie);
            }
        }
    }

    [Serializable()]
    public class Movie
    { 
        public string Title { get; set; }
        public string Director { get; set; }
        public Score Score { get; set; }
        public MovieType Type { get; set; }
    }

    public class BooleanAndConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                foreach (object value in values)
                {
                    if ((value is bool) && (bool) value == false)
                    {
                        return false;
                    }
                }
                return true;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
            }
        }


    public class Test
    {
        public Test(string _t, string _d)
        {
            TitleSearch = _t;
            DirectorSearch = _d;
        }
        public string TitleSearch { get; set; }
        public string DirectorSearch { get; set; }
    }

    public class Validator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "value cannot be empty");
            }
            else
            {
                if (value.ToString().Length == 0)
                {
                    Console.WriteLine("WALIDACJA, CHUJ NIE REAKCJA");
                    return new ValidationResult(false, "name has to be bigger than 0 chars");
                }
            }
            return ValidationResult.ValidResult;
        }
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