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
    /// Interaction logic for AddDependency.xaml
    /// </summary>
    public partial class AddDependency : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        BO.Task curenntTask;
        BO.Engineer curenntEngineer;
        public AddDependency(int idTask, int idEng = 0)
        {
            InitializeComponent();
            TaskList = s_bl.Task.AllTaskDependency(idTask);
            curenntTask = s_bl.Task.Read(idTask);
            if (idEng != 0)
            {
                curenntEngineer = s_bl.Engineer.Read(idEng);
            }
        }

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("TaskList",
                typeof(IEnumerable<BO.TaskInList>),
                typeof(AddDependency),
                new PropertyMetadata(null));

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView)!.SelectedItem is BO.TaskInList selectedTask)
            {
                MessageBoxResult result = MessageBox.Show("האם ברצונך לבחור את המשימה הזאת להוספה כתלות?", "אישור בחירת משימה", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // יצירת עצם תלות חדש
                    DO.Dependency dependency = new()
                    {
                        DependentTask = curenntTask.Id, // משתנה idTask צריך להיות מוגדר במערכת ולהכיל את המזהה של המשימה הנוכחית
                        DependsOnTask = selectedTask.Id // המשימה שנבחרה על ידי המשתמש
                    };
                    DalApi.Factory.Get.Dependency.Create(dependency);
                    // הוספת התלות למערכת
                    //s_bl.Dependency.AddDependency(dependency);

                    // פתיחת חלון חדש להצגת המשימות וסגירת החלון הנוכחי
                    if (curenntEngineer != null)
                    {
                        ListOfTask listOfTask = new (curenntEngineer.Id);
                        listOfTask.Show();
                    }
                    Close();
                }
            }
        }



        //s_bl.Engineer.UpdateEngineerTask(selectedTask.Id, curenntEngineer.Id);
        //פעולה שתוסיף תלות עם המשימה הנוכחית והמשימה שנבחרה
    }
}
