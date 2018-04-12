namespace deceed.DsLight.EditorGUI
{
    partial class CompatibilityError
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.lblText = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(295, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnReplace
            // 
            this.btnReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReplace.Location = new System.Drawing.Point(171, 263);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(118, 23);
            this.btnReplace.TabIndex = 1;
            this.btnReplace.Text = "Replace columns";
            this.btnReplace.UseVisualStyleBackColor = true;
            // 
            // lblText
            // 
            this.lblText.Location = new System.Drawing.Point(12, 9);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(358, 39);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "label1";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 51);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(355, 160);
            this.listBox1.TabIndex = 3;
            // 
            // lblQuestion
            // 
            this.lblQuestion.Location = new System.Drawing.Point(12, 214);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(358, 39);
            this.lblQuestion.TabIndex = 4;
            this.lblQuestion.Text = "Do you want to add the query and replace all columns of the entity or cancel the " +
    "action?";
            // 
            // CompatibilityError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(382, 298);
            this.ControlBox = false;
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CompatibilityError";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New query is not compatible";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label lblQuestion;
    }
}