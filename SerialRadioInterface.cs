/*
 * Created by SharpDevelop.
 * User: uzix
 * Date: 04.01.2017
 * Time: 16:46
 */
using System;

using SDRSharp.Radio;


namespace SDRSharp.SerialController
{
	public interface SerialRadioInterface
	{
		long RadioFrequency { get; set; }
		DetectorType RadioMode { get; set; }
	}
}
