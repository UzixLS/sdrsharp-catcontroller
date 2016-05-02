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
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class SerialPktProcessor
	{
		static Dictionary<DetectorType, uint> mode2int = new Dictionary<DetectorType, uint> {
			{DetectorType.NFM, 4},
			{DetectorType.WFM, 4},
			{DetectorType.AM,  5},
			{DetectorType.DSB, 5},
			{DetectorType.LSB, 1},
			{DetectorType.USB, 2},
			{DetectorType.CW,  3},
			{DetectorType.RAW, 4}
		};
		static Dictionary<uint, DetectorType> int2mode = new Dictionary<uint, DetectorType> {
			{1, DetectorType.LSB},
			{2, DetectorType.USB},
			{3, DetectorType.CW},
			{4, DetectorType.NFM},
			{5, DetectorType.AM}
		};
		public readonly char separator = ';';
			
		public delegate void FrequencyChangeHandler(object sender, long freq);
    	public event FrequencyChangeHandler OnFrequencyChange;
    	
    	public delegate long GetFrequencyHandler();
    	public event GetFrequencyHandler OnGetFrequency;

    	public delegate void ModeChangeHandler(object sender, DetectorType mode);
		public event ModeChangeHandler OnModeChange;    	

    	public delegate DetectorType GetModeHandler();
    	public event GetModeHandler OnGetMode;
    	
		public string process(string data)
		{
			string response = "";
			// TS-50 command parse
			if (data.StartsWith("IF", StringComparison.Ordinal)) {
				long freq = OnGetFrequency();
				DetectorType mode = OnGetMode();
				response += "IF";
				response += String.Format("{0:00000000000}", freq);
				response += "0000000000000000";
				response += mode2int[mode];
				response += "0000000";
				response += separator;
			}
			if (data.StartsWith("FA", StringComparison.Ordinal)) {
				long freq;
				if (long.TryParse(data.Substring(2), out freq)) {
					OnFrequencyChange(this, freq);
				}
			}
			if (data.StartsWith("MD", StringComparison.Ordinal)) {
				uint mode;
				if (uint.TryParse(data.Substring(2), out mode)) {
					OnModeChange(this, int2mode[mode]);
				}
			}
			return response;
		}
		
	}
}
