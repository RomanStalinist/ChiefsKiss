using ChiefsKiss.Data;
using System;
using System.Linq;
using System.Windows;

namespace ChiefsKiss
{
    public partial class App : Application
    {
        public static User User = EmptyUser;
        public static readonly PovarEntities Context = new PovarEntities();
        public static readonly User EmptyUser = new User
        {
            UserName = "Гость"
        };

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Window mWindow;

            if (TryGetLoginAndPasswordFromArgs(e.Args, out var login, out var password))
            {
                // Попытка аутентификации пользователя по логину и паролю
                User = Context.Users.FirstOrDefault(u => u.UserLogin == login && u.UserPassword == password);
                if (User is null)
                {
                    User = EmptyUser;
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    mWindow = new AuthWindow();
                }
                else
                    mWindow = new ProductsWindow();
            }
            else if (e.Args.Contains("/no-auth"))
                mWindow = new ProductsWindow();
            else
                mWindow = new AuthWindow();

            MainWindow = mWindow;
            mWindow.Show();
        }

        private bool TryGetLoginAndPasswordFromArgs(string[] args, out string login, out string password)
        {
            login = null;
            password = null;

            if (args.Length < 3)
                return false;

            if (args[0] == "/auth")
            {
                login = args[1];
                password = args[2];
                return !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password);
            }

            return false;
        }
    }
}