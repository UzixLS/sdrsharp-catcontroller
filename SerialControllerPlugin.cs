using System;
using System.ComponentModel;
using System.Windows.Forms;

using SDRSharp.Common;
using SDRSharp.Radio;


namespace SDRSharp.SerialController
{
    public class SerialControllerPlugin: ISharpPlugin,SerialRadioInterface
    {
        private const string _displayName = "SerialController";

        private ISharpControl _control;
        private SerialControllerPanel _controlPanel;
        private SerialPortCtrl _serialPort;
        private ProtocolInterface _Protocol;

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
			_control.PropertyChanged += PropertyChangedHandler;
            _Protocol = new Protocol_TS50(this);
            _serialPort = new SerialPortCtrl(_Protocol);           
            _controlPanel = new SerialControllerPanel(_serialPort);
            _controlPanel.readSettings();
        }
        
        public void Close()
        {
        	_serialPort.closePort();
        	_controlPanel.saveSettings();
        }        

        
        void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
		{
        	if (_serialPort.IsOpen)
		    {
        		string response = _Protocol.PktTransmitter(e.PropertyName);
				if (! string.IsNullOrEmpty(response))
					_serialPort.DataTransmit(response);
		    }
		}
        
        public long RadioFrequency
        {
        	get {
        		return _control.Frequency;
        	}
        	set {
        		_controlPanel.addToLogList(value.ToString("N0")+" Hz");
        		_control.Frequency = value;
        	}
        }
            
        public DetectorType RadioMode
        {
        	get {
        		return _control.DetectorType;
        	}
        	set {
	        	_controlPanel.addToLogList(value.ToString());
	        	_control.DetectorType = value;        		
        	}
        }
        
    }
}
