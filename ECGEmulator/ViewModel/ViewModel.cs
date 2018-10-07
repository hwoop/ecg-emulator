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
    public sealed class SelecetedInfo
    {
        private static SelecetedInfo instance = new SelecetedInfo();

        public string Port { get; set; }
        public uint Baudrate { get; set; }
        public uint DataBits { get; set; }
        public Parity Parity { get; set; }
        public uint StopBits { get; set; }

        private SelecetedInfo()
        {
        }

        public static SelecetedInfo getInstance()
        {
            return instance;
        }
    }

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
                        param => Worker.TryConnect(SelectedPort)
                        );
                }
                return cmdTryConnect;
            }
        }
        #endregion
        
        #region Properties
        public ObservableCollection<string> PortList { get; set; }

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
}
