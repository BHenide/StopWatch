using System;
using System.Diagnostics;
using System.Threading;
using StopWatch.Support;

namespace StopWatch.ViewModel
{
    class StopWatchVM : Notification
    {      
        public static string FirstCome = string.Empty;

        private ButtonSerialPort _BSP;
        private Stopwatch _TimeRun;
        private Thread _Flow;
        private TimeSpan _TimeS;
        private string _Time;
        private string _Name_Operation;
        private string _Member_One;
        private string _Member_One_Time;
        private string _Member_Two;
        private string _Member_Two_Time;
        private bool _Stop;
        private bool _IsTimerRun;

        public bool IsTimerRun
        {
            get { return _IsTimerRun; }
            set
            {
                _IsTimerRun = value;
                OnPropertyChanged("IsTimerRun");
            }
        }

        public string Time
        {
            get
            {
                return _Time;
            }
            set
            {
                _Time = value;
                OnPropertyChanged("Time");
            }
        }
        public string Name_Operation
        {
            get { return _Name_Operation; }
            set
            {
                _Name_Operation = value;
                OnPropertyChanged("Name_Operation");
            }
        }      
        public string Member_One
        {
            get { return _Member_One; }
            set
            {
                _Member_One = value;
                OnPropertyChanged("Member_One");
            }
        }
        public string Member_One_Time
        {
            get { return _Member_One_Time; }
            set
            {
                _Member_One_Time = value;
                OnPropertyChanged("Member_One_Time");
            }
        }

        public string Member_Two
        {
            get { return _Member_Two; }
            set
            {
                _Member_Two = value;
                OnPropertyChanged("Member_Two");
            }
        }
        public string Member_Two_Time
        {
            get { return _Member_Two_Time; }
            set
            {
                _Member_Two_Time = value;
                OnPropertyChanged("Member_Two_Time");
            }
        }


        #region For Button Click Stop Timer

        private bool _GoTimeRun;

        public bool GoTimeRun
        {
            get { return _GoTimeRun; }
        }
        #endregion

        #region Work Timer              

        private RelayCommand _WorkTimer;
        public RelayCommand WorkTimer
        {
            get
            {
                return _WorkTimer ??
                  (_WorkTimer = new RelayCommand(obj =>
                  {
                      WorkTimerRun();
                  }));
            }
        }

