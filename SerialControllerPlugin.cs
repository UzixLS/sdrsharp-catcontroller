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

		long _LastRadioFrequency;
		DetectorType _LastRadioMode;

		
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
			_LastRadioFrequency = _control.Frequency;
			_LastRadioMode = _control.DetectorType;
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
            switch (e.PropertyName)
            {
                case "Frequency":
            		if (_LastRadioFrequency == _control.Frequency)
            			return;
                    break;
                case "DetectorType":
                    if( _LastRadioMode == _control.DetectorType)
                    	return;
                    break;
            }
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
        		_LastRadioFrequency = value;
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
        		_LastRadioMode = value;
	        	_controlPanel.addToLogList(value.ToString());
	        	_control.DetectorType = value;        		
        	}
        }
        
    }
}
