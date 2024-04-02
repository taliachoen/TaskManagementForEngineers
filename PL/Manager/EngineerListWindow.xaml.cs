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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }

        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;
        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }
       
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("EngineerList",
                typeof(IEnumerable<BO.Engineer>),
                typeof(EngineerListWindow),
                new PropertyMetadata(null));


        private void AddItemEngineerButton_Click(object sender, RoutedEventArgs e)
        {
            EngineerWindow engineerWindow = new();
            engineerWindow.ShowDialog();
        }

        private void ExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.All) ?
              s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.GetEngineersByLevel((int)Experience)!;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView)!.SelectedItem is BO.Engineer selectedEngineer)
            {
                EngineerWindow engineerWindow = new(selectedEngineer.Id);
                engineerWindow.ShowDialog();
            }
        }

        private void EngineerWindow_Closed(object sender, EventArgs e)
        {
            // כאשר החלון נסגר, נעדכן את הרשימה בחלון תצוגת הרשימה
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }
    }

}

