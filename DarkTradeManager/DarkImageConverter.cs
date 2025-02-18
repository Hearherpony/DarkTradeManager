using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DarkTradeManager
{
    public class DarkImageConverter : IValueConverter
    {
        // Новый базовый путь к фотографиям товаров
        private const string BaseImagePath = @"D:/Teh/задание/Images/";
        // Путь к изображению-заглушке
        private const string DefaultImagePath = @"D:/Teh/задание/Images/picture.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;
            if (string.IsNullOrEmpty(path))
            {
                path = DefaultImagePath;
            }
            else if (!Path.IsPathRooted(path))
            {
                path = System.IO.Path.Combine(BaseImagePath, path);
            }
            if (!File.Exists(path))
            {
                path = DefaultImagePath;
            }
            try
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(path, UriKind.Absolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                return bmp;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