        private void WorkTimerRun()
        {
            if (Name_Operation == "Старт")
            {
                if (Properties.Settings.Default.ComPort)
                {
                    if (_BSP == null)
                    {
                        int Success = ConnectionCOM();
                        if (Success == -1)
                        {
                            System.Windows.MessageBoxResult ConnectPort = System.Windows.MessageBox.Show("Соединение по COM-порту не удалось хотите продолжить?",
                            "Предупреждение", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                            if (ConnectPort == System.Windows.MessageBoxResult.No) { return; }
                        }
                    }
                }

                System.Windows.MessageBoxResult result = System.Windows.MessageBoxResult.Yes;
                if (_Member_One == string.Empty || _Member_Two == string.Empty)
                {
                    result = System.Windows.MessageBox.Show("Не все дорожки заполнены, вы уверены что хотите запустить секундомер?",
                        "Предупреждение", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                }
                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    _Flow = new Thread(GoStart);
                    _Flow.Start();
                    Name_Operation = "Стоп";
                    IsTimerRun = true;
                    Logger.Log.Info("Нажата кнопка Старт");
                }
            }
            else
            {
                if (Name_Operation == "Стоп")
                {
                    StopT();

                    if (Member_One != string.Empty && Member_Two != string.Empty &&
                    Member_One_Time != string.Empty && Member_Two_Time != string.Empty)
                    {
                        Model.Result New_Race = new Model.Result()
                        {
                            Name_One = Member_One,
                            Time_One = Member_One_Time,
                            Name_Two = Member_Two,
                            Time_Two = Member_Two_Time
                        };
                        using (Model.ParticipantContainer pc = new Model.ParticipantContainer())
                        {
                            pc.Entry(New_Race).State = System.Data.Entity.EntityState.Added;
                            pc.SaveChanges();
                        }
                    }

                    if (FirstCome != string.Empty) { Time = (FirstCome == "One") ? Member_One_Time : Member_Two_Time; }

                    IsTimerRun = false;
                    Name_Operation = "Сброс";
                    Logger.Log.Info("Нажата кнопка Стоп");
                }
                else
                {
                    _TimeRun.Reset();
                    _TimeS = _TimeRun.Elapsed;
                    Time = $"{_TimeS.Minutes.ToString("D2")}:{_TimeS.Seconds.ToString("D2")}:{_TimeS.Milliseconds.ToString("D2")}";
                    Name_Operation = "Старт";
                    Logger.Log.Info("Нажата кнопка Сброс");

                    Time = string.Empty;
                    Member_One_Time = string.Empty;
                    Member_Two_Time = string.Empty;
                    FirstCome = string.Empty;

                    ButtonSerialPort.RideO = true;
                    ButtonSerialPort.RideT = true;
                }
            }
        }
        #endregion                

        #region RunHistoryMember

        HistoryMemberVM Hist;

        private RelayCommand _RunHistoryMember;
        public RelayCommand RunHistoryMember
        {
            get
            {
                return _RunHistoryMember ??
                  (_RunHistoryMember = new RelayCommand(obj =>
                  {
                      Logger.Log.Info("Запуск истории участников");
                      Hist.Update();
                      View.HistoryMember Hm = new View.HistoryMember() { DataContext = Hist };
                      Hm.ShowDialog();
                  }));
            }
        }
        #endregion

        #region RunSetting

        private RelayCommand _RunSetting;
        public RelayCommand RunSetting
        {
            get
            {
                return _RunSetting ??
                  (_RunSetting = new RelayCommand(obj =>
                  {
                      Logger.Log.Info("Запуск настройки");
                      if (_BSP != null)
                      {
                          _BSP.DtrDisable();
                          _BSP.Close();
                          _BSP = null;
                      }
                      View.Setting St = new View.Setting() { DataContext = new SettingVM() };
                      St.ShowDialog();
                      if (Properties.Settings.Default.ComPort)
                      {
                          ConnectionCOM();
                      }
                  }));
            }
        }
        #endregion

        #region RunAbout

        private RelayCommand _RunAbout;
        public RelayCommand RunAbout
        {
            get
            {
                return _RunAbout ??
                  (_RunAbout = new RelayCommand(obj =>
                  {
                      Logger.Log.Info("Запуск о программе");
                      View.About Ab = new View.About();
                      Ab.ShowDialog();
                  }));
            }
        }
        #endregion

        #region Close

        public void Close()
        {
            StopT();
        }
        #endregion

        private void StopT()
        {
            if (_Stop)
            {

                _TimeRun.Stop();
                _Flow.Abort();
            }
        }

        public StopWatchVM()
        {
            Logger.InitLogger();
            Logger.Log.Info("Запуск программы");            
            Hist = new HistoryMemberVM();
            _TimeRun = new Stopwatch();
            _Flow = new Thread(GoTime);
            _Stop = true;           
            Name_Operation = "Старт";
            _Member_One = string.Empty;
            _Member_Two = string.Empty;
            _Member_One_Time = string.Empty;
            _Member_Two_Time = string.Empty;
            _GoTimeRun = false;
        }

        private void GoTime()
        {
            Logger.Log.Info("Запуск секундомера");
            _GoTimeRun = true;
            _TimeRun.Start();
            while (_Stop)
            {
                _TimeS = _TimeRun.Elapsed;
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Time = $"{_TimeS.Minutes.ToString("D2")}:{_TimeS.Seconds.ToString("D2")}:{_TimeS.Milliseconds.ToString("D3")}";
                    if (ButtonSerialPort.RideO)
                    {
                        Member_One_Time = Time;                        
                    }
                    else if(!ButtonSerialPort.RideT)
                    {
                        WorkTimerRun();
                    }

                    if (ButtonSerialPort.RideT)
                    {
                        Member_Two_Time = Time;                        
                    }
                });
                Thread.Sleep(20);
            }
        }        

        private void GoStart()
        {
            int St = Properties.Settings.Default.From;
            Logger.Log.Info("Запуск таймера отсчета");
            _GoTimeRun = false;
            while (St >= 0 && _Stop)
            {                
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Time = St.ToString("D2");
                    if (St <= 2 && Properties.Settings.Default.Sound)
                    {
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer("Sound//1.wav");
                        switch (St)
                        {
                            case 2:
                                player.Play();
                                break;
                            case 1:
                                player = new System.Media.SoundPlayer("Sound//2.wav");
                                player.Play();
                                break;
                            case 0:
                                player = new System.Media.SoundPlayer("Sound//3.wav");
                                player.Play();
                                player.Dispose();
                                break;
                        }
                    }
                    
                });
                Thread.Sleep(1000);
                St--;
            }
            GoTime();
        }

        private int ConnectionCOM()
        {
            Logger.Log.Info("Проверка соединения");
            try
            {
                _BSP = new ButtonSerialPort(Properties.Settings.Default.Port, Properties.Settings.Default.Speed);
                _BSP.Open();
                _BSP.DtrEnable();
                return 1;
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Порт отсутсвует!", "Ошибка");
                _BSP.DtrDisable();
                _BSP.Close();
                _BSP = null;
                return -1;
            }
        }
    }
}
