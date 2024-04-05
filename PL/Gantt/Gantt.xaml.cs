using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Gantt.xaml
    /// </summary>
    public partial class Gantt : Window
    {
        static readonly IBl s_bl = Factory.Get();

        List<TaskInGantt> ganttTasksList;
        DateTime StartInDeed;
        DateTime FinishInDeed;

        public DataTable table
        {
            get { return (DataTable)GetValue(tableProperty); }
            set { SetValue(tableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for table.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty tableProperty =
            DependencyProperty.Register("table", typeof(DataTable), typeof(Gantt), new PropertyMetadata(null));


        public Gantt()
        {
            //Make the showen list
            ganttTasksList = s_bl.Task.ReadAll(t => t.Engineer != null && t.StartDate != null).Where(t => t != null).Select(task => new TaskInGantt(task!)).ToList();
            StartInDeed = ganttTasksList.Min(t => t.StartDate);
            FinishInDeed = ganttTasksList.Max(t => t.EndDate);
            buildTable();
            InitializeComponent();
        }
        private void buildTable()
        {
            table = new DataTable();
            table.Columns.Add("Task Id", typeof(int));
            table.Columns.Add("Task Name", typeof(string));
            table.Columns.Add("Engineer Id", typeof(int));
            table.Columns.Add("Engineer Name", typeof(string));
            int col = 4;
            for (DateTime d = StartInDeed; d <= FinishInDeed; d = d.AddDays(1))
            {
                string s_date = $"{d.Day}-{d.Month}-{d.Year}";
                table.Columns.Add(s_date, typeof(string));
                col++;
            }

            IEnumerable<TaskInGantt> orderedTaskList = ganttTasksList.OrderBy(t => t.StartDate);
            foreach (TaskInGantt task in orderedTaskList)
            {
                DataRow row = table.NewRow();
                row[0] = task.TaskId;
                row[1] = task.TaskName;
                row[2] = task.EngineerId;
                row[3] = task.EngineerName;

                for (DateTime d = StartInDeed; d <= FinishInDeed; d = d.AddDays(1))
                {
                    string s_date = $"{d.Day}-{d.Month}-{d.Year}";
                    if (d < task.StartDate || d > task.EndDate)
                        row[s_date] = Status.Unscheduled;
                    else
                        row[s_date] = task.Status;
                }
                table.Rows.Add(row);
            }
        }
    }
}