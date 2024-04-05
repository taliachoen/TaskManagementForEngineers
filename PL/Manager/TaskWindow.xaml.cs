using PL.Engineer;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                string propertyName = ((Button)sender).Content.ToString()!;

                if (propertyName == "Update")
                    s_bl.Task.Update(CurrentTask);
                else
                    s_bl.Task.Create(CurrentTask);

                Close(); // Close the window after updating or adding a task
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating property: {ex.Message}");
            }
        }

        private void AddDependency_Click(object sender, RoutedEventArgs e)
        {
            AddDependency addDependencyWindow = new AddDependency(CurrentTask.Id);
            addDependencyWindow.Show();
        }

    }
}
