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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for AddEngineerWindow.xaml
    /// </summary>
    public partial class EnterEngineerWindow : Window
    {
        private readonly DalApi.IDal dal = DalApi.Factory.Get;

        public EnterEngineerWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            // קבלת מספר תעודת הזהות מהקלט
            int engineerID = int.Parse(txtEngineerID.Text);

            // בדיקה האם המהנדס קיים במערכת
            var engineerExist = dal.Engineer.ReadAll(item => item.Id == engineerID);

            // אם המהנדס קיים, פתיחת חלון תצוגת המהנדס, אחרת הודעת שגיאה
            if (engineerExist != null)
            {
                // כאן תבצע את הפעולות הנדרשות לפתיחת חלון תצוגת המהנדס
                new EngineerListWindow().Show();
                LoadEngineerTasks(engineerID);

            }
            else
            {
                MessageBox.Show("Engineer not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadEngineerTasks(int engineerID)
        {
            // Load tasks for the engineer with the given ID
            // Populate lstTasks with the engineer's tasks
        }

        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            // Handle task update logic here
        }

    }
}
