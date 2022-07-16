using Microsoft.Win32;
using ServicesOfSchoolOfForeignLanguagesApp.AppData;
using static ServicesOfSchoolOfForeignLanguagesApp.Classes.Manager;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ServicesOfSchoolOfForeignLanguagesApp.Windows
{
    /// <summary>
    /// Interaction logic for AddEditServiceWindow.xaml
    /// </summary>
    public partial class AddEditServiceWindow : Window
    {
        readonly Service currentService = new Service();
        public AddEditServiceWindow(string title, Service service)
        {
            InitializeComponent();

            Title += title;
            if (service != null)
            {
                currentService = service;
            }
            DataContext = currentService;
            if (currentService.ID != 0)
            {
                BtnAddEdit.Content = "Редактировать услугу";
                IDPanel.Visibility = Visibility.Visible;
            }

            FilterAddonPhotos();
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                if (Context.Service.ToList().Where(s => s.Title.ToLower().Contains(TitleBox.Text.ToLower())).Count() > 0 && currentService.ID == 0)
                {
                    errors.AppendLine("В системе уже существует услуга с таким названием. Пожалуйста, выберите другое имя.");
                }
            }
            else
            {
                errors.AppendLine("Название не может быть пустым. Укажите название продукта.");
            }
            if (int.Parse(DurationBox.Text.Trim()) > 14400)
            {
                errors.AppendLine("Длительность не может быть больше 4 часов. Пожалуйста, укажите меньшую длительность.");
            }
            if (int.Parse(DurationBox.Text.Trim()) <= 0)
            {
                errors.AppendLine("Длительность не может быть отрицательной или равной нулю. Пожалуйста, укажите положительную длительность.");
            }
            if (decimal.Parse(CostBox.Text.Trim(), CultureInfo.InvariantCulture) <= 0)
            {
                errors.AppendLine("Стоимость не может быть отрицательной или равной нулю. Пожалуйста, укажите положительную стоимость.");
            }
            if (int.Parse(DiscountBox.Text.Trim()) < 0)
            {
                errors.AppendLine("Скидка не может быть отрицательной. Пожалуйста, укажите неотрицательную скидку.");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (currentService.ID != 0)
            {
                try
                {
                    Context.SaveChanges();
                    MessageBox.Show($"Услуга {currentService.Title} успешно отредактирована!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    Context.Service.Add(currentService);
                    Context.SaveChanges();
                    MessageBox.Show($"Услуга {currentService.Title} успешно добавлена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                ServiceImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                currentService.MainImage = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void BtnSelectAddonImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                currentService.ServicePhoto.Add(new ServicePhoto
                {
                    Photo = File.ReadAllBytes(openFileDialog.FileName)
                });

                FilterAddonPhotos();
            }
        }

        private void FilterAddonPhotos()
        {
            PicturesList.ItemsSource = currentService.ServicePhoto.ToList();
        }

        private void BtnDeleteAddonPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно удалить дополнительное фото?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var currentPhoto = (sender as Button).DataContext as ServicePhoto;
                Context.ServicePhoto.Remove(currentPhoto);

                MessageBox.Show("Дополнительное фото удалено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                FilterAddonPhotos();
            }
        }
    }
}
