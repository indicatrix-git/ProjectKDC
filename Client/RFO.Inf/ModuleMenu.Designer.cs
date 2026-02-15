namespace Ditec.RIS.RFO.Inf
{
    partial class ModuleMenu
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.barManager1 = new DevExpress.XtraBars.BarManager();
			this.miMainMenu = new DevExpress.XtraBars.Bar();
			this.bbiRFO = new DevExpress.XtraBars.BarButtonItem();
			this.bsiOpravneCinnosti = new DevExpress.XtraBars.BarSubItem();
			this.bbiOpravneCinnostiRISRFO = new DevExpress.XtraBars.BarButtonItem();
			this.bbiStotoznenieOsobVRIS = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.bbiSaSZ = new DevExpress.XtraBars.BarButtonItem();
			((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
			this.SuspendLayout();
			// 
			// barManager1
			// 
			this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.miMainMenu});
			this.barManager1.DockControls.Add(this.barDockControlTop);
			this.barManager1.DockControls.Add(this.barDockControlBottom);
			this.barManager1.DockControls.Add(this.barDockControlLeft);
			this.barManager1.DockControls.Add(this.barDockControlRight);
			this.barManager1.Form = this;
			this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.bbiRFO,
            this.bbiSaSZ,
            this.bsiOpravneCinnosti,
            this.bbiOpravneCinnostiRISRFO,
            this.bbiStotoznenieOsobVRIS});
			this.barManager1.MainMenu = this.miMainMenu;
			this.barManager1.MaxItemId = 7;
			// 
			// miMainMenu
			// 
			this.miMainMenu.BarName = "Main menu";
			this.miMainMenu.DockCol = 0;
			this.miMainMenu.DockRow = 0;
			this.miMainMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.miMainMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiRFO),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiOpravneCinnosti)});
			this.miMainMenu.OptionsBar.MultiLine = true;
			this.miMainMenu.OptionsBar.UseWholeRow = true;
			this.miMainMenu.Text = "Main menu";
			// 
			// bbiRFO
			// 
			this.bbiRFO.Caption = "Register FO";
			this.bbiRFO.Id = 0;
			this.bbiRFO.Name = "bbiRFO";
			this.bbiRFO.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiRFO_ItemClick);
			// 
			// bsiOpravneCinnosti
			// 
			this.bsiOpravneCinnosti.Caption = "Opravné činnosti";
			this.bsiOpravneCinnosti.Id = 4;
			this.bsiOpravneCinnosti.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiOpravneCinnostiRISRFO),
            new DevExpress.XtraBars.LinkPersistInfo(this.bbiStotoznenieOsobVRIS)});
			this.bsiOpravneCinnosti.Name = "bsiOpravneCinnosti";
			// 
			// bbiOpravneCinnostiRISRFO
			// 
			this.bbiOpravneCinnostiRISRFO.Caption = "Opravné činnosti RIS-RFO";
			this.bbiOpravneCinnostiRISRFO.Id = 5;
			this.bbiOpravneCinnostiRISRFO.Name = "bbiOpravneCinnostiRISRFO";
			this.bbiOpravneCinnostiRISRFO.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiOpravneCinnostiRISRFO_ItemClick);
			// 
			// bbiStotoznenieOsobVRIS
			// 
			this.bbiStotoznenieOsobVRIS.Caption = "Stotožnenie osôb v RIS";
			this.bbiStotoznenieOsobVRIS.Id = 6;
			this.bbiStotoznenieOsobVRIS.Name = "bbiStotoznenieOsobVRIS";
			this.bbiStotoznenieOsobVRIS.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiStotoznenieOsobVRIS_ItemClick);
			// 
			// barDockControlTop
			// 
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(284, 22);
			// 
			// barDockControlBottom
			// 
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 261);
			this.barDockControlBottom.Size = new System.Drawing.Size(284, 0);
			// 
			// barDockControlLeft
			// 
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 22);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 239);
			// 
			// barDockControlRight
			// 
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(284, 22);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 239);
			// 
			// bbiSaSZ
			// 
			this.bbiSaSZ.Id = 3;
			this.bbiSaSZ.Name = "bbiSaSZ";
			// 
			// ModuleMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.barDockControlLeft);
			this.Controls.Add(this.barDockControlRight);
			this.Controls.Add(this.barDockControlBottom);
			this.Controls.Add(this.barDockControlTop);
			this.Name = "ModuleMenu";
			this.Text = "ModuleMenu";
			((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar miMainMenu;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private DevExpress.XtraBars.BarButtonItem bbiRFO;
		private DevExpress.XtraBars.BarButtonItem bbiSaSZ;
		private DevExpress.XtraBars.BarSubItem bsiOpravneCinnosti;
		private DevExpress.XtraBars.BarButtonItem bbiOpravneCinnostiRISRFO;
		private DevExpress.XtraBars.BarButtonItem bbiStotoznenieOsobVRIS;
    }
}