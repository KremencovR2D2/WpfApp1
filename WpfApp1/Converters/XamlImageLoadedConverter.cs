using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using WpfApp1.Styles;

namespace WpfApp1.Converters
{
    /// <summary>
    /// Конвертатор для загрузки Xaml картинок из ресурсного файла.
    /// В качестве значения использует ImageUrl.
    /// </summary>
    [ValueConversion(typeof(Uri), typeof(UIElement))]
    [ValueConversion(typeof(ImageUrl), typeof(UIElement))]
    public class XamlImageLoaderConverter : IValueConverter
    {
        //private static Regex _regexPath;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            Uri uri;
            if (value is Uri uri1)
                uri = uri1;
            else if (value is ImageUrl)
                uri = ((ImageUrl)value).ImageUri;
            else
            {
                Trace.WriteLine($"XamlImageLoaderConverter: NotSupportedException: {value.GetType()}");
                return DependencyProperty.UnsetValue;
            }


            try
            {
                using (var stream = GetStream(uri))
                {
                    var img = XamlReader.Load(stream);
                    return img;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"XamlImageLoaderConverter: Load Xaml image: {ex}");
                return DependencyProperty.UnsetValue;
            }
        }

        private static Stream GetStream(Uri uri)
        {
            //if (DesignTimeMode.IsDesignMode)
            //    return File.OpenRead(uri.AbsolutePath);

            var streamResourceInfo = Application.GetResourceStream(uri);
            if (streamResourceInfo == null)
            {
                throw new NullReferenceException("streamResourceInfo");
            }

            var stream = streamResourceInfo.Stream;
            if (stream == null)
            {
                throw new NullReferenceException("streamResourceInfo.Stream");
            }
            return stream;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //private Uri RelativeUri(Uri uri)
        //{
        //    if (_regexPath == null)
        //        _regexPath = new Regex(@"pack:\/\/application:,[^,]*,[^,]*,[^\/]*|\/(?<component>[^;]+);component(?<path>(\/|\w|\.)+)");
        //    var match = _regexPath.Match(uri.OriginalString);
        //    if (match.Groups["path"] == null)
        //    {
        //        throw new NullReferenceException($"uri: {uri}, match.Groups[\"path\"]: null");
        //        //return uri;
        //    }
        //    var relativeUri = new Uri($"${match.Groups["path"].Value.Replace("/", "\\")}", UriKind.Relative);
        //    return relativeUri;
        //}
    }
}
