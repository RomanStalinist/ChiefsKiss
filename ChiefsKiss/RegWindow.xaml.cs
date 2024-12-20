using ChiefsKiss.Data;
using System;
using System.Data.Entity;
using System.Windows;

namespace ChiefsKiss
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
            App.User = App.EmptyUser;
            App.User.UserName = string.Empty;
            DataContext = App.User;
        }

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            App.User = App.EmptyUser;
            new AuthWindow().Show();
            Close();
        }

        private async void regButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var form = DataContext as User;

                var user = await App.Context.Users.FirstOrDefaultAsync(
                    u => u.UserLogin == form.UserLogin);

                if (user != null)
                    throw new Exception($"Имя '{form.UserLogin}' уже занято!");

                form.UserRole = 1;
                App.Context.Users.Add(form);
                await App.Context.SaveChangesAsync();

                MessageBox.Show($"Добро пожаловать, {form.UserName}!");
                new AuthWindow().Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void passBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as User).UserPassword = passBox.Password;
            CheckButton();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckButton();
        }

        private void CheckButton()
        {
            var user = DataContext as User;
            regButton.IsEnabled = !string.IsNullOrEmpty(user.UserName)
                && !string.IsNullOrEmpty(user.UserSurname)
                && !string.IsNullOrEmpty(user.UserPassword)
                && !string.IsNullOrEmpty(user.UserLogin);
        }
    }
}
