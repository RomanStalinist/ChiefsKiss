using ChiefsKiss.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;

namespace ChiefsKiss
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private Order _order;
        private List<Product> _selectedProducts;

        public OrderWindow(List<Product> products)
        {
            InitializeComponent();
            _selectedProducts = products;
            _order = new Order
            {
                UserID = App.User.UserID,
                OrderDate = DateTime.Now,
                OrderStatus = "В процессе",
                Products = _selectedProducts,
                OrderDeliveryDate = DateTime.Now.AddDays(1)
            };
        }

        private async void orderButton_Click(object sender, RoutedEventArgs e)
        {
            App.Context.Orders.AddOrUpdate(DataContext as Order);
            await App.Context.SaveChangesAsync();
            MessageBox.Show("Успех!");
            Close();
        }

        private void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckButton();
        }

        private void CheckButton()
        {
            orderButton.IsEnabled = _order.OrderDate != null
                && _order.OrderDeliveryDate != null
                && _order.OrderDeliveryDate > _order.OrderDate
                && _order.PickupPoint != null;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = await App.Context.PickupPoints.AsNoTracking().ToListAsync();
            if (!data.Any())
            {
                MessageBox.Show("Не добавлено ни одного ПВЗ");
                Close();
                return;
            }

            _order.Code = await App.Context.Orders.AsNoTracking().MaxAsync(o => o.Code) + 1;
            pickupPoints.ItemsSource = data;
            _order.PickupPoint = data[0];
            DataContext = _order;
            CheckButton();
        }
    }
}
