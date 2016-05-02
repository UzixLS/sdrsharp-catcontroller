using System;
using System.Windows.Forms;

using SDRSharp.Common;
using SDRSharp.Radio;


namespace SDRSharp.SerialController
{
    public class SerialControllerPlugin: ISharpPlugin
    {
        private const string _displayName = "SerialController";

        private ISharpControl _control;
        private SerialControllerPanel _controlPanel;
        private SerialPortCtrl _serialPort;
        private SerialPktProcessor _serialPktProcessor;

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

            _serialPktProcessor = new SerialPktProcessor();
			_serialPktProcessor.OnFrequencyChange += UpdateFrequency;
			_serialPktProcessor.OnGetFrequency += GetFrequency;
			_serialPktProcessor.OnModeChange += UpdateDemodulation;
			_serialPktProcessor.OnGetMode += GetDemodulation;

            _serialPort = new SerialPortCtrl(_serialPktProcessor);
            _serialPort.separator = _serialPktProcessor.separator;
            
            _controlPanel = new SerialControllerPanel(_serialPort);
            _controlPanel.readSettings();
        }
        
        void UpdateFrequency(object sender, long freq) {
        	_control.Frequency = freq;
        	_controlPanel.addToLogList(freq.ToString("N0")+" Hz");
        }
        
        long GetFrequency() {
        	return _control.Frequency;
        }
        
        void UpdateDemodulation(object sender, DetectorType mode) {
        	_control.DetectorType = mode;
        	_controlPanel.addToLogList(mode.ToString());
        }
        
        DetectorType GetDemodulation() {
        	return _control.DetectorType;
        }
        
        public void Close()
        {
        	_serialPort.closePort();
        	_controlPanel.saveSettings();
        }        
    }
}
