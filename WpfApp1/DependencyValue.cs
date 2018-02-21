using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Класс для проброски значений в стилях
    /// </summary>
    public static class DependencyValue
    {
        /*
          Regexp для генерации DependencyProperty:
            public\s+(?<type>\w+)\s+(?<name>\w+)\s+\{\s*get;\s*set;\s*\}
            \npublic static readonly DependencyProperty $2Property = DependencyProperty.RegisterAttached("$2", typeof($1), typeof(zzz));\rpublic static $1 Get$2(DependencyObject d)\r{\rreturn ($1)d.GetValue($2Property);\r}\rpublic static void Set$2(DependencyObject d, $1 value)\r{\rd.SetValue($2Property, value);\r}
          Сгенерированные DependencyProperty:
            public Brush ImageFill { get; set; }
            public Thickness StrokeThickness { get; set; }
            public Brush ImageStrokeBrush { get; set; }
        */


        public static readonly DependencyProperty ImageFillProperty = DependencyProperty.RegisterAttached("ImageFill", typeof(Brush), typeof(DependencyValue));
        public static Brush GetImageFill(DependencyObject d)
        {
            return (Brush)d.GetValue(ImageFillProperty);
        }
        public static void SetImageFill(DependencyObject d, Brush value)
        {
            d.SetValue(ImageFillProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.RegisterAttached("StrokeThickness", typeof(double), typeof(DependencyValue));
        public static double GetStrokeThickness(DependencyObject d)
        {
            return (double)d.GetValue(StrokeThicknessProperty);
        }
        public static void SetStrokeThickness(DependencyObject d, double value)
        {
            d.SetValue(StrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty ImageStrokeBrushProperty = DependencyProperty.RegisterAttached("ImageStrokeBrush", typeof(Brush), typeof(DependencyValue));
        public static Brush GetImageStrokeBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(ImageStrokeBrushProperty);
        }
        public static void SetImageStrokeBrush(DependencyObject d, Brush value)
        {
            d.SetValue(ImageStrokeBrushProperty, value);
        }

        public static readonly DependencyProperty ForegroundBrushProperty = DependencyProperty.RegisterAttached("ForegroundBrush", typeof(Brush), typeof(DependencyValue));
        public static Brush GetForegroundBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(ForegroundBrushProperty);
        }
        public static void SetForegroundBrush(DependencyObject d, Brush value)
        {
            d.SetValue(ForegroundBrushProperty, value);
        }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width", typeof(double), typeof(DependencyValue));
        public static double GetWidth(DependencyObject d)
        {
            return (double)d.GetValue(WidthProperty);
        }
        public static void SetWidth(DependencyObject d, double value)
        {
            d.SetValue(WidthProperty, value);
        }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached("Height", typeof(double), typeof(DependencyValue));
        public static double GetHeight(DependencyObject d)
        {
            return (double)d.GetValue(HeightProperty);
        }
        public static void SetHeight(DependencyObject d, double value)
        {
            d.SetValue(HeightProperty, value);
        }

        #region ObjectValue

        public static readonly DependencyProperty ObjectValueProperty = DependencyProperty
                .RegisterAttached("ObjectValue", typeof(object), typeof(DependencyValue));

        public static object GetObjectValue(DependencyObject dependencyObject)
        {
            return (object)dependencyObject.GetValue(ObjectValueProperty);
        }

        public static void SetObjectValue(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(ObjectValueProperty, value);
        }

        #endregion ObjectValue
    }
}
