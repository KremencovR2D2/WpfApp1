using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Вспомогательный класс для хранения Uri картинок в ресурсах
    /// </summary>
    public class ImageUrl : DependencyObject
    {
        public static readonly DependencyProperty ImageUriProperty = DependencyProperty.Register("ImageUri", typeof(Uri), typeof(ImageUrl), new PropertyMetadata(default(Uri), ImageUriChanged));
        private static void ImageUriChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var imageUrl = dependencyObject as ImageUrl;
            if (imageUrl == null)
                return;
            var oldValue = (Uri)dependencyPropertyChangedEventArgs.OldValue;
            var newValue = (Uri)dependencyPropertyChangedEventArgs.NewValue;
            /* Logic... */
        }

        /// <summary>
        /// Ссылка на кртинку.
        /// Использовать формат: /&lt;App&gt;;component/&lt;ImagePath&gt;
        /// </summary>
        public Uri ImageUri
        {
            get { return (Uri)GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }
    }
}
