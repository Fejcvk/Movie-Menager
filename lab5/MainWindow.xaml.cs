using System;
using System.Collections.Generic;
using System.Linq;
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

namespace lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Movie> movies { get; set; }
       
        public MainWindow()
        {
            movies = new List<Movie>();
            InitializeComponent();
            //myGrid.ItemsSource = movies;
            
        }
    }
    public class Movie 
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public Score Score { get; set;}
        public MovieType Type { get; set;}

    }

    public enum Score { Terrible, Bad, OK, Good, Awesome };
    public enum MovieType { Thriller, Comedy, Drama, Horror };
}
