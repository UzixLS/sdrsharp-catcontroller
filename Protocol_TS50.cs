/*
 * Created by SharpDevelop.
 * User: uzix
 * Date: 02.05.2016
 * Time: 17:00
 */
using System;
using System.Collections.Generic;

using SDRSharp.Radio;


namespace SDRSharp.SerialController
{
	public class Protocol_TS50 : ProtocolInterface
	{
		static readonly Dictionary<DetectorType, uint> mode2int = new Dictionary<DetectorType, uint> {
			{DetectorType.NFM, 4},
			{DetectorType.WFM, 4},
			{DetectorType.AM,  5},
			{DetectorType.DSB, 5},
			{DetectorType.LSB, 1},
			{DetectorType.USB, 2},
			{DetectorType.CW,  3},
			{DetectorType.RAW, 8}
		};
		static readonly Dictionary<uint, DetectorType> int2mode = new Dictionary<uint, DetectorType> {
			{1, DetectorType.LSB},
			{2, DetectorType.USB},
			{3, DetectorType.CW},
			{4, DetectorType.NFM},
			{5, DetectorType.AM},
			{8, DetectorType.RAW}
		};
		
		public string EndMarker { get { return ";"; } }
		public int MaxLen { get { return 255; } }
		
		SerialRadioInterface _radio;
		bool _DetectorSetFailure;
		
		
		public Protocol_TS50(SerialRadioInterface radio)
		{
			_radio = radio;
			_DetectorSetFailure = false;
		}
		
    	public string PktTransmitter(string ChangedProperty)
    	{
    		string response = "";
            switch (ChangedProperty)
            {
                case "Frequency":
            		response = "FA" + String.Format("{0:00000000000}", _radio.RadioFrequency) + ";";
                    break;
                case "DetectorType":
                    response = "MD" + mode2int[_radio.RadioMode] + ";";
                    break;
            }
            return response;
    	}
    	
		public string PktReceiver(string ReceivedData)
		{
			string response = "";
			if (ReceivedData.StartsWith("IF", StringComparison.Ordinal)) {
				response += "IF";
				response += String.Format("{0:00000000000}", _radio.RadioFrequency);
				response += "0000000000000000";
				if ( _DetectorSetFailure)
					response += 0;
				else
					response += mode2int[_radio.RadioMode];
				response += "0000000";
				response += EndMarker;
			}
			else if (ReceivedData == "FA") {
				response += "FA";
				response += String.Format("{0:00000000000}", _radio.RadioFrequency);
				response += EndMarker;
			}
			else if (ReceivedData.StartsWith("FA", StringComparison.Ordinal)) {
				long freq;
				if (long.TryParse(ReceivedData.Substring(2), out freq)) {
					_radio.RadioFrequency = freq;
				}
			}
			else if (ReceivedData == "MD") {
				response += "MD";
				if (_DetectorSetFailure)
					response += 0;
				else
					response += mode2int[_radio.RadioMode];
				response += EndMarker;
			}
			else if (ReceivedData.StartsWith("MD", StringComparison.Ordinal)) {
				uint mode;
				if (uint.TryParse(ReceivedData.Substring(2), out mode)) {
					try {
						_radio.RadioMode = int2mode[mode];
						_DetectorSetFailure = false;
					}
					catch {
						_DetectorSetFailure = true;
					}
				}
			}
			else if (ReceivedData == "ID") {
				response += "ID";
				response += "021"; //XXX: TS-590S value, idk what's should be there for TS-50
				response += EndMarker;
			}
			else if (ReceivedData == "RX") {
				response += "RX";
				response += EndMarker;
			}
			
			return response;
		}
		
	}
}
