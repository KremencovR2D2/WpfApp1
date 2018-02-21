using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    public class PlayerViewModel : ObservableObject
    {
        public MediaPlayer MyMediaElement { get; set; } = new MediaPlayer();

        private double _maximumLength;

        public double MaximumLength
        {
            get { return _maximumLength; }
            set
            {
                if (_maximumLength != value)
                {
                    OnPropertyChanged("MaximumLength");
                }
            }
        }

        private double _position = 0;

        public double Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    OnPropertyChanged("Position");
                }
            }
        }

        private int _volume = 100;

        public int Volume
        {
            get { return _volume;}
            set
            {
                if (_volume != value)
                {
                    if (value > 0)
                    {
                        IsVolumeOff = false;
                    }

                    if (value == 0)
                    {
                        IsVolumeOff = true;
                    }
                    _volume = value;
                    MyMediaElement.Volume = (double)_volume/100;
                    OnPropertyChanged("Volume");
                }
            }
        }

        private bool _isVolumeOff;

        public bool IsVolumeOff
        {
            get { return _isVolumeOff; }
            set
            {
                if (_isVolumeOff != value)
                {
                    _isVolumeOff = value;

                    Volume = _isVolumeOff ? 0 : 100;
                    OnPropertyChanged("IsVolumeOff");
                }
            }
        }

        private bool _isPlay;

        public bool IsPlay
        {
            get { return _isPlay; }
            set
            {
                if (_isPlay != value)
                {
                    _isPlay = value;
                    ChangePlaymedia(value);
                    OnPropertyChanged("IsPlay");
                }
            }
        }

        public PlayerViewModel()
        {
            MyMediaElement.Volume = 0.5;
            MyMediaElement.Open(new Uri(@"C:\Users\d.krementsov\Downloads\Telegram Desktop\Jon Everist - Matrix.mp3", UriKind.Relative));
            MyMediaElement.Stop();
            MyMediaElement.MediaOpened += MyMediaElementOnMediaOpened;
            MyMediaElement.Volume = Volume;
        }

        private void MyMediaElementOnMediaOpened(object sender, EventArgs eventArgs)
        {
            MaximumLength = MyMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            Position = MyMediaElement.Position.TotalMilliseconds;
        }

        private void ChangePlaymedia(bool value)
        {
            if (value)
            {
                MyMediaElement.Play();
            }
            else
            {
                MyMediaElement.Pause();
            }
        }
    }

    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Дергаем событие по изменению свойства
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
