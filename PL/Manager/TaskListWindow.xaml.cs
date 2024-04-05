/*using PL.Engineer;
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
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl?.Task.ReadAll()!;
        }

        public IEnumerable<BO.Task> TaskList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public int EngineerIdFilter
        {
            get { return (int)GetValue(EngineerIdFilterProperty); }
            set { SetValue(EngineerIdFilterProperty, value); }
        }

        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;
        public BO.Status Status { get; set; } = BO.Status.All;

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("TaskList",
                typeof(IEnumerable<BO.Task>),
                typeof(TaskListWindow),
                new PropertyMetadata(null));

        public static readonly DependencyProperty EngineerIdFilterProperty =
          DependencyProperty.Register("EngineerIdFilter",
              typeof(int),
              typeof(TaskListWindow),
              new PropertyMetadata(0, EngineerIdFilterChangedCallback));


        private void EngineerIdSelector_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (EngineerIdFilter != 0)
            {
                TaskList = s_bl?.Task.FilterTasksById(EngineerIdFilter)!;
            }
            else
            {
                TaskList = s_bl?.Task.ReadAll()!;
            }
        }

        private void ExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Experience == BO.EngineerExperience.All) ?
                s_bl?.Task.ReadAll()! : s_bl?.Task.GeTaskByCopmlexity((int)Experience)!;
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Status == BO.Status.All) ?
                s_bl?.Task.ReadAll()! : s_bl?.Task.GeTaskByStatus((int)Status)!;
        }

        private void UpdateTaskListByEngineerId(int engineerId)
        {
            EngineerIdFilter = engineerId;
            if (engineerId != 0)
            {
                TaskList = s_bl?.Task.ReadAll(task => (bool)(task.Engineer?.Id.ToString().StartsWith(engineerId.ToString())));
                //TaskList = s_bl?.Task.ReadAll(task => FilterTasksById(engineerId).Any(t => t.Id == task.Id)) ?? Enumerable.Empty<BO.Task>();
                //TaskList = s_bl?.Task.FilterTasksById(engineerId).Any(t => t.Id == task.Id)) ?? Enumerable.Empty<BO.Task>();
                //TaskList = s_bl?.Task.FilterTasksById(engineerId);
            }
            else
            {
                TaskList = s_bl?.Task.ReadAll()!;
            }
        }
     
        private void TaskWindow_Closed(object sender, EventArgs e)
        {
            TaskList = s_bl?.Task.ReadAll()!;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView)!.SelectedItem is BO.Task selectedTask)
            {
                TaskWindow taskWindow = new(selectedTask.Id);
                taskWindow.Closed += TaskWindow_Closed!;
                taskWindow.ShowDialog();
            }
        }

        private static void EngineerIdFilterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as TaskListWindow;
            if (window != null)
            {
                window.UpdateTaskListByEngineerId((int)e.NewValue);
            }
        }

        private void EngineerIdFilterTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // אם המשתמש לחץ Enter, נחשב את הערך כאשר הוא מוחלף
                BindingExpression binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow taskWindow = new();
            taskWindow.ShowDialog();
        }
    }
}*/

using PL.Engineer; // משתנה הספריות שהפרויקט משתמש בהן
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

namespace PL.Manager // הקוד שיופעל במערכת הניהול
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // השלט בלוגיקת העסק
        // אינטרפייס שבו יש מימוש ב-BlApi הגדרנו IBl בתוך BlApi, וכאן אנו קוראים לפעולות הגדולות

        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl?.Task.ReadAll()!;
        }

        // מאפיינים להתנגשות של הערכים בכל מקום
        public IEnumerable<BO.Task> TaskList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public int EngineerIdFilter
        {
            get { return (int)GetValue(EngineerIdFilterProperty); }
            set { SetValue(EngineerIdFilterProperty, value); }
        }

        // הגדרת ערכי ברירת מחדל עבור המאפיינים
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;
        public BO.Status Status { get; set; } = BO.Status.All;

        // הגדרת המאפיינים שיש להם אפשרות להתעדכן מה-XAML
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("TaskList",
                typeof(IEnumerable<BO.Task>),
                typeof(TaskListWindow),
                new PropertyMetadata(null));

        public static readonly DependencyProperty EngineerIdFilterProperty =
          DependencyProperty.Register("EngineerIdFilter",
              typeof(int),
              typeof(TaskListWindow),
              new PropertyMetadata(0, EngineerIdFilterChangedCallback));


        private void EngineerIdSelector_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // עדכן את רשימת המשימות בהתאם למהנדס שנבחר
            if (EngineerIdFilter != 0)
            {
                TaskList = s_bl?.Task.FilterTasksById(EngineerIdFilter)!;
            }
            else
            {
                TaskList = s_bl?.Task.ReadAll()!;
            }
        }

        // מתודות לעדכון רשימת המשימות בהתאם לסינון של המשתמש
        private void ExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Experience == BO.EngineerExperience.All) ?
                s_bl?.Task.ReadAll()! : s_bl?.Task.GeTaskByCopmlexity((int)Experience)!;
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Status == BO.Status.All) ?
                s_bl?.Task.ReadAll()! : s_bl?.Task.GeTaskByStatus((int)Status)!;
        }

        private static void EngineerIdFilterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as TaskListWindow;
            if (window != null)
            {
                window.UpdateTaskListByEngineerId((int)e.NewValue);
            }
        }

        private void TaskWindow_Closed(object sender, EventArgs e)
        {
            TaskList = s_bl?.Task.ReadAll()!;
        }

        // פתיחת חלון פרטי המשימה במקרה של קליק כפול על אחת מהמשימות ברשימה
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView)!.SelectedItem is BO.Task selectedTask)
            {
                TaskWindow taskWindow = new(selectedTask.Id);
                taskWindow.Closed += TaskWindow_Closed!;
                taskWindow.ShowDialog();
            }
        }

        // עדכון רשימת המשימות עם שינוי במזהה המהנדס
        private void UpdateTaskListByEngineerId(int engineerId)
        {
            EngineerIdFilter = engineerId;
            if (engineerId != 0)
            {
                TaskList = s_bl.Task.ReadAll(task => (bool)(task.Engineer!.Id.ToString().StartsWith(engineerId.ToString())));
            }
            else
            {
                TaskList = s_bl?.Task.ReadAll()!;
            }
        }

        // פתיחת החלון ליצירת משימה חדשה
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow taskWindow = new();
            taskWindow.ShowDialog();
        }
    }
}
