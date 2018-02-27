/*
 * Author: Pawel Walczak (pewusoft)
 * Date: 2015-01-12 20:50
 *
 */
using System;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms;


namespace SDRSharp.SerialController
{
	public class SerialPortCtrl
	{
		public bool IsOpen {
			get { return _port != null && _port.IsOpen; }
		}
		
		SerialPort _port;
		ProtocolInterface _protocol;
		string _received;
		
		public SerialPortCtrl(ProtocolInterface protocol)
		{
			_protocol = protocol;
			_received = "";
		}
		
    	public static string[] GetAllPorts()
		{
			try {
				return SerialPort.GetPortNames();
    		}
    		catch (Exception e) {
    			MessageBox.Show("Couldn't read port list:\n"+e.ToString(), "SerialController", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return new string[0];
			}
		}
		
		public bool openPort(string portName)
		{
			try {
	    		if (_port != null && _port.IsOpen)
					return false;
    			
    			if (portName == null || (portName.Trim().Equals("")))
    				return false;

				_port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
				_port.DataReceived += new SerialDataReceivedEventHandler( DataReceivedHandler );
						
				if (_port != null) {
					_port.Open();
					return true;
				}
				return false;
			}
			catch (Exception e) {
    			MessageBox.Show("Couldn't open port "+portName+":\n"+e.ToString(), "SerialController",
    			                MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}
		
		public bool closePort()
		{
			if (_port != null)	{
				if (_port.IsOpen) {
					try {
						_port.Close();
						return true;
					}
					catch (IOException)	 {
						return false;
					}
				}
				else {
					return false;
				}
			}
			return false;
		}
		
		void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
			while (_port.BytesToRead > 0) {
				if (_received.Length > _protocol.MaxLen)
					_received = "";
				
				string input = ((char)_port.ReadChar()).ToString();
				if (input == _protocol.EndMarker) {
					string response = _protocol.PktReceiver(_received);
					if (! string.IsNullOrEmpty(response)) {
						DataTransmit(response);
					}
					_received = "";
				}
				else {
					_received += input;
				}
			}
        }
		
		public void DataTransmit(string data)
		{
			_port.Write(data);
		}
	}
}
