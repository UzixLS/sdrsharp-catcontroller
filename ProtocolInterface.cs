/*
 * Created by SharpDevelop.
 * User: uzix
 * Date: 04.01.2017
 * Time: 16:02
 */
using System;

namespace SDRSharp.SerialController
{
	public interface ProtocolInterface
	{
		string EndMarker { get; }
		int MaxLen { get; }
		string PktTransmitter(string ChangedProperty);
		string PktReceiver(string ReveivedData);
	}
}
