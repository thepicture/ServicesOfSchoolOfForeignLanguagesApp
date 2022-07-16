using ServicesOfSchoolOfForeignLanguagesApp.AppData;
using ServicesOfSchoolOfForeignLanguagesApp.Classes;
using ServicesOfSchoolOfForeignLanguagesApp.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ServicesOfSchoolOfForeignLanguagesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Service> services = new List<Service>();
        readonly List<string> sortElements = new List<string>();
        readonly List<string> filterElements = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            sortElements.Add("По возрастанию");
            sortElements.Add("По убыванию");

            filterElements.Add("Все");
            filterElements.Add("От 0 до 5%");
            filterElements.Add("От 5 до 15%");
            filterElements.Add("От 15 до 30%");
            filterElements.Add("От 30 до 70%");
            filterElements.Add("От 70 до 100%");

            ComboFilter.ItemsSource = filterElements;
            ComboFilter.SelectedIndex = 0;

            ComboSort.ItemsSource = sortElements;
            ComboSort.SelectedIndex = 0;

            FilterServices();

            Manager.IsAdminMode = true;
        }

        private void FilterServices()
        {
            services = Manager.Context.Service.ToList();

            services = services.Where(s => s.Title.ToLower().Contains(SearchBox.Text.Trim().ToLower()) || (s.Description == null ? "" : s.Description).ToLower().Contains(SearchBox.Text.ToLower())).ToList();

            switch (ComboFilter.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    services = services.Where(s => (s.Discount == null) || (s.Discount < 0.05)).ToList();
                    break;
                case 2:
                    services = services.Where(s => (s.Discount >= 0.05) && (s.Discount < 0.15)).ToList();
                    break;
                case 3:
                    services = services.Where(s => (s.Discount >= 0.15) && (s.Discount < 0.3)).ToList();
                    break;
                case 4:
                    services = services.Where(s => (s.Discount >= 0.3) && (s.Discount < 0.7)).ToList();
                    break;
                case 5:
                    services = services.Where(s => (s.Discount >= 0.7) && (s.Discount < 1)).ToList();
                    break;
            }

            switch (ComboSort.SelectedIndex)
            {
                case 0:
                    services = services.OrderBy(s => s.GetRealCost).ToList();
                    break;
                case 1:
                    services = services.OrderByDescending(s => s.GetRealCost).ToList();
                    break;
            }

            ServiceView.ItemsSource = services;
            CountBox.Text = $"Выведено {services.Count} из {Manager.Context.Service.Count()} записей";
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void EnterCodeItem_Click(object sender, RoutedEventArgs e)
        {
            var codeEnterWindow = new CodeEnterWindow
            {
                Owner = this
            };
            codeEnterWindow.ShowDialog();
            if (Manager.IsAdminMode)
            {
                VisibilityGrid.Visibility = Visibility.Visible;
                EnterCodeItem.IsEnabled = false;
                ServiceView.ItemsSource = Manager.Context.Service.ToList();
                Title = "Услуги школы иностранных языков - Режим администратора";
            }
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterServices();
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterServices();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterServices();
        }

        private void BtnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            var service = (sender as Button).DataContext as Service;
            if (MessageBox.Show($"Точно удалить запись {service.Title}?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            {
                return;
            }
            if (service.ClientService.Count > 0)
            {
                MessageBox.Show("Удаление запрещено. У услуги есть прошлые или будущие записи. Пожалуйста, дождитесь предоставления услуг клиентам до удаления.",
                    "Операция запрещена", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (service.ServicePhoto.Count > 0)
            {
                foreach (var photo in service.ServicePhoto)
                {
                    Manager.Context.ServicePhoto.Remove(photo);
                }
            }
            Manager.Context.Service.Remove(service);
            try
            {
                Manager.Context.SaveChanges();
                MessageBox.Show($"Запись {service.Title} удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                FilterServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            var addEditServiceWindow = new AddEditServiceWindow("Добавление услуги", null)
            {
                Owner = this
            };
            addEditServiceWindow.ShowDialog();

            FilterServices();
        }

        private void BtnEditService_Click(object sender, RoutedEventArgs e)
        {
            var addEditServiceWindow = new AddEditServiceWindow("Редактирование услуги", (sender as Button).DataContext as Service)
            {
                Owner = this
            };
            addEditServiceWindow.ShowDialog();

            FilterServices();
        }
    }
}
