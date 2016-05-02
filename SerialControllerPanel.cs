using System;
using System.Windows.Forms;

using SDRSharp.Radio;

namespace SDRSharp.SerialController
{
    public partial class SerialControllerPanel : UserControl
    {
    	readonly SerialPortCtrl _serialPort;
    	
        public SerialControllerPanel(SerialPortCtrl serialPort)
        {
            InitializeComponent();
            _serialPort = serialPort;
            
            BtnRefreshPortsClick(this, null);
        }
		
		void BtnRefreshPortsClick(object sender, EventArgs e) {
			comboPorts.Items.Clear();
			comboPorts.Items.AddRange(SerialPortCtrl.GetAllPorts());
			if (comboPorts.Items.Count > 0)
				comboPorts.SelectedIndex = 0;
		}
		
		public void addToLogList(String log) {
			lbLog.Items.Add(log);
			// scroll to bottom
			lbLog.SelectedIndex = lbLog.Items.Count - 1;
			lbLog.SelectedIndex = -1;
		}
		
		public void readSettings() {
			comboPorts.Text = Utils.GetStringSetting("serialControlComPort", "");
			cbLogToFile.Checked = Utils.GetBooleanSetting("serialControlLogToFile");
			cbEnable.Checked = Utils.GetBooleanSetting("serialControlEnable");
			CbEnableClick(null,null);
		}
		
		public void saveSettings() {
			Utils.SaveSetting("serialControlComPort", comboPorts.Text);
			Utils.SaveSetting("serialControlLogToFile", cbLogToFile.Checked);
			Utils.SaveSetting("serialControlEnable", cbEnable.Checked);
		}
		void CbEnableClick(object sender, EventArgs e)
		{
			if (! _serialPort.IsOpen)
				_serialPort.openPort(comboPorts.Text);
			else
				_serialPort.closePort();
			cbEnable.Checked = _serialPort.IsOpen;
			comboPorts.Enabled = !cbEnable.Checked;
			btnRefreshPorts.Enabled = !cbEnable.Checked;
			cbLogToFile.Enabled = !cbEnable.Checked;
			_serialPort.EnableLogging = cbLogToFile.Checked;
		}
		void CbEnableKeyDown(object sender, EventArgs e)
		{
			CbEnableClick(sender, e);
		}
    }
}
