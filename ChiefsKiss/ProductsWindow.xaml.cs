using ChiefsKiss.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChiefsKiss
{
    public partial class ProductsWindow
    {
        public ProductsWindow()
        {
            InitializeComponent();
            DataContext = App.User ?? App.EmptyUser;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.User = App.EmptyUser;
            new AuthWindow().Show();
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = await App.Context.Products.ToListAsync();
        }

        private void orderBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenOrder();
        }

        private void OpenOrder()
        {
            var selected = new List<Product>();
            foreach (var item in listView.SelectedItems)
                if (item is Product p)
                    selected.Add(p);

            if (selected.Any())
            {
                new OrderWindow(selected).ShowDialog();
                return;
            }

            MessageBox.Show(
                "Не выбран ни один товар!",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void openOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            new OrdersWindow().ShowDialog();
        }
    }
}
