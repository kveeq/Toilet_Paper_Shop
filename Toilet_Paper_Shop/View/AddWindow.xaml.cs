using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Toilet_Paper_Shop.db;

namespace Toilet_Paper_Shop.View
{
    public partial class AddWindow : Window
    {
        OpenFileDialog ofdImage = new OpenFileDialog();
        public AddWindow()
        {
            InitializeComponent();
            TypeCmb.ItemsSource = App.db.TypeProd.ToList();
            MaterialCmb.ItemsSource = App.db.Material.ToList();
        }

        private void btn_Image_Click(object sender, RoutedEventArgs e)
        {
            if (ofdImage.ShowDialog() == true)
            {   
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(ofdImage.FileName);
                image.EndInit();
                productImg.Source = image;
            }
        }

        private void PriceTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btn_ImageDel_Click(object sender, RoutedEventArgs e)
        {
            ofdImage.FileName = "";
            productImg.Source = null;
        }

        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {
            if (NameTxt.Text == "" || PriceTB.Text == "" || TypeCmb.SelectedIndex == -1 || MaterialCmb.SelectedIndex == -1 || ArticleTB.Text == "")
            {
                MessageBox.Show("Введите все данные!");
            }
            else
            {
                Product prod = new Product();
                prod.Name = NameTxt.Text;
                prod.MinCostForAgent = Convert.ToInt32(PriceTB.Text);
                prod.Material = (Material)MaterialCmb.SelectedItem;
                prod.Id_Prod = Convert.ToInt32(ArticleTB.Text);
                prod.TypeProd = (TypeProd)TypeCmb.SelectedItem;
                if (ofdImage.FileName != "")
                    prod.Picture = File.ReadAllBytes(ofdImage.FileName);
                App.db.Product.Add(prod);
                App.db.SaveChanges();
                MessageBox.Show("Продукт успешно добавлен");
                new ProductListWindow().Show();
                this.Close();
            }
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            NameTxt.Clear();
            PriceTB.Clear();
            ArticleTB.Clear();
            MaterialCmb.SelectedIndex = -1;
            TypeCmb.SelectedIndex = -1;
            btn_ImageDel_Click(null, null);
        }
    }
}
