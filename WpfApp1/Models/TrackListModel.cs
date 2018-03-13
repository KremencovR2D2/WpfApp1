namespace WpfApp1.Models
{
    public class TrackListModel: ObservableObject
    {
        private bool _isPaused;

        public bool IsPaused
        {
            get { return _isPaused; }

            set
            {
                if (_isPaused != value)
                {
                    _isPaused = value;
                    OnPropertyChanged("IsPaused");
                }
            }
        }

        private bool _isPlaying;

        public bool IsPlaying
        {
            get { return _isPlaying;}

            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnPropertyChanged("IsPlaying");
                }
            }
        }

        private string _path;

        public string Path
        {
            get { return _path; }

            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged("Path");
                }
            }
        }

        private string _artist;

        public string Artist
        {
            get { return _artist; }

            set
            {
                if (_artist != value)
                {
                    _artist = value;
                    OnPropertyChanged("Artist");
                }
            }
        }

        private string _album;

        public string Album
        {
            get { return _album; }

            set
            {
                if (_album != value)
                {
                    _album = value;
                    OnPropertyChanged("Album");
                }
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }

            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }

            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }

        private string _year;

        public string Year
        {
            get { return _year; }

            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged("Year");
                }
            }
        }

        private string _duration;

        public string Duration
        {
            get { return _duration; }

            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }
    }
}
