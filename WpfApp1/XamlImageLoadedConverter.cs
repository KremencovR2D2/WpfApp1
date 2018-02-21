using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfApp1
{
    /// <summary>
    /// Конвертатор для загрузки Xaml картинок из ресурсного файла.
    /// В качестве значения использует ImageUrl.
    /// </summary>
    [ValueConversion(typeof(Uri), typeof(UIElement))]
    [ValueConversion(typeof(ImageUrl), typeof(UIElement))]
    public class XamlImageLoaderConverter : IValueConverter
    {
        private static Regex _regexPath;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            Uri uri;
            if (value is Uri)
                uri = (Uri)value;
            else if (value is ImageUrl)
                uri = ((ImageUrl)value).ImageUri;
            else
            {
                Trace.WriteLine(string.Format("XamlImageLoaderConverter: NotSupportedException: {0}", value.GetType()));
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
                Trace.WriteLine(string.Format("XamlImageLoaderConverter: Load Xaml image: {0}", ex));
                return DependencyProperty.UnsetValue;
            }
        }

        private static Stream GetStream(Uri uri)
        {
            Stream stream;
            //if (DesignTimeMode.IsDesignMode)
            //    return File.OpenRead(uri.AbsolutePath);

            var streamResourceInfo = Application.GetResourceStream(uri);
            if (streamResourceInfo == null)
            {
                throw new NullReferenceException("streamResourceInfo");
            }

            stream = streamResourceInfo.Stream;
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

        private Uri RelativeUri(Uri uri)
        {
            if (_regexPath == null)
                _regexPath = new Regex(@"pack:\/\/application:,[^,]*,[^,]*,[^\/]*|\/(?<component>[^;]+);component(?<path>(\/|\w|\.)+)");
            var match = _regexPath.Match(uri.OriginalString);
            if (match.Groups["path"] == null)
            {
                throw new NullReferenceException(string.Format("uri: {0}, match.Groups[\"path\"]: null", uri));
                //return uri;
            }
            var relativeUri = new Uri(string.Format("${0}", match.Groups["path"].Value.Replace("/", "\\")), UriKind.Relative);
            return relativeUri;
        }
    }
}
