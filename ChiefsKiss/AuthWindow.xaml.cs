using ChiefsKiss.Data;
using System;
using System.Data.Entity;
using System.Windows;

namespace ChiefsKiss
{
    public partial class AuthWindow
    {
        public AuthWindow()
        {
            InitializeComponent();
            App.User = App.User is null ? App.EmptyUser : App.User;
            DataContext = App.User;
        }

        private void passBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as User).UserPassword = passBox.Password;
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var form = DataContext as User;
                if (string.IsNullOrEmpty(form.UserLogin)
                    || string.IsNullOrEmpty(form.UserPassword))
                    throw new Exception("Заполните все поля!");

                var user = await App.Context.Users.FirstOrDefaultAsync(
                    u => u.UserLogin == form.UserLogin && u.UserPassword == form.UserPassword)
                    ?? throw new Exception("Неверный логин или пароль!");

                App.User = user;
                SwitchToProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void withoutLoginButton_Click(object sender, RoutedEventArgs e)
        {
            App.User = App.EmptyUser;
            SwitchToProducts();
        }

        private void SwitchToProducts()
        {
            new ProductsWindow().Show();
            Close();
        }

        private void regLink_Click(object sender, RoutedEventArgs e)
        {
            new RegWindow().Show();
            Close();
        }
    }
}
