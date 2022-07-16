using ServicesOfSchoolOfForeignLanguagesApp.Classes;
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

namespace ServicesOfSchoolOfForeignLanguagesApp.Windows
{
    /// <summary>
    /// Interaction logic for CodeEnterWindow.xaml
    /// </summary>
    public partial class CodeEnterWindow : Window
    {
        public CodeEnterWindow()
        {
            InitializeComponent();
        }

        private void BtnActivate_Click(object sender, RoutedEventArgs e)
        {
            if(CodeBox.Password == "0000")
            {
                Manager.IsAdminMode = true;
                MessageBox.Show("Режим администратора активирован!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Неверный код. Проверьте написание.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
