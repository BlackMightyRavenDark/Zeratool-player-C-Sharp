using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static Zeratool_player_C_Sharp.ZeratoolPlayerEngine;
using static Zeratool_player_C_Sharp.DirectShowUtils;
using static Zeratool_player_C_Sharp.Utils;
using Zeratool_player_C_Sharp.Properties;

namespace Zeratool_player_C_Sharp
{
    public partial class ZeratoolPlayerGui : UserControl
    {
        private string _title;
        private bool _isMaximized = false;
        private bool _isFullscreenMode = false;
        public const int MIN_WIDTH = 386;
        public const int MIN_HEIGHT = 200;
        public static readonly Color COLOR_ACTIVE = Color.Blue;
        public static readonly Color COLOR_INACTIVE = Color.SkyBlue;

        private ZeratoolPlayerEngine _playerEngine;
        private ZeratoolPlaylist _playlist;

        public ZeratoolPlayerEngine PlayerEngine => _playerEngine;
        public PlayerState State => PlayerEngine.State;
        public DirectShowGraphMode GraphMode { get { return PlayerEngine.GraphMode; } set { SetGraphMode(value); } }
        public DirectShowGraphMode PrefferedGraphMode { get; set; } = DirectShowGraphMode.Intellectual;
        public string FileName { get { return PlayerEngine.FileName; } }
        public string Title { get { return _title; } set { SetTitle(value); } }
        public int Volume { get { return PlayerEngine.Volume; } set { PlayerEngine.Volume = value; volumeBar.Refresh(); } }
        public double TrackDuration => PlayerEngine.Duration;
        public double TrackPosition { get { return PlayerEngine.Position; } set { SetTrackPosition(value); } }
        public ZeratoolPlaylist Playlist => _playlist;
        public bool IsMaximized => _isMaximized;
        public bool IsFullscreen => _isFullscreenMode;
        public bool IsTitleBarVisible { get { return lblTitleBar.Visible || panelZ.Visible; } set { SetTitleBarVisible(value); } }
        public bool IsControlsVisible { get { return panelControls.Visible; } set { SetPanelControlsVisible(value); } }
     

        public enum PlayerAction { Play, Pause, OpenFile, Fullscreen, OpenPlaylist, OpenSettings }

        //for dragging.
        private Point oldWindowPos;
        private Point oldMousePos;

        public delegate void ActivatedDelegate(object sender);
        public delegate void DragStartDelegate(object sender, int x, int y);
        public delegate void DragDragDelegate(object sender, int x, int y);
        public delegate void DragEndDelegate(object sender);
        public delegate void DropFilesDelegate(object sender, List<string> droppedFiles);
        public delegate void MinMaxDelegate(object sender, ref bool maximized);
        public delegate void ClosingDelegate(object sender);
        public delegate void ActionDelegate(object sender, PlayerAction triggeredAction, int errorCode);
        public delegate void TrackRenderedDelegate(object sender, int errorCode);
        public delegate void TrackFinishedDelegate(object sender);
        public delegate void TitleChangedDelegate(object sender, string title);
        public ActivatedDelegate Activated;
        public DragStartDelegate DragStart;
        public DragDragDelegate DragDrag;
        public DragEndDelegate DragEnd;
        public DropFilesDelegate DropFiles;
        public MinMaxDelegate MinMax;
        public ClosingDelegate Closing;
        public ActionDelegate ActionTriggered;
        public TrackRenderedDelegate TrackRendered;
        public TrackFinishedDelegate TrackFinished;
        public TitleChangedDelegate TitleChanged;


        public ZeratoolPlayerGui()
        {
            InitializeComponent();

            Disposed += OnDispose;

            OnCreate();
        }

        private void OnDispose(object sender, EventArgs e)
        {
            timerTrack.Enabled = false;
            timerSystemTime.Enabled = false;
            if (_playerEngine != null)
            {
                _playerEngine.Clear();
                _playerEngine = null;
            }
            System.Diagnostics.Debug.WriteLine($"Player {Title} disposed");
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }

