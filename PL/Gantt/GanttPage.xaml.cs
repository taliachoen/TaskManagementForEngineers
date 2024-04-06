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

namespace PL.Gantt
{
    /// <summary>
    /// Interaction logic for GanttPage.xaml
    /// </summary>
    public partial class GanttPage : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public GanttPage()
        {
            InitializeComponent();
            listTasksSchedule = s_bl!.Task.GetAllScheduleTasks();
        }

        // Property for binding the task details
        public List<BO.TaskScheduleDays> listTasksSchedule
        {
            get { return (List<BO.TaskScheduleDays>)GetValue(listTasksSchedualeProperty); }
            set { SetValue(listTasksSchedualeProperty, value); }
        }

        // Dependency property for binding
        public static readonly DependencyProperty listTasksSchedualeProperty =
            DependencyProperty.Register("listTasksSchedule",
                typeof(List<BO.TaskScheduleDays>),
                typeof(GanttPage),
                new PropertyMetadata(null));

      

    }
}
