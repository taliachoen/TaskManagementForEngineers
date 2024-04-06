using BlApi;
using PL.Engineer;
using PL.Manager;
using System.ComponentModel;
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
using System.Windows.Threading;

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
            MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize all data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                s_bl.InitializeDB();
            }
        }

        private void ResetData_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset all data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
            }
        }

        private void CreatingSchedule_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to Creating a schedule now?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                s_bl.UpdateProjectSchedule(s_bl.Clock);
            }
        }

        private void ViewTasks_Click(object sender, RoutedEventArgs e)
        {
            // Open a new Engineer List Window
            new TaskListWindow().Show();
        }


        //private void GanttChart_Click(object sender, RoutedEventArgs e)
        //{
        //    // Open the Gantt Chart window
        //    new Gantt().Show();
        //}

    }
}