        private void OnCreate()
        {
            _title = lblTitleBar.Text;

            _playerEngine = new ZeratoolPlayerEngine();
            _playerEngine.GraphMode = PrefferedGraphMode;
            _playlist = new ZeratoolPlaylist(_playerEngine);

            Playlist.IndexChanged += (s, index) =>
            {
                if (index >= 0)
                {
                    ZeratoolPlaylist zeratoolPlaylist = s as ZeratoolPlaylist;
                    PlayerEngine.FileName = zeratoolPlaylist[zeratoolPlaylist.PlayingIndex];
                    Title = Path.GetFileName(PlayerEngine.FileName);
                }
                else
                {
                    Title = "<No name>";
                }
            };

            PlayerEngine.Cleared += (s) =>
            {
                timerTrack.Enabled = false;
                btnPlay.BackgroundImage = Resources.play_inactive.ToBitmap();
                btnPlay.Refresh(); 
                btnPause.BackgroundImage = Resources.pause_inactive.ToBitmap();
                btnPause.Refresh();
                
                panelVideoScreen.Refresh();
                volumeBar.Refresh();
                seekBar.Refresh();
                UpdateTrackPositionIndicator();
            };

            PlayerEngine.TrackRendered += (s, errorCode) =>
            {
                if (errorCode == S_OK)
                {
                    ResizeOutputWindow();
                    btnPlay.BackgroundImage = Resources.play_active.ToBitmap();
                    timerTrack.Enabled = true;
                }
                UpdateTrackPositionIndicator();
                volumeBar.Refresh();
                seekBar.Refresh();
                Title = Path.GetFileName((s as ZeratoolPlayerEngine).FileName);
                TrackRendered?.Invoke(this, errorCode);
            };

            PlayerEngine.TrackFinished += (s) =>
            {
                if (TrackFinished != null)
                {
                    TrackFinished.Invoke(this);
                }
                else
                {
                    Pause();
                }
            };
            
            PlayerEngine.VideoOutputWindow = panelVideoScreen;

            timerSystemTime.Enabled = true;
        }

        private void ZeratoolPlayerGui_Load(object sender, EventArgs e)
        {
            SetDoubleBuffered(volumeBar, true);
            SetDoubleBuffered(seekBar, true);

            panelZ.BringToFront();
            lblTitleBar.BringToFront();
            panelMaxClose.BringToFront();
            panelControls.BringToFront();

            UpdateSystemTimeIndicator();

            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddPolygon(new Point[] {
                new Point(0, panelCorner.Height),
                new Point(panelCorner.Width, panelCorner.Height),
                new Point(panelCorner.Width, 0)
            });
            panelCorner.Region = new Region(graphicsPath);
            graphicsPath.Dispose();
        }

        public int Play()
        {
            if (State == PlayerState.Null)
            {
                PlayerEngine.GraphMode = PrefferedGraphMode;
            }

            int res = PlayerEngine.Play();
            
            btnPlay.BackgroundImage = res == S_OK ? Resources.play_active.ToBitmap() : Resources.play_inactive.ToBitmap();
            btnPause.BackgroundImage = Resources.pause_inactive.ToBitmap();

            ActionTriggered?.Invoke(this, PlayerAction.Play, res);
            return res;
        }

        public int Play(int playlistIndex)
        {
            Clear();
            if (Playlist.Count > 0)
            {
                Playlist.SetIndex(playlistIndex);
                string fn = Playlist[playlistIndex];
                PlayerEngine.FileName = fn;
                Title = Path.GetFileName(fn);
                return Play();
            }
            return S_FALSE;
        }

        public bool Pause()
        {
            if (State == PlayerState.Paused)
            {
                ActionTriggered?.Invoke(this, PlayerAction.Pause, S_OK);
                return true;
            }
            bool res = PlayerEngine.Pause();
            if (res)
            {
                btnPlay.BackgroundImage = Resources.play_inactive.ToBitmap();
                btnPause.BackgroundImage = Resources.pause_active.ToBitmap();

                seekBar.Refresh();
                UpdateTrackPositionIndicator();
            }
            ActionTriggered?.Invoke(this, PlayerAction.Pause, res ? S_OK : S_FALSE);
            return res;
        }

        public bool Stop()
        {
            return PlayerEngine.Stop();
        }

        public void Seek(double seconds)
        {
            TrackPosition += seconds;
        }

        public void Clear()
        {
            PlayerEngine?.Clear();
        }

        private void panelVideoScreen_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch (State)
                    {
                        case PlayerState.Playing:
                            Pause();
                            break;

                        case PlayerState.Paused:
                        case PlayerState.Stopped:
                            Play();
                            break;
                    }
                    break;

