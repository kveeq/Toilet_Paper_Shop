using System.Windows;
using Toilet_Paper_Shop.db;

namespace Toilet_Paper_Shop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ToiletPaper_dbEntities db = new ToiletPaper_dbEntities();
    }
}
