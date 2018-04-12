namespace deceed.DsLight.EditorGUI
{
    partial class AddQuery
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
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.gbType = new System.Windows.Forms.GroupBox();
            this.rbNonQuery = new System.Windows.Forms.RadioButton();
            this.rbScalar = new System.Windows.Forms.RadioButton();
            this.rbReader = new System.Windows.Forms.RadioButton();
            this.gbSource = new System.Windows.Forms.GroupBox();
            this.rbSQL = new System.Windows.Forms.RadioButton();
            this.rbStoredProc = new System.Windows.Forms.RadioButton();
            this.gbName = new System.Windows.Forms.GroupBox();
            this.lblDuplicateName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.gbSQL = new System.Windows.Forms.GroupBox();
            this.txtSQL = new System.Windows.Forms.TextBox();
            this.gbSP = new System.Windows.Forms.GroupBox();
            this.cboSPs = new System.Windows.Forms.ComboBox();
            this.gbType.SuspendLayout();
            this.gbSource.SuspendLayout();
            this.gbName.SuspendLayout();
            this.gbSQL.SuspendLayout();
            this.gbSP.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(221, 298);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(302, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevious.Enabled = false;
            this.btnPrevious.Location = new System.Drawing.Point(140, 298);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 2;
            this.btnPrevious.Text = "< Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // gbType
            // 
            this.gbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbType.Controls.Add(this.rbNonQuery);
            this.gbType.Controls.Add(this.rbScalar);
            this.gbType.Controls.Add(this.rbReader);
            this.gbType.Location = new System.Drawing.Point(12, 12);
            this.gbType.Name = "gbType";
            this.gbType.Size = new System.Drawing.Size(366, 115);
            this.gbType.TabIndex = 4;
            this.gbType.TabStop = false;
            this.gbType.Text = "What type of query do you want to add?";
            // 
            // rbNonQuery
            // 
            this.rbNonQuery.AutoSize = true;
            this.rbNonQuery.Location = new System.Drawing.Point(16, 75);
            this.rbNonQuery.Name = "rbNonQuery";
            this.rbNonQuery.Size = new System.Drawing.Size(155, 17);
            this.rbNonQuery.TabIndex = 5;
            this.rbNonQuery.Text = "A query that returns nothing";
            this.rbNonQuery.UseVisualStyleBackColor = true;
            // 
            // rbScalar
            // 
            this.rbScalar.AutoSize = true;
            this.rbScalar.Location = new System.Drawing.Point(16, 52);
            this.rbScalar.Name = "rbScalar";
            this.rbScalar.Size = new System.Drawing.Size(185, 17);
            this.rbScalar.TabIndex = 4;
            this.rbScalar.Text = "A query that returns a single value";
            this.rbScalar.UseVisualStyleBackColor = true;
            // 
            // rbReader
            // 
            this.rbReader.AutoSize = true;
            this.rbReader.Checked = true;
            this.rbReader.Location = new System.Drawing.Point(16, 29);
            this.rbReader.Name = "rbReader";
            this.rbReader.Size = new System.Drawing.Size(141, 17);
            this.rbReader.TabIndex = 3;
            this.rbReader.TabStop = true;
            this.rbReader.Text = "A query that returns data";
            this.rbReader.UseVisualStyleBackColor = true;
            // 
            // gbSource
            // 
            this.gbSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSource.Controls.Add(this.rbSQL);
            this.gbSource.Controls.Add(this.rbStoredProc);
            this.gbSource.Location = new System.Drawing.Point(12, 143);
            this.gbSource.Name = "gbSource";
            this.gbSource.Size = new System.Drawing.Size(366, 87);
            this.gbSource.TabIndex = 5;
            this.gbSource.TabStop = false;
            this.gbSource.Text = "Where does the query come from?";
            // 
            // rbSQL
            // 
            this.rbSQL.AutoSize = true;
            this.rbSQL.Location = new System.Drawing.Point(16, 26);
            this.rbSQL.Name = "rbSQL";
            this.rbSQL.Size = new System.Drawing.Size(160, 17);
            this.rbSQL.TabIndex = 1;
            this.rbSQL.Text = "I will enter an SQL statement";
            this.rbSQL.UseVisualStyleBackColor = true;
            // 
            // rbStoredProc
            // 
            this.rbStoredProc.AutoSize = true;
            this.rbStoredProc.Checked = true;
            this.rbStoredProc.Location = new System.Drawing.Point(16, 49);
            this.rbStoredProc.Name = "rbStoredProc";
            this.rbStoredProc.Size = new System.Drawing.Size(184, 17);
            this.rbStoredProc.TabIndex = 0;
            this.rbStoredProc.TabStop = true;
            this.rbStoredProc.Text = "From an existing stored procedure";
            this.rbStoredProc.UseVisualStyleBackColor = true;
            // 
            // gbName
            // 
            this.gbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbName.Controls.Add(this.lblDuplicateName);
            this.gbName.Controls.Add(this.txtName);
            this.gbName.Location = new System.Drawing.Point(12, 104);
            this.gbName.Name = "gbName";
            this.gbName.Size = new System.Drawing.Size(366, 82);
            this.gbName.TabIndex = 6;
            this.gbName.TabStop = false;
            this.gbName.Text = "Query name:";
            this.gbName.Visible = false;
            // 
            // lblDuplicateName
            // 
            this.lblDuplicateName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDuplicateName.BackColor = System.Drawing.Color.LightSalmon;
            this.lblDuplicateName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuplicateName.Location = new System.Drawing.Point(13, 49);
            this.lblDuplicateName.Name = "lblDuplicateName";
            this.lblDuplicateName.Padding = new System.Windows.Forms.Padding(3);
            this.lblDuplicateName.Size = new System.Drawing.Size(332, 23);
            this.lblDuplicateName.TabIndex = 1;
            this.lblDuplicateName.Text = "A query with this name already exists.";
            this.lblDuplicateName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDuplicateName.Visible = false;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(16, 26);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(329, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            // 
            // gbSQL
            // 
            this.gbSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSQL.Controls.Add(this.txtSQL);
            this.gbSQL.Location = new System.Drawing.Point(12, 104);
            this.gbSQL.Name = "gbSQL";
            this.gbSQL.Size = new System.Drawing.Size(365, 178);
            this.gbSQL.TabIndex = 6;
            this.gbSQL.TabStop = false;
            this.gbSQL.Text = "SQL query:";
            this.gbSQL.Visible = false;
            // 
            // txtSQL
            // 
            this.txtSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQL.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQL.Location = new System.Drawing.Point(11, 22);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSQL.Size = new System.Drawing.Size(348, 143);
            this.txtSQL.TabIndex = 0;
            this.txtSQL.TextChanged += new System.EventHandler(this.txtSQL_TextChanged);
            this.txtSQL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSQL_KeyDown);
            // 
            // gbSP
            // 
            this.gbSP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSP.Controls.Add(this.cboSPs);
            this.gbSP.Location = new System.Drawing.Point(12, 12);
            this.gbSP.Name = "gbSP";
            this.gbSP.Size = new System.Drawing.Size(366, 86);
            this.gbSP.TabIndex = 7;
            this.gbSP.TabStop = false;
            this.gbSP.Text = "Stored procedure:";
            this.gbSP.Visible = false;
            // 
            // cboSPs
            // 
            this.cboSPs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSPs.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSPs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSPs.Location = new System.Drawing.Point(16, 29);
            this.cboSPs.Name = "cboSPs";
            this.cboSPs.Size = new System.Drawing.Size(329, 21);
            this.cboSPs.TabIndex = 0;
            this.cboSPs.SelectedIndexChanged += new System.EventHandler(this.cboSPs_SelectedIndexChanged);
            // 
            // AddQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(390, 333);
            this.ControlBox = false;
            this.Controls.Add(this.gbName);
            this.Controls.Add(this.gbSP);
            this.Controls.Add(this.gbSQL);
            this.Controls.Add(this.gbSource);
            this.Controls.Add(this.gbType);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(406, 329);
            this.Name = "AddQuery";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add query";
            this.gbType.ResumeLayout(false);
            this.gbType.PerformLayout();
            this.gbSource.ResumeLayout(false);
            this.gbSource.PerformLayout();
            this.gbName.ResumeLayout(false);
            this.gbName.PerformLayout();
            this.gbSQL.ResumeLayout(false);
            this.gbSQL.PerformLayout();
            this.gbSP.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.GroupBox gbType;
        private System.Windows.Forms.RadioButton rbNonQuery;
        private System.Windows.Forms.RadioButton rbScalar;
        private System.Windows.Forms.RadioButton rbReader;
        private System.Windows.Forms.GroupBox gbSource;
        private System.Windows.Forms.RadioButton rbSQL;
        private System.Windows.Forms.RadioButton rbStoredProc;
        private System.Windows.Forms.GroupBox gbSQL;
        private System.Windows.Forms.TextBox txtSQL;
        private System.Windows.Forms.GroupBox gbName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.GroupBox gbSP;
        private System.Windows.Forms.ComboBox cboSPs;
        private System.Windows.Forms.Label lblDuplicateName;
    }
}