                case MouseButtons.Middle:
                    ToggleFullscreenMode();
                    break;

                case MouseButtons.Right:
                    if (e.Y < panelVideoScreen.Height / 2)
                    {
                        SetTitleBarVisible(!IsTitleBarVisible);
                    }
                    else
                    {
                        SetPanelControlsVisible(!IsControlsVisible);
                    }

                    ResizeOutputWindow();
                    break;
            }
        }

        private void panelVideoScreen_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (State == PlayerState.Null)
                {
                    Play();
                }
                else
                {
                    ToggleFullscreenMode();
                }
            }
        }

        private void lblTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            if (!IsMaximized && e.Button == MouseButtons.Left)
            {
                lblTitleBar.Cursor = Cursors.SizeAll;
            }
            oldWindowPos = new Point(e.X, e.Y);
            DragStart?.Invoke(this, e.X, e.Y);
        }

        private void lblTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMaximized && e.Button == MouseButtons.Left)
            {
                if (DragDrag != null)
                {
                    DragDrag.Invoke(this, e.X, e.Y);
                }
                else
                {
                    int x = Clamp(Left + e.X - oldWindowPos.X, 0, Parent.Width - 16 - Width);
                    int y = Clamp(Top + e.Y - oldWindowPos.Y, 0, Parent.Height - 38 - Height);
                    Location = new Point(x, y);
                }
            }
        }

        private void lblTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lblTitleBar.Cursor = Cursors.Default;
            }
            DragEnd?.Invoke(this);
        }

        private void lblTitleBar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!IsFullscreen && e.Button == MouseButtons.Left && MinMax != null)
            {
                _isMaximized = !_isMaximized;
                MinMax.Invoke(this, ref _isMaximized);
                AfterMinMax();
            }
        }

        private void panelVideoScreen_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void panelVideoScreen_DragDrop(object sender, DragEventArgs e)
        {
            if (DropFiles != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] strings = (string[])e.Data.GetData(DataFormats.FileDrop);
                    List<string> list = new List<string>();
                    list.AddRange(strings);
                    DropFiles.Invoke(this, list);
                }
            }
        }

        public void Activate()
        {
            Activated?.Invoke(this);
        }

        public void SetTitle(string title)
        {
            string newTitle = string.IsNullOrEmpty(title) ? "<No name>" : title;
            if (_title != newTitle)
            {
                _title = newTitle;
                lblTitleBar.Text = newTitle;
                TitleChanged?.Invoke(this, newTitle);
            }
            lblTitleBar.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void SetTitleBarVisible(bool flag)
        {
            lblTitleBar.Visible = flag;
            panelZ.Visible = flag;
        }

        private void SetPanelControlsVisible(bool flag)
        {
            panelControls.Visible = flag;
            if (flag)
            {
                UpdateTrackIndicators();
                UpdateSystemTimeIndicator();
            }
        }

        public void ToggleFullscreenMode()
        {
            if (IsMaximized)
            {
                _isFullscreenMode = !_isFullscreenMode;
                ActionTriggered?.Invoke(this, PlayerAction.Fullscreen, 0);
                btnFullscreen.BackgroundImage = IsFullscreen ? Resources.fullscreen_exit.ToBitmap() : Resources.fullscreen.ToBitmap();
            }
        }

        private void SetGraphMode(DirectShowGraphMode graphMode)
        {
            if (State == PlayerState.Null)
            {
                PlayerEngine.GraphMode = graphMode;
            }
        }

        public int PlayFile(string fileName)
        {
            Clear();
            Playlist.Clear();
            Playlist.Add(fileName);
            Playlist.SetIndex(0);
            PlayerEngine.FileName = fileName;
            Title = Path.GetFileName(fileName);
            return Play();
        }

        private void SetTrackPosition(double position)
        {
            if (State != PlayerState.Null)
            {
                PlayerEngine.Position = position;
                if (IsControlsVisible)
                {
                    UpdateTrackIndicators();
                }
            }
        }

        private void UpdateTrackPositionIndicator()
        {
            if (State != PlayerState.Null)
            {
                string elapsedString = new DateTime(TimeSpan.FromSeconds(TrackPosition).Ticks).ToString("H:mm:ss");
                string durationString = new DateTime(TimeSpan.FromSeconds(TrackDuration).Ticks).ToString("H:mm:ss");
                lblTrackPosition.Text = $"{elapsedString} < {durationString}";
            }
            else
            {
                lblTrackPosition.Text = "0:00:00 | 0:00:00";
                lblTrackPosition.Refresh();
            }
        }

        public void SetTitleBarBackColor(Color colorBkg)
        {
            lblTitleBar.BackColor = colorBkg;
            panelZ.BackColor = colorBkg;
            panelMaxClose.BackColor = colorBkg;
        }

        private void UpdateTrackIndicators()
        {
            seekBar.Refresh();
            UpdateTrackPositionIndicator();
        }

        private void UpdateSystemTimeIndicator()
        {
            string t = DateTime.Now.ToString("HH:mm:ss");
            lblSystemTime.Text = t;
        }

        public void ResizeOutputWindow()
        {
            panelVideoScreen.Top = lblTitleBar.Visible ? lblTitleBar.Height : 0;
            panelVideoScreen.Height = panelControls.Visible ? panelControls.Top - panelVideoScreen.Top : Height - panelVideoScreen.Top;
            if (State != PlayerState.Null)
            {
                Size size = PlayerEngine.VideoSize;
                Rectangle videoRect = new Rectangle(0, 0, size.Width, size.Height);
                Rectangle r = CenterRect(ResizeRect(videoRect, panelVideoScreen.ClientSize), panelVideoScreen.ClientRectangle);
                PlayerEngine.SetScreenRect(r);
            }
        }

        private void AfterMinMax()
        {
            if (IsMaximized)
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
                panelCorner.Visible = false;
                btnSettings.Left = Width - btnSettings.Width - 4;
                toolTip1.SetToolTip(btnMinMax, "Уменьшить");
            }
            else
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top;
                panelCorner.Visible = true;
                btnSettings.Left = panelCorner.Left - btnSettings.Width - 6;
                toolTip1.SetToolTip(btnMinMax, "Развернуть");
            }
            lblSystemTime.Left = btnSettings.Left - lblSystemTime.Width - 4;
        }

        public void Maximize()
        {
            _isMaximized = true;
            MinMax?.Invoke(this, ref _isMaximized);
            AfterMinMax();
        }

        private void ResizePlayer(int w, int h)
        {
            Width = w;
            Height = h;
            ResizeOutputWindow();
        }

        private void volumeBar_Paint(object sender, PaintEventArgs e)
        {
            Brush brush = new SolidBrush(volumeBar.BackColor);
            e.Graphics.FillRectangle(brush, volumeBar.ClientRectangle);
            brush.Dispose();
            if (Volume > 0)
            {
                int x1 = (int)(volumeBar.Width / 100.0 * Volume);
                Rectangle r = new Rectangle(0, 0, x1, volumeBar.Height);
                e.Graphics.FillRectangle(_playerEngine.AudioRendered ? Brushes.Lime : Brushes.LightGray, r);
            }

            string t = $"Volume: {Volume}%";

            Font fnt = new Font("Tahoma", 10.0f);
            SizeF size = e.Graphics.MeasureString(t, fnt);

            int x = volumeBar.Width / 2 - (int)size.Width / 2;
            int y = volumeBar.Height / 2 - (int)size.Height / 2;
            e.Graphics.DrawString(t, fnt, Brushes.Black, x, y);

            fnt.Dispose();
        }

        private void volumeBar_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            if (e.Button == MouseButtons.Left)
            {
                Volume = (int)(100.0 / volumeBar.Width * e.X);
                volumeBar.Refresh();
            }
        }

        private void volumeBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Volume = (int)(100.0 / volumeBar.Width * e.X);
                volumeBar.Refresh();
            }
        }

        private void seekBar_Paint(object sender, PaintEventArgs e)
        {
            Brush brush = new SolidBrush(seekBar.BackColor);
            e.Graphics.FillRectangle(brush, seekBar.ClientRectangle);
            brush.Dispose();
            if (State != PlayerState.Null && TrackDuration > 0.0)
            {
                int x = (int)(seekBar.Width / TrackDuration * TrackPosition);
                Rectangle r = new Rectangle(0, 0, x, seekBar.Height);
                e.Graphics.FillRectangle(Brushes.Blue, r);

                string elapsedString = new DateTime(TimeSpan.FromSeconds(TrackPosition).Ticks).ToString("H:mm:ss");
                string remainingString = new DateTime(TimeSpan.FromSeconds(TrackDuration - TrackPosition).Ticks).ToString("H:mm:ss");

                Font fnt = new Font("Tahoma", 11.0f);
                SizeF size = e.Graphics.MeasureString(elapsedString, fnt);

                Rectangle thumbRect = new Rectangle(x - 3, 0, 6, seekBar.Height);
                e.Graphics.FillRectangle(Brushes.White, thumbRect);
                e.Graphics.DrawRectangle(Pens.Black, thumbRect);

                int y = (int)(seekBar.Height / 2 - size.Height / 2);
                e.Graphics.DrawString(elapsedString, fnt, Brushes.White, x - size.Width - 2, y);
                e.Graphics.DrawString(remainingString, fnt, Brushes.Black, x + 4, y);

                fnt.Dispose();

                e.Graphics.FillCircle(Brushes.Black, seekBar.Width / 2, seekBar.Height / 2, 6);               
            }
        }

        private void timerSystemTime_Tick(object sender, EventArgs e)
        {
            UpdateSystemTimeIndicator();
        }

        private void timerTrack_Tick(object sender, EventArgs e)
        {
            if (panelControls.Visible)
            {
                UpdateTrackIndicators();
            }
        }

        private void seekBar_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            if (e.Button == MouseButtons.Left && State != PlayerState.Null)
            {
                TrackPosition = TrackDuration / seekBar.Width * e.X;
            }
        }

        private void btnClose_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsMaximized && e.Button == MouseButtons.Left)
            {  
                if (e.X >= 0 && e.X <= btnClose.Width && e.Y >= 0 && e.Y <= btnClose.Height)
                {
                    Closing?.Invoke(this);
                    Dispose();
                }
            }
        }

        private void btnMinMax_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsFullscreen && e.Button == MouseButtons.Left && MinMax != null)
            {
                if (e.X >= 0 && e.X <= btnMinMax.Width && e.Y >= 0 && e.Y <= btnMinMax.Height)
                {
                    _isMaximized = !_isMaximized;
                    MinMax.Invoke(this, ref _isMaximized);
                    AfterMinMax();
                }
            }
        }

        private void panelControls_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
        }

        private void btnSettings_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.X >= 0 && e.X <= btnSettings.Width && e.Y >= 0 && e.Y <= btnSettings.Height)
            {
                ActionTriggered?.Invoke(this, PlayerAction.OpenSettings, 0);
            }
        }

        private void ZeratoolPlayerGui_Resize(object sender, EventArgs e)
        {
            if (panelControls.Visible)
            {
                UpdateTrackIndicators();
            }
            ResizeOutputWindow();
        }

        private void seekBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && State != PlayerState.Null)
            {
                double dur = TrackDuration;
                double pos = dur / seekBar.Width * e.X;
                if (pos > dur)
                {
                    pos = dur - 5.0;
                }
                if (pos < 0.0)
                {
                    pos = 0.0;
                }

                TrackPosition = pos;

                UpdateTrackIndicators();
            }
        }

        private void btnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            if (e.Button == MouseButtons.Left)
            {
                Play();
            }
        }

        private void btnPause_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            if (e.Button == MouseButtons.Left)
            {
                Pause();
            }
        }

        private void btnOpenFile_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            if (e.Button == MouseButtons.Left)
            {
                Pause();
                ActionTriggered?.Invoke(this, PlayerAction.OpenFile, 0);
            }
        }

        private void btnFullscreen_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            if (e.Button == MouseButtons.Left)
            {
                ToggleFullscreenMode();
            }
        }

        private void btnPlaylist_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            ActionTriggered?.Invoke(this, PlayerAction.OpenPlaylist, 0);
        }

        private void panelCorner_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMaximized && e.Button == MouseButtons.Left)
            {
                int w = Clamp(Width + e.X - oldMousePos.X, MIN_WIDTH, Parent.Width - Left - 16);
                int h = Clamp(Height + e.Y - oldMousePos.Y, MIN_HEIGHT, Parent.Height - Top - 36);
                ResizePlayer(w, h);
            }

        }

        private void panelCorner_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            oldMousePos = new Point(e.X, e.Y);
        }

        private void btnPreviousTrack_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            Playlist.PreviousTrack();
        }
        
        private void btnNextTrack_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
            Playlist.NextTrack();
        }

        private void panelZ_MouseDown(object sender, MouseEventArgs e)
        {
            Activate();
        }

    }
}
