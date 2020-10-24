using StopWatch.Support;
using System.Collections.ObjectModel;

namespace StopWatch.ViewModel
{
    class SettingVM : Notification
    {       
        public int From
        {
            get { return Properties.Settings.Default.From; }
            set
            {
                Properties.Settings.Default.From = value;
                OnPropertyChanged("From");
                SaveChange();
            }
        }
        public bool Sound
        {
            get { return Properties.Settings.Default.Sound; }
            set
            {
                Properties.Settings.Default.Sound = value;
                OnPropertyChanged("Sound");
                SaveChange();
            }
        }
        public bool Emulation
        {
            get { return Properties.Settings.Default.Emulation; }
            set
            {
                Properties.Settings.Default.Emulation = value;
                OnPropertyChanged("Emulation");
                SaveChange();
            }
        }

        public bool ComPort
        {
            get { return Properties.Settings.Default.ComPort; }
            set
            {
                Properties.Settings.Default.ComPort = value;
                OnPropertyChanged("ComPort");
                SaveChange();
            }
        }

        public int Port
        {
            get { return Properties.Settings.Default.Port; }
            set
            {
                Properties.Settings.Default.Port = value;
                OnPropertyChanged("Port");
                SaveChange();
            }
        }
        public int Speed
        {
            get { return Properties.Settings.Default.Speed; }
            set
            {
                Properties.Settings.Default.Speed = value;
                OnPropertyChanged("Speed");
                SaveChange();
            }
        }

        public string RideOne
        {
            get { return Properties.Settings.Default.RideOne; }
            set
            {
                Properties.Settings.Default.RideTwo = RideOne;
                OnPropertyChanged("RideTwo");
                Properties.Settings.Default.RideOne = value;
                OnPropertyChanged("RideOne");
                SaveChange();
            }
        }
        public string RideTwo
        {
            get { return Properties.Settings.Default.RideTwo; }
            set
            {
                Properties.Settings.Default.RideOne = RideTwo;
                OnPropertyChanged("RideOne");
                Properties.Settings.Default.RideTwo = value;
                OnPropertyChanged("RideTwo");
                SaveChange();
            }
        }

        public string ButtonCTS
        {
            get { return Properties.Settings.Default.ButtonCTS; }
            set
            {
                Properties.Settings.Default.ButtonCTS = value;
                OnPropertyChanged("ButtonCTS");
            }
        }
        public string ButtonDSR
        {
            get { return Properties.Settings.Default.ButtonDSR; }
            set
            {
                Properties.Settings.Default.ButtonDSR = value;
                OnPropertyChanged("ButtonDSR");
            }
        }

        #region Appoint

        private RelayCommand _Appoint;
        public RelayCommand Appoint
        {
            get
            {
                return _Appoint ??
                  (_Appoint = new RelayCommand(obj =>
                  {
                      string ButtonName = obj.ToString();
                      View.CashButton CB;
                      if (ButtonName == "CTS")
                      {
                          CB = new View.CashButton(true);
                      }
                      else
                      {
                          CB = new View.CashButton(false);
                      }
                      CB.ShowDialog();

                      if (ButtonName == "CTS")
                      {
                          ButtonCTS = Properties.Settings.Default.ButtonCTS;
                      }
                      else
                      {
                          ButtonDSR = Properties.Settings.Default.ButtonDSR;
                      }

                      SaveChange();
                  }));
            }
        }

        #endregion


        public ObservableCollection<string> Rides { get; set; }
        private void SaveChange()
        {
            Properties.Settings.Default.Save();
        }        

        public SettingVM()
        {
            Rides = new ObservableCollection<string>();
            Rides.Add(RideOne);
            Rides.Add(RideTwo);
        }
    }    
}
