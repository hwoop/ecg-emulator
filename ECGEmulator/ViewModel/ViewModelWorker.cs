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
            //Serial = new SerialCommunicator();
        }

        public void TryConnect(string portName)
        {

        }
    }
}
