
namespace Zeratool_player_C_Sharp
{
    partial class FormBookmarks
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
            this.listViewBookmarks = new System.Windows.Forms.ListView();
            this.columnHeaderBookmarkTimecode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBookmarkTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddBookmark = new System.Windows.Forms.Button();
            this.comboBoxPlayers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewBookmarks
            // 
            this.listViewBookmarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewBookmarks.BackColor = System.Drawing.Color.Black;
            this.listViewBookmarks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderBookmarkTimecode,
            this.columnHeaderBookmarkTitle});
            this.listViewBookmarks.ForeColor = System.Drawing.Color.Lime;
            this.listViewBookmarks.FullRowSelect = true;
            this.listViewBookmarks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewBookmarks.HideSelection = false;
            this.listViewBookmarks.Location = new System.Drawing.Point(8, 33);
            this.listViewBookmarks.MultiSelect = false;
            this.listViewBookmarks.Name = "listViewBookmarks";
            this.listViewBookmarks.Size = new System.Drawing.Size(394, 122);
            this.listViewBookmarks.TabIndex = 0;
            this.listViewBookmarks.UseCompatibleStateImageBehavior = false;
            this.listViewBookmarks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderBookmarkTimecode
            // 
            this.columnHeaderBookmarkTimecode.Text = "Время";
            this.columnHeaderBookmarkTimecode.Width = 100;
            // 
            // columnHeaderBookmarkTitle
            // 
            this.columnHeaderBookmarkTitle.Text = "Название";
            this.columnHeaderBookmarkTitle.Width = 304;
            // 
            // btnAddBookmark
            // 
            this.btnAddBookmark.Location = new System.Drawing.Point(8, 163);
            this.btnAddBookmark.Name = "btnAddBookmark";
            this.btnAddBookmark.Size = new System.Drawing.Size(75, 23);
            this.btnAddBookmark.TabIndex = 1;
            this.btnAddBookmark.Text = "Добавить";
            this.btnAddBookmark.UseVisualStyleBackColor = true;
            // 
            // comboBoxPlayers
            // 
            this.comboBoxPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPlayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlayers.FormattingEnabled = true;
            this.comboBoxPlayers.Location = new System.Drawing.Point(53, 6);
            this.comboBoxPlayers.Name = "comboBoxPlayers";
            this.comboBoxPlayers.Size = new System.Drawing.Size(349, 21);
            this.comboBoxPlayers.TabIndex = 2;
            this.comboBoxPlayers.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlayers_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Плеер:";
            // 
            // FormBookmarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(414, 191);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxPlayers);
            this.Controls.Add(this.btnAddBookmark);
            this.Controls.Add(this.listViewBookmarks);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MinimumSize = new System.Drawing.Size(430, 230);
            this.Name = "FormBookmarks";
            this.Text = "Отметины";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBookmarks_FormClosing);
            this.Load += new System.EventHandler(this.FormBookmarks_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewBookmarks;
        private System.Windows.Forms.ColumnHeader columnHeaderBookmarkTimecode;
        private System.Windows.Forms.ColumnHeader columnHeaderBookmarkTitle;
        private System.Windows.Forms.Button btnAddBookmark;
        private System.Windows.Forms.ComboBox comboBoxPlayers;
        private System.Windows.Forms.Label label1;
    }
}