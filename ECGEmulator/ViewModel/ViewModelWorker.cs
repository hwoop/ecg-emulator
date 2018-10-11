using ECGEmulator.External;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
            Serial.DataReceived += new SerialDataReceivedEventHandler(OnDataRecieved);

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

        public void CloseSerialPort()
        {
            Serial.DataReceived -= OnDataRecieved;
            Serial?.Close();

            if (Serial.IsOpen)
                Console.WriteLine("Fail to close {0} port.", Serial.PortName);
            else
                Console.WriteLine("Closed");
        }

        private byte[] STX = new byte[] { 0x02 };
        private byte[] ETX = new byte[] { 0x03 };
        public void Send(string data)
        {
            if (!Serial.IsOpen)
                return;

            byte[] datas = STX.Concat(Encoding.ASCII.GetBytes(data)).Concat(ETX).ToArray();

            //Serial.Write(STX, 0, 1);
            Serial.Write(datas, 0, datas.Length);
            //Serial.Write(ETX, 0, 1);
        }

        private void OnDataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            string data = (sender as SerialPort).ReadExisting();
        }
    }
}
