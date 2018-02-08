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
			{DetectorType.RAW, 4}
		};
		static readonly Dictionary<uint, DetectorType> int2mode = new Dictionary<uint, DetectorType> {
			{1, DetectorType.LSB},
			{2, DetectorType.USB},
			{3, DetectorType.CW},
			{4, DetectorType.NFM},
			{5, DetectorType.AM}
		};
		
		public string EndMarker { get { return ";"; } }

		SerialRadioInterface _radio;
		
		public Protocol_TS50(SerialRadioInterface radio)
		{
			_radio = radio;
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
    	
		public string PktReceiver(string ReveivedData)
		{
			string response = "";
			if (ReveivedData.StartsWith("IF", StringComparison.Ordinal)) {
				response += "IF";
				response += String.Format("{0:00000000000}", _radio.RadioFrequency);
				response += "0000000000000000";
				response += mode2int[_radio.RadioMode];
				response += "0000000";
				response += EndMarker;
			}
			if (ReveivedData.StartsWith("FA", StringComparison.Ordinal)) {
				long freq;
				if (long.TryParse(ReveivedData.Substring(2), out freq)) {
					_radio.RadioFrequency = freq;
				}
			}
			if (ReveivedData.StartsWith("MD", StringComparison.Ordinal)) {
				uint mode;
				if (uint.TryParse(ReveivedData.Substring(2), out mode)) {
					_radio.RadioMode = int2mode[mode];
				}
			}
			return response;
		}
		
	}
}
