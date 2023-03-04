using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Toilet_Paper_Shop.db;

namespace Toilet_Paper_Shop.View
{
    public partial class ProductListWindow : Window
    {

        const int pageSize = 20;
        int pageNumber;
        List<Product> prod = App.db.Product.ToList();

        public ProductListWindow()
        {
            InitializeComponent();
            var lst = App.db.Product.ToList();
            foreach (var item in lst)
            {
                item.Picture = item.Picture ?? File.ReadAllBytes(@"..\..\Resources\plug.png");
            }
            ProductLst.ItemsSource = lst;
            RefreshPagination();
            FilterCmb.ItemsSource = App.db.TypeProd.ToList();
        }

        private void BLeft_Click(object sender, RoutedEventArgs e)
        {
            if (pageNumber == 0)
                return;
            pageNumber--;
            RefreshPagination();
        }

        private void BRight_Click(object sender, RoutedEventArgs e)
        {
            if (prod.Count % pageSize == 0)
            {
                if (pageNumber == (prod.Count / pageSize) - 1)
                    return;
            }
            else
            {
                if (pageNumber == (prod.Count / pageSize))
                    return;
            }

            pageNumber++;
            RefreshPagination();
        }

        private void RefreshPagination()
        {
            ProductLst.ItemsSource = null;
            ProductLst.ItemsSource = prod.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            RefreshButtons();
        }

        private Button GetButton(int content)
        {
            Button button = new Button();
            button.Content = content;
            button.Click += Button_Click;
            button.Margin = new Thickness(0, 5, 0, 5);
            button.Width = 25;
            Padding = new Thickness(0);
            button.Height = 25;
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.BorderBrush = Brushes.White;
            button.Background = Brushes.White;
            return button;
        }

        private void RefreshButtons()
        {
            BtnsPnl.Children.Clear();
            if (prod.Count % pageSize == 0)
            {
                for (int i = 0; i < prod.Count / pageSize; i++)
                {
                    BtnsPnl.Children.Add(GetButton(i + 1));
                }
            }
            else
            {
                for (int i = 0; i < prod.Count / pageSize; i++)
                {
                    BtnsPnl.Children.Add(GetButton(i + 1));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            pageNumber = Convert.ToInt32(button.Content) - 1;
            RefreshPagination();
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var lst = App.db.Product.OrderBy(a => a.Name).ToList();
            lst = lst.Where(a => a.Name.ToLower().Contains(SearchTB.Text.ToLower())).ToList();
            ProductLst.ItemsSource = lst;
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterCmb.SelectedIndex == -1)
                return;

            SortCmb.SelectedIndex = -1;
            var typeName = ((TypeProd)FilterCmb.SelectedItem).NameType;
            ProductLst.ItemsSource = App.db.Product.Where(x => x.TypeProd.NameType == typeName).ToList();
        }

        private void ProductLst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (Product)ProductLst.SelectedItem;
            var newWindow = new EditWindow(item.Id_Prod);
            newWindow.Show();
            newWindow.Activate();
            newWindow.Topmost = true;
            newWindow.Closing += ((s, ce) => { this.IsHitTestVisible = true; RefreshPagination(); });
            this.Activated += (s, ce) => newWindow.Activate();
            this.IsHitTestVisible = false;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            new AddWindow().Show();
            this.Close();
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortCmb.SelectedIndex == -1)
                return;

            FilterCmb.SelectedIndex = -1;
            if (SortCmb.SelectedIndex == 1)
            {
                ProductLst.ItemsSource = App.db.Product.OrderByDescending(x => x.MinCostForAgent).ToList();
                return;
            }

            ProductLst.ItemsSource = App.db.Product.OrderBy(x => x.MinCostForAgent).ToList();
        }

        private void DelBTN_Click(object sender, RoutedEventArgs e)
        {
            var q = ProductLst.SelectedItem as Product;
            if (q == null)
            {
                MessageBox.Show("Ничего не выбрано!");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить продукт?", "Удалить?", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var deleteItem = App.db.Product.Where(x => x.Id_Prod == q.Id_Prod).FirstOrDefault();
                    App.db.MaterialsAndProducts.RemoveRange(App.db.MaterialsAndProducts.Where(x => x.Id_Prod == q.Id_Prod));
                    App.db.Product.Remove(deleteItem);
                    App.db.SaveChanges();
                    ProductLst.ItemsSource = App.db.Product.ToList();
                    MessageBox.Show("Выполнено!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка!\n" + ex.Message);
                }
            }
        }
    }
}