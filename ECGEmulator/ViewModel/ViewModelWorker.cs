using ECGEmulator.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECGEmulator.ViewModel
{
    public class ViewModelWorker
    {
        SerialCommunicator Serial;
        public ViewModelWorker()
        {
            Serial = new SerialCommunicator();
        }

        public bool OpenSerialPort()
        {
            if (Serial.IsOpen)
                return false;

            Serial.PortName = SelectedInfo.Instance.Port;
            Serial.BaudRate = SelectedInfo.Instance.Baudrate;
            Serial.DataBits = SelectedInfo.Instance.DataBits;
            Serial.StopBits = SelectedInfo.Instance.StopBits;
            Serial.Parity = SelectedInfo.Instance.Parity;
            
            Serial.Open();

            if (Serial.IsOpen)
            {
                Console.WriteLine("Connected");
                return true;
            }
            else
            {
                Console.WriteLine("Fail to open {0} port.", Serial.PortName);
                return false;
            }
        }
    }
}
