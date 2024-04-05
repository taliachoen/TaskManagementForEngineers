using BO;
using PL.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for TaskInList.xaml
    /// </summary>
    public partial class ListOfTask : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        BO.Engineer curenntEngineer;
        public ListOfTask(int id)
        {
            InitializeComponent();
            curenntEngineer = s_bl.Engineer.Read(id);
            TaskList = s_bl.Task.AllTaskForEngineer(curenntEngineer.Level);
        }
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("TaskList",
                typeof(IEnumerable<BO.TaskInList>),
                typeof(ListOfTask),
                new PropertyMetadata(null));

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView)!.SelectedItem is BO.TaskInList selectedTask)
            {
                MessageBoxResult result = MessageBox.Show("האם ברצונך לבחור את המשימה הזאת?", "אישור בחירת משימה", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    s_bl.Engineer.UpdateEngineerTask(selectedTask.Id, curenntEngineer.Id);
                    ShowEngineerTask showEngineerTask = new(curenntEngineer.Id);
                    showEngineerTask.Show();
                    Close();
                }
            }
        }
        

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow taskWindow = new();
            taskWindow.ShowDialog();
        }

    }
}


