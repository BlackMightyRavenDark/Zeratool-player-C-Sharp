
namespace Zeratool_player_C_Sharp
{
    partial class FormPlaylist
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlaylist));
            this.comboBoxPlayers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbPlaylist = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // comboBoxPlayers
            // 
            this.comboBoxPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPlayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlayers.FormattingEnabled = true;
            this.comboBoxPlayers.Location = new System.Drawing.Point(51, 3);
            this.comboBoxPlayers.Name = "comboBoxPlayers";
            this.comboBoxPlayers.Size = new System.Drawing.Size(606, 21);
            this.comboBoxPlayers.TabIndex = 0;
            this.comboBoxPlayers.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlayers_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Плеер:";
            // 
            // lbPlaylist
            // 
            this.lbPlaylist.AllowDrop = true;
            this.lbPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPlaylist.BackColor = System.Drawing.Color.Black;
            this.lbPlaylist.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbPlaylist.Font = new System.Drawing.Font("Lucida Console", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbPlaylist.ForeColor = System.Drawing.Color.Lime;
            this.lbPlaylist.FormattingEnabled = true;
            this.lbPlaylist.Location = new System.Drawing.Point(2, 30);
            this.lbPlaylist.Name = "lbPlaylist";
            this.lbPlaylist.Size = new System.Drawing.Size(655, 319);
            this.lbPlaylist.TabIndex = 2;
            this.lbPlaylist.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbPlaylist_DrawItem);
            this.lbPlaylist.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbPlaylist_MeasureItem);
            this.lbPlaylist.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbPlaylist_DragDrop);
            this.lbPlaylist.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbPlaylist_DragEnter);
            this.lbPlaylist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbPlaylist_MouseDoubleClick);
            // 
            // FormPlaylist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 350);
            this.Controls.Add(this.lbPlaylist);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxPlayers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "FormPlaylist";
            this.Text = "Плейлист";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPlaylist_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPlayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbPlaylist;
    }
}