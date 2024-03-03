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

        private void AddItemEngineerButton_Click(object sender, RoutedEventArgs e)
        {
            // פתיחת חלון חדש (לדוגמה, אפשר להשתמש בתיבת דו-שיח פשוטה)
            AddEngineerWindow addItemWindow = new();
            addItemWindow.ShowDialog();
            // כאן תוכל לטפל במידע שהוסף לרשימה או לעשות כל פעולה נדרשת לאחר סגירת החלון החדש
        }

        private void ExperianceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.All) ?
                s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Experience)!;
        }


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

       


    }

}

