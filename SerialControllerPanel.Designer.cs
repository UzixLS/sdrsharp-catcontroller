namespace SDRSharp.SerialController
{
    partial class SerialControllerPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialControllerPanel));
        	this.cbEnable = new System.Windows.Forms.CheckBox();
        	this.comboPorts = new System.Windows.Forms.ComboBox();
        	this.btnRefreshPorts = new System.Windows.Forms.Button();
        	this.lbLog = new System.Windows.Forms.ListBox();
        	this.SuspendLayout();
        	// 
        	// cbEnable
        	// 
        	this.cbEnable.Location = new System.Drawing.Point(3, 3);
        	this.cbEnable.Name = "cbEnable";
        	this.cbEnable.Size = new System.Drawing.Size(74, 22);
        	this.cbEnable.TabIndex = 1;
        	this.cbEnable.Text = "Enable";
        	this.cbEnable.UseVisualStyleBackColor = true;
        	this.cbEnable.Click += new System.EventHandler(this.CbEnableClick);
        	this.cbEnable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CbEnableKeyDown);
        	// 
        	// comboPorts
        	// 
        	this.comboPorts.FormattingEnabled = true;
        	this.comboPorts.Location = new System.Drawing.Point(83, 3);
        	this.comboPorts.Name = "comboPorts";
        	this.comboPorts.Size = new System.Drawing.Size(100, 21);
        	this.comboPorts.TabIndex = 2;
        	// 
        	// btnRefreshPorts
        	// 
        	this.btnRefreshPorts.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshPorts.Image")));
        	this.btnRefreshPorts.Location = new System.Drawing.Point(188, 2);
        	this.btnRefreshPorts.Name = "btnRefreshPorts";
        	this.btnRefreshPorts.Size = new System.Drawing.Size(24, 24);
        	this.btnRefreshPorts.TabIndex = 3;
        	this.btnRefreshPorts.UseVisualStyleBackColor = true;
        	this.btnRefreshPorts.Click += new System.EventHandler(this.BtnRefreshPortsClick);
        	// 
        	// lbLog
        	// 
        	this.lbLog.FormattingEnabled = true;
        	this.lbLog.Location = new System.Drawing.Point(3, 30);
        	this.lbLog.Name = "lbLog";
        	this.lbLog.Size = new System.Drawing.Size(215, 69);
        	this.lbLog.TabIndex = 4;
        	// 
        	// SerialControllerPanel
        	// 
        	this.Controls.Add(this.lbLog);
        	this.Controls.Add(this.btnRefreshPorts);
        	this.Controls.Add(this.comboPorts);
        	this.Controls.Add(this.cbEnable);
        	this.Name = "SerialControllerPanel";
        	this.Size = new System.Drawing.Size(222, 131);
        	this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox cbEnable;
        private System.Windows.Forms.ComboBox comboPorts;
        private System.Windows.Forms.Button btnRefreshPorts;
        private System.Windows.Forms.ListBox lbLog;


    }
}
