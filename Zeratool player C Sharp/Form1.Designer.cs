
namespace Zeratool_player_C_Sharp
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.player = new Zeratool_player_C_Sharp.ZeratoolPlayerGui();
            this.SuspendLayout();
            // 
            // player
            // 
            this.player.GraphMode = Zeratool_player_C_Sharp.ZeratoolPlayerEngine.GRAPH_MODE.Manual;
            this.player.IsControlsVisible = true;
            this.player.IsTitleBarVisible = true;
            this.player.Location = new System.Drawing.Point(35, 11);
            this.player.Name = "player";
            this.player.PrefferedGraphMode = Zeratool_player_C_Sharp.ZeratoolPlayerEngine.GRAPH_MODE.Manual;
            this.player.Size = new System.Drawing.Size(386, 276);
            this.player.TabIndex = 0;
            this.player.Title = "No name";
            this.player.TrackPosition = 0D;
            this.player.Volume = 25;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.player);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Zeratool player";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ZeratoolPlayerGui player;
    }
}

