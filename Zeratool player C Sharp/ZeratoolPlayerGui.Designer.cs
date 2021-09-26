
namespace Zeratool_player_C_Sharp
{
    partial class ZeratoolPlayerGui
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZeratoolPlayerGui));
            this.panelControls = new System.Windows.Forms.Panel();
            this.btnPreviousTrack = new System.Windows.Forms.Panel();
            this.btnNextTrack = new System.Windows.Forms.Panel();
            this.panelCorner = new System.Windows.Forms.Panel();
            this.btnPlaylist = new System.Windows.Forms.Panel();
            this.btnFullscreen = new System.Windows.Forms.Panel();
            this.btnOpenFile = new System.Windows.Forms.Panel();
            this.btnPause = new System.Windows.Forms.Panel();
            this.btnPlay = new System.Windows.Forms.Panel();
            this.lblTrackPosition = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Panel();
            this.seekBar = new System.Windows.Forms.Panel();
            this.volumeBar = new System.Windows.Forms.Panel();
            this.lblSystemTime = new System.Windows.Forms.Label();
            this.lblTitleBar = new System.Windows.Forms.Label();
            this.panelMaxClose = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Panel();
            this.btnMinMax = new System.Windows.Forms.Panel();
            this.timerSystemTime = new System.Windows.Forms.Timer(this.components);
            this.timerTrack = new System.Windows.Forms.Timer(this.components);
            this.panelZ = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnLog = new System.Windows.Forms.Panel();
            this.panelVideoScreen = new Zeratool_player_C_Sharp.CustomPanel();
            this.panelControls.SuspendLayout();
            this.panelMaxClose.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panelControls.Controls.Add(this.btnLog);
            this.panelControls.Controls.Add(this.btnPreviousTrack);
            this.panelControls.Controls.Add(this.btnNextTrack);
            this.panelControls.Controls.Add(this.panelCorner);
            this.panelControls.Controls.Add(this.btnPlaylist);
            this.panelControls.Controls.Add(this.btnFullscreen);
            this.panelControls.Controls.Add(this.btnOpenFile);
            this.panelControls.Controls.Add(this.btnPause);
            this.panelControls.Controls.Add(this.btnPlay);
            this.panelControls.Controls.Add(this.lblTrackPosition);
            this.panelControls.Controls.Add(this.btnSettings);
            this.panelControls.Controls.Add(this.seekBar);
            this.panelControls.Controls.Add(this.volumeBar);
            this.panelControls.Controls.Add(this.lblSystemTime);
            this.panelControls.Location = new System.Drawing.Point(0, 199);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(386, 77);
            this.panelControls.TabIndex = 0;
            this.panelControls.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelControls_MouseDown);
            // 
            // btnPreviousTrack
            // 
            this.btnPreviousTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviousTrack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPreviousTrack.BackgroundImage")));
            this.btnPreviousTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPreviousTrack.Location = new System.Drawing.Point(290, 0);
            this.btnPreviousTrack.Name = "btnPreviousTrack";
            this.btnPreviousTrack.Size = new System.Drawing.Size(32, 32);
            this.btnPreviousTrack.TabIndex = 12;
            this.toolTip1.SetToolTip(this.btnPreviousTrack, "Предыдущий файл");
            this.btnPreviousTrack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPreviousTrack_MouseDown);
            // 
            // btnNextTrack
            // 
            this.btnNextTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextTrack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNextTrack.BackgroundImage")));
            this.btnNextTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextTrack.Location = new System.Drawing.Point(354, 0);
            this.btnNextTrack.Name = "btnNextTrack";
            this.btnNextTrack.Size = new System.Drawing.Size(32, 32);
            this.btnNextTrack.TabIndex = 11;
            this.toolTip1.SetToolTip(this.btnNextTrack, "Следующий файл");
            this.btnNextTrack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNextTrack_MouseDown);
            // 
            // panelCorner
            // 
            this.panelCorner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCorner.BackColor = System.Drawing.Color.Lime;
            this.panelCorner.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.panelCorner.Location = new System.Drawing.Point(374, 64);
            this.panelCorner.Name = "panelCorner";
            this.panelCorner.Size = new System.Drawing.Size(11, 11);
            this.panelCorner.TabIndex = 10;
            this.toolTip1.SetToolTip(this.panelCorner, "Потяни меня");
            this.panelCorner.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCorner_MouseDown);
            this.panelCorner.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelCorner_MouseMove);
            // 
            // btnPlaylist
            // 
            this.btnPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlaylist.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlaylist.BackgroundImage")));
            this.btnPlaylist.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlaylist.Location = new System.Drawing.Point(322, 0);
            this.btnPlaylist.Name = "btnPlaylist";
            this.btnPlaylist.Size = new System.Drawing.Size(32, 32);
            this.btnPlaylist.TabIndex = 9;
            this.toolTip1.SetToolTip(this.btnPlaylist, "Список файлов");
            this.btnPlaylist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPlaylist_MouseDown);
            // 
            // btnFullscreen
            // 
            this.btnFullscreen.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFullscreen.BackgroundImage")));
            this.btnFullscreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFullscreen.Location = new System.Drawing.Point(96, 0);
            this.btnFullscreen.Name = "btnFullscreen";
            this.btnFullscreen.Size = new System.Drawing.Size(32, 32);
            this.btnFullscreen.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnFullscreen, "На весь экран и обратно");
            this.btnFullscreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnFullscreen_MouseDown);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOpenFile.BackgroundImage")));
            this.btnOpenFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOpenFile.Location = new System.Drawing.Point(0, 0);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(32, 32);
            this.btnOpenFile.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btnOpenFile, "Открыть файл");
            this.btnOpenFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnOpenFile_MouseDown);
            // 
            // btnPause
            // 
            this.btnPause.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPause.BackgroundImage")));
            this.btnPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPause.Location = new System.Drawing.Point(64, 0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(32, 32);
            this.btnPause.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btnPause, "Пауза");
            this.btnPause.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPause_MouseDown);
            // 
            // btnPlay
            // 
            this.btnPlay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlay.BackgroundImage")));
            this.btnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlay.Location = new System.Drawing.Point(32, 0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(32, 32);
            this.btnPlay.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnPlay, "Плей");
            this.btnPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseDown);
            // 
            // lblTrackPosition
            // 
            this.lblTrackPosition.BackColor = System.Drawing.Color.Black;
            this.lblTrackPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTrackPosition.ForeColor = System.Drawing.Color.Lime;
            this.lblTrackPosition.Location = new System.Drawing.Point(134, 3);
            this.lblTrackPosition.Name = "lblTrackPosition";
            this.lblTrackPosition.Size = new System.Drawing.Size(150, 23);
            this.lblTrackPosition.TabIndex = 4;
            this.lblTrackPosition.Text = "0:00:00 | 0:00:00";
            this.lblTrackPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTrackPosition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelControls_MouseDown);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSettings.BackgroundImage")));
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSettings.Location = new System.Drawing.Point(342, 53);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(24, 24);
            this.btnSettings.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnSettings, "Настройки");
            this.btnSettings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCorner_MouseDown);
            this.btnSettings.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSettings_MouseUp);
            // 
            // seekBar
            // 
            this.seekBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seekBar.BackColor = System.Drawing.Color.LightSteelBlue;
            this.seekBar.Location = new System.Drawing.Point(2, 32);
            this.seekBar.Name = "seekBar";
            this.seekBar.Size = new System.Drawing.Size(380, 20);
            this.seekBar.TabIndex = 2;
            this.seekBar.Paint += new System.Windows.Forms.PaintEventHandler(this.seekBar_Paint);
            this.seekBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.seekBar_MouseDown);
            this.seekBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.seekBar_MouseMove);
            // 
            // volumeBar
            // 
            this.volumeBar.BackColor = System.Drawing.SystemColors.Control;
            this.volumeBar.ForeColor = System.Drawing.Color.Black;
            this.volumeBar.Location = new System.Drawing.Point(0, 54);
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(100, 20);
            this.volumeBar.TabIndex = 1;
            this.toolTip1.SetToolTip(this.volumeBar, "Громкость");
            this.volumeBar.Paint += new System.Windows.Forms.PaintEventHandler(this.volumeBar_Paint);
            this.volumeBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.volumeBar_MouseDown);
            this.volumeBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.volumeBar_MouseMove);
            // 
            // lblSystemTime
            // 
            this.lblSystemTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSystemTime.AutoSize = true;
            this.lblSystemTime.BackColor = System.Drawing.Color.Black;
            this.lblSystemTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSystemTime.ForeColor = System.Drawing.Color.Lime;
            this.lblSystemTime.Location = new System.Drawing.Point(265, 55);
            this.lblSystemTime.Name = "lblSystemTime";
            this.lblSystemTime.Size = new System.Drawing.Size(71, 20);
            this.lblSystemTime.TabIndex = 0;
            this.lblSystemTime.Text = "00:00:00";
            this.lblSystemTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelControls_MouseDown);
            // 
            // lblTitleBar
            // 
            this.lblTitleBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitleBar.BackColor = System.Drawing.Color.Blue;
            this.lblTitleBar.ForeColor = System.Drawing.Color.White;
            this.lblTitleBar.Location = new System.Drawing.Point(20, 0);
            this.lblTitleBar.Name = "lblTitleBar";
            this.lblTitleBar.Size = new System.Drawing.Size(326, 20);
            this.lblTitleBar.TabIndex = 1;
            this.lblTitleBar.Text = "<No name>";
            this.lblTitleBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitleBar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lblTitleBar_MouseDoubleClick);
            this.lblTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitleBar_MouseDown);
            this.lblTitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTitleBar_MouseMove);
            this.lblTitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTitleBar_MouseUp);
            // 
            // panelMaxClose
            // 
            this.panelMaxClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMaxClose.BackColor = System.Drawing.Color.Blue;
            this.panelMaxClose.Controls.Add(this.btnClose);
            this.panelMaxClose.Controls.Add(this.btnMinMax);
            this.panelMaxClose.Location = new System.Drawing.Point(346, 0);
            this.panelMaxClose.Name = "panelMaxClose";
            this.panelMaxClose.Size = new System.Drawing.Size(40, 20);
            this.panelMaxClose.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.Location = new System.Drawing.Point(20, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(20, 20);
            this.btnClose.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnClose, "Закрыть");
            this.btnClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelControls_MouseDown);
            this.btnClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseUp);
            // 
            // btnMinMax
            // 
            this.btnMinMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMinMax.BackgroundImage")));
            this.btnMinMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMinMax.Location = new System.Drawing.Point(0, 0);
            this.btnMinMax.Name = "btnMinMax";
            this.btnMinMax.Size = new System.Drawing.Size(20, 20);
            this.btnMinMax.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnMinMax, "Развернуть");
            this.btnMinMax.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelControls_MouseDown);
            this.btnMinMax.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMinMax_MouseUp);
            // 
            // timerSystemTime
            // 
            this.timerSystemTime.Enabled = true;
            this.timerSystemTime.Interval = 1000;
            this.timerSystemTime.Tick += new System.EventHandler(this.timerSystemTime_Tick);
            // 
            // timerTrack
            // 
            this.timerTrack.Interval = 1000;
            this.timerTrack.Tick += new System.EventHandler(this.timerTrack_Tick);
            // 
            // panelZ
            // 
            this.panelZ.BackColor = System.Drawing.Color.Blue;
            this.panelZ.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelZ.BackgroundImage")));
            this.panelZ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelZ.Location = new System.Drawing.Point(0, 0);
            this.panelZ.Name = "panelZ";
            this.panelZ.Size = new System.Drawing.Size(20, 20);
            this.panelZ.TabIndex = 4;
            this.panelZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelZ_MouseDown);
            // 
            // btnLog
            // 
            this.btnLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLog.BackgroundImage = global::Zeratool_player_C_Sharp.Properties.Resources.log;
            this.btnLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog.Location = new System.Drawing.Point(235, 55);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(24, 24);
            this.btnLog.TabIndex = 13;
            this.toolTip1.SetToolTip(this.btnLog, "Открыть лог");
            this.btnLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLog_MouseDown);
            // 
            // panelVideoScreen
            // 
            this.panelVideoScreen.AllowDrop = true;
            this.panelVideoScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelVideoScreen.BackColor = System.Drawing.Color.Black;
            this.panelVideoScreen.Location = new System.Drawing.Point(0, 20);
            this.panelVideoScreen.Name = "panelVideoScreen";
            this.panelVideoScreen.Size = new System.Drawing.Size(386, 179);
            this.panelVideoScreen.TabIndex = 3;
            this.panelVideoScreen.MouseSingleDown += new Zeratool_player_C_Sharp.CustomPanel.MouseSingleDownDelegate(this.panelVideoScreen_MouseDown);
            this.panelVideoScreen.MouseDoubleDown += new Zeratool_player_C_Sharp.CustomPanel.MouseDoubleDownDelegate(this.panelVideoScreen_MouseDoubleClick);
            this.panelVideoScreen.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelVideoScreen_DragDrop);
            this.panelVideoScreen.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelVideoScreen_DragEnter);
            // 
            // ZeratoolPlayerGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelZ);
            this.Controls.Add(this.panelVideoScreen);
            this.Controls.Add(this.lblTitleBar);
            this.Controls.Add(this.panelMaxClose);
            this.Controls.Add(this.panelControls);
            this.Name = "ZeratoolPlayerGui";
            this.Size = new System.Drawing.Size(386, 276);
            this.Load += new System.EventHandler(this.ZeratoolPlayerGui_Load);
            this.Resize += new System.EventHandler(this.ZeratoolPlayerGui_Resize);
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.panelMaxClose.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Label lblTitleBar;
        private System.Windows.Forms.Panel panelMaxClose;
        private System.Windows.Forms.Label lblSystemTime;
        private System.Windows.Forms.Timer timerSystemTime;
        private System.Windows.Forms.Panel volumeBar;
        private System.Windows.Forms.Panel seekBar;
        private System.Windows.Forms.Timer timerTrack;
        private System.Windows.Forms.Panel btnClose;
        private System.Windows.Forms.Panel btnMinMax;
        private System.Windows.Forms.Panel btnSettings;
        private System.Windows.Forms.Label lblTrackPosition;
        private System.Windows.Forms.Panel btnPlay;
        private System.Windows.Forms.Panel btnPause;
        private System.Windows.Forms.Panel btnOpenFile;
        private System.Windows.Forms.Panel btnFullscreen;
        private CustomPanel panelVideoScreen;
        private System.Windows.Forms.Panel btnPlaylist;
        private System.Windows.Forms.Panel panelCorner;
        private System.Windows.Forms.Panel btnNextTrack;
        private System.Windows.Forms.Panel btnPreviousTrack;
        private System.Windows.Forms.Panel panelZ;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel btnLog;
    }
}
