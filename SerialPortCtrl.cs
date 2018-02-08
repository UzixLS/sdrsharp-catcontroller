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
		
		public SerialPortCtrl(ProtocolInterface protocol)
		{
			_protocol = protocol;
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
			string data = "";
			while (data.IndexOf(_protocol.EndMarker) < 0) {
				byte[] bytes = new byte[_port.BytesToRead+32];
				try {
					_port.Read(bytes, 0, _port.BytesToRead);
				}
				catch (Exception) {
					return;
				}
				data += System.Text.Encoding.UTF8.GetString(bytes);
			}
			data = data.Substring(0, data.IndexOf(_protocol.EndMarker));
			
			string response = _protocol.PktReceiver(data);
			if (! string.IsNullOrEmpty(response))
				DataTransmit(response);
        }
		
		public void DataTransmit(string data)
		{
			_port.Write(data);
		}
	}
}
