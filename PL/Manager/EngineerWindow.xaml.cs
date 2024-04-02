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
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        
        public EngineerWindow(int Id = 0)
        {
        
            InitializeComponent();
            if (Id == 0)
            {
                CurrentEngineer = new BO.Engineer();
            }
            else
            {
                try
                {
                    // Call the BL method to get the existing object by Id
                    CurrentEngineer = s_bl.Engineer.Read(Id);
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., engineer not found)
                    MessageBox.Show($"Error: {ex.Message}");
                }
                Closed += EngineerWindow_Closed!;
            }
        }


        private void EngineerWindow_Closed(object sender, EventArgs e)
        {
            var mainWindow = Application.Current.Windows
                                            .OfType<EngineerListWindow>()
                                            .FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.EngineerList = s_bl.Engineer.ReadAll()!;
            }
        }

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("CurrentEngineer",
                typeof(BO.Engineer),
                typeof(EngineerWindow),
                new PropertyMetadata(null));

        private void BtnAddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEngineer.Id == 0)
            {
                // מצב הוספה
                s_bl.Engineer.Create(CurrentEngineer);
                MessageBox.Show("Engineer added successfully!");
            }
            else
            {
                // מצב עדכון
                s_bl.Engineer.Update(CurrentEngineer);
                MessageBox.Show("Engineer updated successfully!");
            }
            Close(); // סגירת החלון לאחר ביצוע הפעולה        }
        }

        private void UpdatePropertyValue(object sender, RoutedEventArgs e)
        {
            try
            {
                // קבלת שם הפרמטר ששולחים מהכפתור
                string? propertyName = ((Button)sender).Content.ToString();

                if (propertyName == "Update")
                    s_bl.Engineer.Update(CurrentEngineer);
                else
                    s_bl.Engineer.Create(CurrentEngineer);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating property: {ex.Message}");
            }
        }



    }
}
