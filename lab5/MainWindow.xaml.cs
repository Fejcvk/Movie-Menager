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

        public MainWindow()
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            BooleanAndConverter booleanAndConverter = new BooleanAndConverter();
            InitializeComponent();
            movies = new ObservableCollection<Movie>();
            this.DataContext = movies;
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
            if (authorCBox.IsChecked == false && titleCBox.IsChecked == false &&
                scoreCBox.IsChecked == false && typeCBox.IsChecked == false)
            {
                Console.WriteLine("Warrning, none of the criteria was specified");
                MessageBox.Show("You must specifie some criteria", "Warrning", MessageBoxButton.OK);
            }
            else
            {
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

    public class Validator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value.ToString().Length == 0 ? new ValidationResult(false, " value cannot be empty.") : ValidationResult.ValidResult;
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