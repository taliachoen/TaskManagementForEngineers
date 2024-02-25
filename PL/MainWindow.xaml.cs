using BlApi;
using PL.Engineer;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ViewEngineers_Click(object sender, RoutedEventArgs e)
        {
            // Open a new Engineer List Window
            new EngineerListWindow().Show();
        }

        private void InitData_Click(object sender, RoutedEventArgs e)
        {
            // הודעת אישור מהמשתמש
            MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize all data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // בדיקה אם המשתמש אישר
            if (result == MessageBoxResult.Yes)
            {
               s_bl.InitializeDB();
            }
        }

        private void ResetData_Click(object sender, RoutedEventArgs e)
        {
            // הודעת אישור מהמשתמש
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset all data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // בדיקה אם המשתמש אישר
            if (result == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
            }
        }

    }
}