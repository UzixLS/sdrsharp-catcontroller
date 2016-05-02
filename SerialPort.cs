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
		bool _enableLogging = true;
		public bool EnableLogging {
			set { this._enableLogging = value; }
			get { return this._enableLogging; }
		}
		
		StreamWriter logger;
		SerialPort _port;
		
		public delegate void FrequencyChangeHandler(object sender, long freq);
    	public event FrequencyChangeHandler OnFrequencyChange;
				
    	public static string[] GetAllPorts()
		{
			try {
				return SerialPort.GetPortNames();
			} catch {
				MessageBox.Show("Exception while getting available serial ports", "SerialController", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return new string[0];
			}
		}
		
		public bool openPort(string portName) {
			try {
	    		if (_port != null && _port.IsOpen) {
					return false;
				}
    			
    			if (portName == null || (portName.Trim().Equals("")))
    				return false;

				_port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
				_port.DataReceived += Port_DataReceived;
			
			
				if (_port != null) {
					_port.Open();
					if (_enableLogging) {
						prepareLogger();
						log("Port " + _port.PortName + " opened");
					}
					return true;
				}
				return false;
			} catch (Exception) {
    			MessageBox.Show("Couldn't open port "+portName, "SerialController", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}
		
		public bool closePort() {
			if (_port != null) {
				if (_port.IsOpen) {
					try {
						_port.Close();
						if (_enableLogging) {
							log("Port " + _port.PortName + " closed");
							closeLogger();
						}
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
			if (OnFrequencyChange == null) return;
			
			string data = _port.ReadLine();
			// log commands to file
			log("Received on COM port: "+data);
			// AR-ONE RF command parse, as simple as can be, but faster than regex
			if (data.StartsWith("RF", StringComparison.Ordinal)) {
				long freq;
				if (long.TryParse(data.Substring("RF".Length), out freq)) {
					OnFrequencyChange(this, freq);
					log("Changing frequency to: "+freq.ToString("N0"));
				}
			}
        }
		
		void log(String str) {
			if (logger!=null) {
				logger.WriteLine("[" + DateTime.Now + "]: " + str.Trim());
			}
		}
		
		void prepareLogger() {
			try {
				if (logger != null) {
					logger.Close();
				}
				logger = new StreamWriter(new FileStream("serial.log",
				                                         FileMode.Append,
				                                         FileAccess.Write,
				                                         FileShare.ReadWrite,
				                                       	 1024,
				                                         FileOptions.WriteThrough));
				logger.AutoFlush = true;
			} catch (Exception) {
				logger = null;
			}
		}
		void closeLogger() {
			try {
				if (logger != null) {
					logger.Close();
				}
				logger = null;
			} catch (Exception) {
				logger = null;
			}
		}
	}
}
