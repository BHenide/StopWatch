using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;

namespace StopWatch.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel.StopWatchVM();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Support.Logger.Log.Info("Выход из программы");
            ((ViewModel.StopWatchVM)DataContext).Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Properties.Settings.Default.Emulation)
            {
                if (((ViewModel.StopWatchVM)DataContext).GoTimeRun)
                {
                    string NewName = Support.ReformerNameButton.Reform(e);
                    if (NewName == Properties.Settings.Default.ButtonCTS)
                    {
                        if (Properties.Settings.Default.RideOne == "CTS")
                        {
                            Support.ButtonSerialPort.RideO = false;
                            ViewModel.StopWatchVM.FirstCome = "One";
                        }
                        else
                        {
                            Support.ButtonSerialPort.RideT = false;
                            ViewModel.StopWatchVM.FirstCome = "Two";
                        }
                    }
                    else if (NewName == Properties.Settings.Default.ButtonDSR)
                    {
                        if (Properties.Settings.Default.RideOne == "DSR")
                        {
                            Support.ButtonSerialPort.RideO = false;
                            ViewModel.StopWatchVM.FirstCome = "One";
                        }
                        else
                        {
                            Support.ButtonSerialPort.RideT = false;
                            ViewModel.StopWatchVM.FirstCome = "Two";
                        }
                    }
                }
            }
        }
    }
}
