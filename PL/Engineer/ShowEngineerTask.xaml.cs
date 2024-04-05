using System;
using System.Windows;

namespace PL.Engineer
{
    public partial class ShowEngineerTask : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public ShowEngineerTask(int engineerID)
        {
            InitializeComponent();
            CurrentEngineerTask = s_bl.Task.ReadAll(task => task.Engineer!.Id == engineerID).FirstOrDefault()!;
        }

        // Property for binding the task details
        public BO.Task CurrentEngineerTask
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        // Dependency property for binding
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("CurrentEngineerTask",
                typeof(BO.Task),
                typeof(ShowEngineerTask),
                new PropertyMetadata(null));

        // Click event for Update Task button
        private void UpdateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.Update(CurrentEngineerTask);
                Close(); // Close the window after updating the task
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating property: {ex.Message}");
            }
        }

        // Click event for Complete Task button
        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentEngineerTask.Status = BO.Status.Done;
                CurrentEngineerTask.CompleteDate = DateTime.Now;
                s_bl.Task.Update(CurrentEngineerTask);
                Close(); // Close the window after completing the task
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error completing task: {ex.Message}");
            }
        }
    }
}
