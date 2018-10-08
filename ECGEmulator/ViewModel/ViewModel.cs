using ECGEmulator.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        param => Worker.TryConnect(Selected.Port)
                        );
                }
                return cmdTryConnect;
            }
        }
        #endregion

        #region Properties
        public ObservableCollection<string> PortList { get; set; }

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

        ViewModelWorker Worker;
        public ViewModel()
        {
            Worker = new ViewModelWorker();
            InitSerial();
        }

        private void InitSerial()
        {
            PortList = new ObservableCollection<string>(SerialPort.GetPortNames());
            NotifyPropertyChanged(nameof(PortList));
        }
    }

    public class SelectedInfo: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private static SelectedInfo instance = new SelectedInfo();

        private string port;
        public string Port
        {
            get { return port; }
            set
            {
                port = value;
                NotifyPropertyChanged(nameof(Port));
            }
        }
        public uint Baudrate { get; set; }
        public uint DataBits { get; set; }
        public Parity Parity { get; set; }
        public uint StopBits { get; set; }

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
