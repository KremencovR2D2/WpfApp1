using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using TagLib;

namespace WpfApp1.Models
{
    public class PlayerViewModel : ObservableObject
    {
        public MediaPlayer MyMediaElement { get; set; } = new MediaPlayer();

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private int _trackIndex;

        private string _artist;

        public string Artist
        {
            get { return _artist; }
            set
            {
                if (_artist != value)
                {
                    if (string.IsNullOrEmpty(value) || value == " ")
                        _artist = "Unknown Artist";
                    else
                    {
                        _artist = value;
                    }

                    OnPropertyChanged("Artist");
                }
            }
        }

        private string _song;

        public string Song
        {
            get { return _song; }
            set
            {
                if (_song != value)
                {
                    if (string.IsNullOrEmpty(value) || value == " ")
                        _song = "Unknown Song";
                    else
                    {
                        _song = value;
                    }

                    OnPropertyChanged("Song");
                }
            }
        }

        private TrackListModel _currentTrack;

        public TrackListModel CurrentTrack
        {
            get { return _currentTrack; }
            set
            {
                if (_currentTrack != value)
                {
                    _currentTrack = value;
                    OnPropertyChanged("CurrentTrack");
                }
            }
        }

        private TrackListModel _selectedTrack;

        public TrackListModel SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                if (_selectedTrack != value)
                {
                    _selectedTrack = value;
                    OnPropertyChanged("SelectedTrack");
                }
            }
        }

        private TrackListModel _previousTrack;

        public TrackListModel PreviousTrack
        {
            get { return _previousTrack; }
            set
            {
                if (_previousTrack != value)
                {
                    _previousTrack = value;
                    OnPropertyChanged("PreviousTrack");
                }
            }
        }

        private ObservableCollection<TrackListModel> _trackModelList;

        public ObservableCollection<TrackListModel> TrackModelList
        {
            get { return _trackModelList; }
            set
            {
                if (_trackModelList != value)
                {
                    _trackModelList = value;
                    OnPropertyChanged("TrackModelList");
                }
            }
        }

        private Tag _currentTag;

        public Tag CurrentTag
        {
            get { return _currentTag; }
            set
            {
                if (_currentTag != value)
                {
                    _currentTag = value;
                    Artist = _currentTag.Performers.FirstOrDefault();
                    Song = _currentTag.Title;
                    OnPropertyChanged("CurrentTag");
                }
            }
        }

        private string _startTime = "0:00";

        public string StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }

        private string _endTime = "0:00";

        public string EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnPropertyChanged("EndTime");
                }
            }
        }

        private double _maximumLength;

        public double MaximumLength
        {
            get { return _maximumLength; }
            set
            {
                if (_maximumLength != value)
                {
                    _maximumLength = value;
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
                    _position = value;
                    OnPropertyChanged("Position");
                }
            }
        }

        private int _volume = 100;

        public int Volume
        {
            get { return _volume; }
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
                    MyMediaElement.Volume = (double) _volume / 100;
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
                    ChangePlayState(value);
                    OnPropertyChanged("IsPlay");
                }
            }
        }


        private bool _isShuffleEnable;

        public bool IsShuffleEnable
        {
            get { return _isShuffleEnable; }
            set
            {
                if (_isShuffleEnable != value)
                {
                    _isShuffleEnable = value;
                    OnPropertyChanged("IsShuffleEnable");
                }
            }
        }

        private bool _isRepeatEnable;

        public bool IsRepeatEnable
        {
            get { return _isRepeatEnable; }
            set
            {
                if (_isRepeatEnable != value)
                {
                    _isRepeatEnable = value;
                    OnPropertyChanged("IsRepeatEnable");
                }
            }
        }

        public PlayerViewModel()
        {
            TrackModelList = new ObservableCollection<TrackListModel>();
            InitializeTrackList();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += TimerPosition;
            CurrentTrack = TrackModelList.First();
            SelectedTrack = CurrentTrack;
            _trackIndex = 0;
            MyMediaElement.Open(new Uri(CurrentTrack.Path, UriKind.Relative));
            MyMediaElement.Stop();
            MyMediaElement.MediaOpened += MyMediaElementOnMediaOpened;
            MyMediaElement.MediaEnded += MyMediaElementOnMediaEnded;
            MyMediaElement.Volume = Volume;
        }

        private void MyMediaElementOnMediaEnded(object sender, EventArgs eventArgs)
        {
            if (IsRepeatEnable)
            {
                MyMediaElement.Stop();
                _timer.Stop();
                Position = 0;
                Thread.Sleep(100);
                StartTime = "0:00";
                IsPlay = true;
                ChangePlayState(true);
            }
            else
            {
                MyMediaElement.Stop();
                _timer.Stop();
                Thread.Sleep(100);
                Position = 0;
                StartTime = "0:00";
                if (OnCanExecuteNextTrackCommand(null))
                {
                    ChangeTrack(true);
                }
                else
                {
                    IsPlay = false;
                }
            }
        }

        private void MyMediaElementOnMediaOpenedPreview(object sender, EventArgs eventArgs)
        {
            var duration =
                $"{MyMediaElement.NaturalDuration.TimeSpan.Minutes}:{GetCorrectSeconds(MyMediaElement.NaturalDuration.TimeSpan.Seconds)}";
            var path = MyMediaElement.Source.OriginalString;
            if (path == TrackModelList.Last().Path)
            {
                MyMediaElement.MediaOpened -= MyMediaElementOnMediaOpenedPreview;
                MyMediaElement.Open(new Uri(CurrentTrack.Path, UriKind.Relative));
                MyMediaElement.Stop();
                MyMediaElement.MediaOpened += MyMediaElementOnMediaOpened;
                MyMediaElement.MediaEnded += MyMediaElementOnMediaEnded;
                MyMediaElement.Volume = Volume;
            }

            TrackModelList.First(x => x.Path == path).Duration = duration;
        }

        private void MyMediaElementOnMediaOpened(object sender, EventArgs eventArgs)
        {
            MaximumLength = MyMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            EndTime =
                $"{MyMediaElement.NaturalDuration.TimeSpan.Minutes}:{GetCorrectSeconds(MyMediaElement.NaturalDuration.TimeSpan.Seconds)}";
            if (IsPlay)
            {
                ChangePlayState(true);
                Position = 0;
                StartTime = "0:00";
            }
        }

        private void TimerPosition(object sender, EventArgs e)
        {
            double pos = 0;
            MyMediaElement.Dispatcher.Invoke(() => pos = MyMediaElement.Position.TotalMilliseconds);
            var timeSpan = new TimeSpan(0, 0, 0, 0, (int) pos);
            StartTime = $"{timeSpan.Minutes}:{GetCorrectSeconds(timeSpan.Seconds)}";
            Position = pos;
        }

        private void InitializeTrackList()
        {
            var path = ConfigurationManager.AppSettings["Path"];
            var directoryInfo = new DirectoryInfo(Path.GetFullPath(path));
            var files = directoryInfo.GetFiles("*.mp3");
            var trackList = new List<TrackListModel>();
            foreach (var file in files)
            {
                //Сделать отдельную модель
                var tagFile = TagLib.File.Create(file.FullName);
                trackList.Add(new TrackListModel
                {
                    FileName = file.Name.Substring(0, file.Name.Length - 4),
                    Artist = !string.IsNullOrEmpty(tagFile.Tag.Performers.FirstOrDefault())
                        ? tagFile.Tag.Performers.FirstOrDefault()
                        : "Unknown Artist",
                    Album = !string.IsNullOrEmpty(tagFile.Tag.Album) ? tagFile.Tag.Album : "Unknown Album",
                    Title = !string.IsNullOrEmpty(tagFile.Tag.Title) ? tagFile.Tag.Title : "Unknown Title",
                    Year = tagFile.Tag.Year != 0 ? tagFile.Tag.Year.ToString() : "-",
                    Path = file.FullName,
                    IsPlaying = false,
                });
            }

            TrackModelList = new ObservableCollection<TrackListModel>(trackList);
        }

        private void ChangePlayState(bool value)
        {
            if (value)
            {
                Artist = CurrentTrack.Artist;
                Song = CurrentTrack.Title;
                SelectedTrack.IsPlaying = true;
                MyMediaElement.Play();
                _timer.Start();
                foreach (var track in TrackModelList)
                {
                    track.IsPlaying = track == CurrentTrack;
                    track.IsPaused = false;
                }
            }
            else
            {
                MyMediaElement.Pause();
                _timer.Stop();
                foreach (var track in TrackModelList)
                {
                    track.IsPlaying = false;
                    if (track == CurrentTrack)
                    {
                        track.IsPaused = true;
                    }
                }
            }
        }

        private string GetCorrectSeconds(int seconds)
        {
            var second = seconds < 10 ? $"0{seconds}" : $"{seconds}";
            return second;
        }

        private RelayCommand _changePositionCommand;

        public RelayCommand ChangePositionCommand
        {
            get
            {
                return _changePositionCommand ??
                       (_changePositionCommand = new RelayCommand(obj =>
                       {
                           var timeSpan = new TimeSpan(0, 0, 0, 0, (int) Position);
                           MyMediaElement.Position = timeSpan;
                           StartTime = $"{timeSpan.Minutes}:{GetCorrectSeconds(timeSpan.Seconds)}";
                           _timer.Start();
                       }));
            }
        }

        private RelayCommand _stopTimerCommand;

        public RelayCommand StopTimerCommand
        {
            get
            {
                return _stopTimerCommand ??
                       (_stopTimerCommand = new RelayCommand(obj => { _timer.Stop(); }));
            }
        }

        private RelayCommand _previousTrackCommand;

        public RelayCommand PreviousTrackCommand
        {
            get
            {
                return _previousTrackCommand ??
                       (_previousTrackCommand = new RelayCommand(obj => { ChangeTrack(false); },
                           OnCanExecutePreviousTrackCommand));
            }
        }

        private RelayCommand _nextTrackCommand;

        public RelayCommand NextTrackCommand
        {
            get
            {
                return _nextTrackCommand ??
                       (_nextTrackCommand = new RelayCommand(obj => { ChangeTrack(true); },
                           OnCanExecuteNextTrackCommand));
            }
        }

        private RelayCommand _doubleClickCommand;

        public RelayCommand DoubleClickCommand
        {
            get
            {
                return _doubleClickCommand ??
                       (_doubleClickCommand = new RelayCommand(PlayTrack));
            }
        }

        private void PlayTrack(object o)
        {
            var track = o as TrackListModel;
            CurrentTrack = track;
            _trackIndex = TrackModelList.IndexOf(track);
            MyMediaElement.Open(new Uri(CurrentTrack.Path, UriKind.Relative));
            MyMediaElement.Stop();
            Position = 0;
            Thread.Sleep(100);
            StartTime = "0:00";
            IsPlay = true;
            ChangePlayState(true);
        }

        private void ChangeTrack(bool next)
        {
            if (!IsShuffleEnable)
            {
                if (next)
                {
                    _trackIndex = _trackIndex + 1;
                    CurrentTrack = TrackModelList[_trackIndex];
                    SelectedTrack = CurrentTrack;
                }
                else
                {
                    _trackIndex = _trackIndex - 1;
                    CurrentTrack = TrackModelList[_trackIndex];
                    SelectedTrack = CurrentTrack;
                }
            }
            else
            {
                if (next)
                {
                    var random = new Random();
                    var randomDigit = random.Next(0, TrackModelList.Count - 1);
                    _trackIndex = randomDigit;
                    PreviousTrack = CurrentTrack;
                    CurrentTrack = TrackModelList[_trackIndex];
                }
                else
                {
                    var random = new Random();
                    var randomDigit = random.Next(0, TrackModelList.Count - 1);
                    if (PreviousTrack == null)
                    {
                        _trackIndex = randomDigit;
                        CurrentTrack = TrackModelList[_trackIndex];
                        PreviousTrack = TrackModelList[random.Next(0, TrackModelList.Count - 1)];
                    }
                    else
                    {
                        CurrentTrack = PreviousTrack;
                        _trackIndex = randomDigit;
                        PreviousTrack = TrackModelList[_trackIndex];
                    }
                }
            }
            MyMediaElement.Stop();
            _timer.Stop();
            MyMediaElement.Open(new Uri(CurrentTrack.Path, UriKind.Relative));
            Position = 0;
            Thread.Sleep(100);
            StartTime = "0:00";
            IsPlay = true;
            ChangePlayState(true);
        }

        private bool OnCanExecuteNextTrackCommand(object obj)
        {
            return _trackIndex + 1 <= TrackModelList.Count - 1 || IsShuffleEnable;
        }

        private bool OnCanExecutePreviousTrackCommand(object obj)
        {
            return _trackIndex - 1 >= 0 || IsShuffleEnable;
        }
    }
}
