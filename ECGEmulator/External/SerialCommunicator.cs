using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECGEmulator.External
{
    public class SerialCommunicator : SerialPort
    {
        public SerialCommunicator() : base()
        {
        }

        public SerialCommunicator(string comPort) : base(comPort)
        {
        }

        public SerialCommunicator(string comPort, int baudRate) : base(comPort, baudRate)
        {
        }

        private void InitializeComponent()
        {

        }
    }
}
