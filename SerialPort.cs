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
	/// <summary>
	/// Description of SerialPort.
	/// </summary>
	public class SerialPortCtrl
	{
		public bool IsOpen {
			get { return _port != null && _port.IsOpen; }
		}
		
		char _separator;
		public char separator {
			get { return separator; }
			set { this._separator = value; }
		}
		
		SerialPort _port;
		SerialPktProcessor _pktprocessor;
		
		public SerialPortCtrl( SerialPktProcessor pktprocessor )
		{
			_pktprocessor = pktprocessor;
		}
		
    	public static string[] GetAllPorts()
		{
			try {
				return SerialPort.GetPortNames();
    		} catch (Exception e) {
    			MessageBox.Show("Cannot read port list:\n"+e.ToString(), "SerialController", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return new string[0];
			}
		}
		
		public bool openPort(string portName) {
			try {
	    		if (_port != null && _port.IsOpen)
					return false;
    			
    			if (portName == null || (portName.Trim().Equals("")))
    				return false;

				_port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
				_port.DataReceived += new SerialDataReceivedEventHandler( Port_DataReceived );
						
				if (_port != null) {
					_port.Open();
					return true;
				}
				return false;
			} catch (Exception e) {
    			MessageBox.Show("Couldn't open port "+portName+":\n"+e.ToString(), "SerialController",
    			                MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}
		
		public bool closePort() {
			if (_port != null) {
				if (_port.IsOpen) {
					try {
						_port.Close();
						return true;
					} catch (IOException) {
						return false;
					}
				} else {
					return false;
				}
			}
			return false;
		}
		
		void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
			string data = "";
			while (data.IndexOf(_separator) < 0) {
				byte[] bytes = new byte[_port.BytesToRead+32];
				try {
					_port.Read(bytes, 0, _port.BytesToRead);
				}
				catch (Exception) {
					return;
				}
				data += System.Text.Encoding.UTF8.GetString(bytes);
			}
			data = data.Substring(0, data.IndexOf(_separator));
			
			string response = _pktprocessor.process(data);
			if (! string.IsNullOrEmpty(response))
				_port.Write(response);
        }
	}
}
