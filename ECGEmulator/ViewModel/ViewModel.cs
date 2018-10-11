using ECGEmulator.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Input;

namespace ECGEmulator.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region ICommand
        private ICommand cmdTryConnect;
        public ICommand CmdTryConnect
        {
            get
            {
                if (cmdTryConnect == null)
                {
                    cmdTryConnect = new RelayCommand(
                        param => TryConnect()
                        );
                }
                return cmdTryConnect;
            }
        }

        private ICommand cmdSendMessage;
        public ICommand CmdSendMessage
        {
            get
            {
                if (cmdSendMessage == null)
                {
                    cmdSendMessage = new RelayCommand(
                        param => Send(RawData)
                        );
                }
                return cmdSendMessage;
            }
        }
        #endregion

        #region Properties
        public ObservableCollection<string> PortList { get; set; }
        public ObservableCollection<int> BaudRate { get; set; }
        public ObservableCollection<int> DataBits { get; set; }
        public ObservableCollection<Parity> Parity { get; set; }
        public ObservableCollection<StopBits> StopBits { get; set; }

        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                NotifyPropertyChanged(nameof(IsConnected));
            }
        }

        public string RawData { get; set; }

        private StringBuilder dataLogger = new StringBuilder();
        public string DataLogger
        {
            get { return Convert.ToString(dataLogger); }
        }

        private SelectedInfo selected;
        public SelectedInfo Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                NotifyPropertyChanged(nameof(Selected));
            }
        }
        #endregion

        private ViewModelWorker Worker;
        public ViewModel()
        {
            Worker = new ViewModelWorker();
            InitSerial();
        }

        private void InitSerial()
        {
            PortList = new ObservableCollection<string>(SerialPort.GetPortNames());
            BaudRate = new ObservableCollection<int>
            { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200,
                38400, 56000, 57600, 115200, 128000, 256000 };
            DataBits = new ObservableCollection<int> { 5, 6, 7, 8 };
            Parity = new ObservableCollection<Parity> { System.IO.Ports.Parity.None,
                System.IO.Ports.Parity.Even, System.IO.Ports.Parity.Odd,
                System.IO.Ports.Parity.Mark, System.IO.Ports.Parity.Space };
            StopBits = new ObservableCollection<StopBits> { //System.IO.Ports.StopBits.None,
            System.IO.Ports.StopBits.One, System.IO.Ports.StopBits.OnePointFive, System.IO.Ports.StopBits.Two};

            SelectedInfo.Instance.Port = PortList.FirstOrDefault();
            SelectedInfo.Instance.Baudrate = BaudRate.FirstOrDefault();
            SelectedInfo.Instance.DataBits = DataBits.FirstOrDefault();
            SelectedInfo.Instance.Parity = Parity.FirstOrDefault();
            SelectedInfo.Instance.StopBits = StopBits.FirstOrDefault();

#if Debug
            RunDemo();
#endif
        }

        private void TryConnect()
        {
            if (Worker.OpenSerialPort())
            {
                IsConnected = true;
            }
            else
            {
                Worker.CloseSerialPort();
                IsConnected = false;
            }
        }

        private void Send(string data)
        {
            Worker.Send(data);
            dataLogger.AppendLine(data);
            NotifyPropertyChanged(nameof(DataLogger));
        }

        Timer DemoTimer;
        public void RunDemo()
        {
            DemoTimer = new Timer();
            DemoTimer.Interval = 20;
            DemoTimer.Elapsed += new ElapsedEventHandler(OnTime);
            DemoTimer.Start();
        }

        Random Rand = new Random();
        private void OnTime(object sender, ElapsedEventArgs e)
        {   
            Send(Convert.ToString(Rand.Next()%255));
        }
    }

    public class SelectedInfo : INotifyPropertyChanged
    {
#region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
#endregion

        private static SelectedInfo instance = new SelectedInfo();

        public string Port { get; set; }
        public int Baudrate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }

        private SelectedInfo()
        {
        }

        public static SelectedInfo Instance
        {
            get
            {
                if (instance == null)
                    instance = new SelectedInfo();

                return instance;
            }
        }
    }

}
