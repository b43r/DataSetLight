namespace deceed.DsLight.Test
{
    partial class Form1
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
            this.DsLightEditor = new deceed.DsLight.EditorGUI.DsLightEditor();
            this.SuspendLayout();
            // 
            // DsLightEditor
            // 
            this.DsLightEditor.AutoScroll = true;
            this.DsLightEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DsLightEditor.Location = new System.Drawing.Point(0, 0);
            this.DsLightEditor.Name = "DsLightEditor";
            this.DsLightEditor.Size = new System.Drawing.Size(664, 449);
            this.DsLightEditor.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(664, 449);
            this.Controls.Add(this.DsLightEditor);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private deceed.DsLight.EditorGUI.DsLightEditor DsLightEditor;
    }
}

