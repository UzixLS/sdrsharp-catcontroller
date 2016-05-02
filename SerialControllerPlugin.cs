using System;
using System.Windows.Forms;

using SDRSharp.Common;

namespace SDRSharp.SerialController
{
    public class SerialControllerPlugin: ISharpPlugin
    {
        private const string _displayName = "SerialController";

        private SerialControllerPanel _controlPanel;
        private SerialPortCtrl _serialPort;
        private ISharpControl _control;

        public string DisplayName
        {
            get { return _displayName; }
        }

        public bool HasGui
        {
            get { return true; }
        }

        public UserControl Gui
        {
            get { return _controlPanel; }
        }

        public void Initialize(ISharpControl control)
        {
            _control = control;
            _serialPort = new SerialPortCtrl();
			_serialPort.OnFrequencyChange += UpdateFrequency;
            
            _controlPanel = new SerialControllerPanel(_serialPort);
            _controlPanel.readSettings();
        }
        
        void UpdateFrequency(object sender, long freq) {
        	_control.Frequency = freq;
        	_controlPanel.addToLogList(freq.ToString("N0")+" Hz");
        	
        }
        
        public void Close()
        {
        	_serialPort.closePort();
        	_controlPanel.saveSettings();
        }        
    }
}
