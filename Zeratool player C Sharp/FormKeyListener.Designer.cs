
namespace Zeratool_player_C_Sharp
{
    partial class FormKeyListener
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormKeyListener));
            this.lblKeyCombination = new System.Windows.Forms.Label();
            this.lblKeyActionTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblKeyCombination
            // 
            this.lblKeyCombination.AutoSize = true;
            this.lblKeyCombination.Location = new System.Drawing.Point(12, 30);
            this.lblKeyCombination.Name = "lblKeyCombination";
            this.lblKeyCombination.Size = new System.Drawing.Size(93, 13);
            this.lblKeyCombination.TabIndex = 0;
            this.lblKeyCombination.Text = "lblKeyCombination";
            // 
            // lblKeyActionTitle
            // 
            this.lblKeyActionTitle.AutoSize = true;
            this.lblKeyActionTitle.Location = new System.Drawing.Point(12, 9);
            this.lblKeyActionTitle.Name = "lblKeyActionTitle";
            this.lblKeyActionTitle.Size = new System.Drawing.Size(85, 13);
            this.lblKeyActionTitle.TabIndex = 1;
            this.lblKeyActionTitle.Text = "lblKeyActionTitle";
            // 
            // FormKeyListener
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 55);
            this.Controls.Add(this.lblKeyActionTitle);
            this.Controls.Add(this.lblKeyCombination);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormKeyListener";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Изменение клавиши";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormKeyListener_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKeyCombination;
        private System.Windows.Forms.Label lblKeyActionTitle;
    }
}