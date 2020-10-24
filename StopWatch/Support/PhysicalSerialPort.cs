using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;



namespace StopWatch.Support
{
    // DTR выдает сигнал высокоро уровня 
    // и замыкается с выводами DSR или  CTS
    // далее можно считывать текущее состояниие выводов
    

    class ButtonSerialPort
    {
        private SerialPort                      _port;

        private static bool _RideO = true;
        private static bool _RideT = true;

        public static bool RideO { get => _RideO; set => _RideO = value; }
        public static bool RideT { get => _RideT; set => _RideT = value; }


        public ButtonSerialPort(
            int portNumber, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One,
            Handshake handShake = Handshake.None, int readTimeout = -1, int writeTimeout = -1) :
            this("COM" + portNumber.ToString(), baudRate, parity, dataBits, stopBits, handShake, readTimeout, writeTimeout)
        {
            _port.PinChanged    += PinChanged;
            
        }


        public ButtonSerialPort(
            string portName, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One,
            Handshake handShake = Handshake.None, int readTimeout = -1, int writeTimeout = -1)
        {
            _port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            _port.Handshake     = handShake;
            _port.ReadTimeout   = readTimeout;
            _port.WriteTimeout  = writeTimeout;

            
            _port.PinChanged += PinChanged;
        }


        public void Open()
        {
            _port.Open();
        }
        public void Close()
        {
            _port.Close();
        }

        public void DtrEnable()
        {
            _port.DtrEnable = true;
        }
        public void DtrDisable()
        {
            _port.DtrEnable = false;
        }

        public bool getDsrState()
        {
            return _port.DsrHolding;
        }

        public bool getCtsState()
        {
            return _port.CtsHolding;
        }


        internal void PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            SerialPinChange serialPinChangeUser;
            serialPinChangeUser = e.EventType;

            switch(serialPinChangeUser)
            {
                case SerialPinChange.CtsChanged:
                    if (Properties.Settings.Default.RideTwo == "CTS")
                    {
                        _RideT = false;
                        ViewModel.StopWatchVM.FirstCome = "Two";
                    }
                    else
                    {
                        _RideO = false;
                        ViewModel.StopWatchVM.FirstCome = "One";
                    }
                    break;
                case SerialPinChange.DsrChanged:
                    if (Properties.Settings.Default.RideOne == "DSR")
                    {
                        _RideO = false;
                        ViewModel.StopWatchVM.FirstCome = "One";
                    }
                    else
                    {
                        _RideT = false;
                        ViewModel.StopWatchVM.FirstCome = "Two";
                    }
                    break;
            }
        }
        
    }
}