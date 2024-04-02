using PL.Engineer;
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
using System.Windows.Shapes;

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    
        public TaskWindow(int taskId = 0)
        {

            InitializeComponent();
            if (taskId == 0)
            {
                CurrentTask = new BO.Task();
            }
            else
            {
                try
                {
                    CurrentTask = s_bl.Task.Read(taskId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            Closed += TaskWindow_Closed!;

        }

        private void TaskWindow_Closed(object sender, EventArgs e)
        {
            var mainWindow = Application.Current.Windows
                                            .OfType<TaskListWindow>()
                                            .FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.TaskList = s_bl.Task.ReadAll()!;
            }
        }

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("CurrentTask",
                typeof(BO.Task),
                typeof(TaskWindow),
                new PropertyMetadata(null));

        private void UpdatePropertyValue(object sender, RoutedEventArgs e)
        {

            try
            {
                string? propertyName = ((Button)sender).Content.ToString();

                if (propertyName == "Update")
                    s_bl.Task.Update(CurrentTask);
                else
                    s_bl.Task.Create(CurrentTask);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating property: {ex.Message}");
            }
        }
    }


   

}

