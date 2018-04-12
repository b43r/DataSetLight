namespace deceed.DsLight.EditorGUI
{
    partial class DsLightEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mnuEmpty = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuEmptyNew = new System.Windows.Forms.ToolStripMenuItem();
            this.lblAdd = new System.Windows.Forms.Label();
            this.mnuEntity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuEntityRename = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEntityDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEntitySeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEntityAddQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEntityRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuery = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuQueryEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQueryRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQueryDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEmpty.SuspendLayout();
            this.mnuEntity.SuspendLayout();
            this.mnuQuery.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuEmpty
            // 
            this.mnuEmpty.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEmptyNew});
            this.mnuEmpty.Name = "contextMenuStrip1";
            this.mnuEmpty.Size = new System.Drawing.Size(176, 26);
            // 
            // mnuEmptyNew
            // 
            this.mnuEmptyNew.Name = "mnuEmptyNew";
            this.mnuEmptyNew.Size = new System.Drawing.Size(175, 22);
            this.mnuEmptyNew.Text = "Create new entity...";
            this.mnuEmptyNew.Click += new System.EventHandler(this.mnuEmptyNew_Click);
            // 
            // lblAdd
            // 
            this.lblAdd.AutoSize = true;
            this.lblAdd.BackColor = System.Drawing.Color.Transparent;
            this.lblAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdd.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lblAdd.Location = new System.Drawing.Point(115, 148);
            this.lblAdd.Name = "lblAdd";
            this.lblAdd.Size = new System.Drawing.Size(211, 24);
            this.lblAdd.TabIndex = 1;
            this.lblAdd.Text = "Click to add new entity...";
            this.lblAdd.Click += new System.EventHandler(this.lblAdd_Click);
            // 
            // mnuEntity
            // 
            this.mnuEntity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEntityRename,
            this.mnuEntityDelete,
            this.mnuEntitySeparator,
            this.mnuEntityAddQuery,
            this.mnuEntityRefresh});
            this.mnuEntity.Name = "mnuEntity";
            this.mnuEntity.Size = new System.Drawing.Size(187, 98);
            // 
            // mnuEntityRename
            // 
            this.mnuEntityRename.Name = "mnuEntityRename";
            this.mnuEntityRename.Size = new System.Drawing.Size(186, 22);
            this.mnuEntityRename.Text = "Rename entity \"{0}\"...";
            this.mnuEntityRename.Click += new System.EventHandler(this.mnuEntityRename_Click);
            // 
            // mnuEntityDelete
            // 
            this.mnuEntityDelete.Name = "mnuEntityDelete";
            this.mnuEntityDelete.Size = new System.Drawing.Size(186, 22);
            this.mnuEntityDelete.Text = "Delete entity \"{0}\"";
            this.mnuEntityDelete.Click += new System.EventHandler(this.mnuEntityDelete_Click);
            // 
            // mnuEntitySeparator
            // 
            this.mnuEntitySeparator.Name = "mnuEntitySeparator";
            this.mnuEntitySeparator.Size = new System.Drawing.Size(183, 6);
            // 
            // mnuEntityAddQuery
            // 
            this.mnuEntityAddQuery.Name = "mnuEntityAddQuery";
            this.mnuEntityAddQuery.Size = new System.Drawing.Size(186, 22);
            this.mnuEntityAddQuery.Text = "Add query...";
            this.mnuEntityAddQuery.Click += new System.EventHandler(this.mnuEntityAddQuery_Click);
            // 
            // mnuEntityRefresh
            // 
            this.mnuEntityRefresh.Name = "mnuEntityRefresh";
            this.mnuEntityRefresh.Size = new System.Drawing.Size(186, 22);
            this.mnuEntityRefresh.Text = "Refresh all queries";
            this.mnuEntityRefresh.Click += new System.EventHandler(this.mnuEntityRefresh_Click);
            // 
            // mnuQuery
            // 
            this.mnuQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuQueryEdit,
            this.mnuQueryRefresh,
            this.mnuQueryDelete});
            this.mnuQuery.Name = "mnuQuery";
            this.mnuQuery.Size = new System.Drawing.Size(147, 70);
            // 
            // mnuQueryEdit
            // 
            this.mnuQueryEdit.Name = "mnuQueryEdit";
            this.mnuQueryEdit.Size = new System.Drawing.Size(146, 22);
            this.mnuQueryEdit.Text = "Edit query";
            this.mnuQueryEdit.Click += new System.EventHandler(this.mnuQueryEdit_Click);
            // 
            // mnuQueryRefresh
            // 
            this.mnuQueryRefresh.Name = "mnuQueryRefresh";
            this.mnuQueryRefresh.Size = new System.Drawing.Size(146, 22);
            this.mnuQueryRefresh.Text = "Refresh query";
            this.mnuQueryRefresh.Click += new System.EventHandler(this.mnuQueryRefresh_Click);
            // 
            // mnuQueryDelete
            // 
            this.mnuQueryDelete.Name = "mnuQueryDelete";
            this.mnuQueryDelete.Size = new System.Drawing.Size(146, 22);
            this.mnuQueryDelete.Text = "Delete query";
            this.mnuQueryDelete.Click += new System.EventHandler(this.mnuQueryDelete_Click);
            // 
            // DsLightEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lblAdd);
            this.DoubleBuffered = true;
            this.Name = "DsLightEditor";
            this.Size = new System.Drawing.Size(441, 296);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DsLightEditor_Scroll);
            this.SizeChanged += new System.EventHandler(this.DsLightEditor_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DsLightEditor_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DsLightEditor_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DsLightEditor_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DsLightEditor_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DsLightEditor_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DsLightEditor_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DsLightEditor_MouseUp);
            this.Resize += new System.EventHandler(this.DsLightEditor_Resize);
            this.mnuEmpty.ResumeLayout(false);
            this.mnuEntity.ResumeLayout(false);
            this.mnuQuery.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuEmpty;
        private System.Windows.Forms.ToolStripMenuItem mnuEmptyNew;
        private System.Windows.Forms.Label lblAdd;
        private System.Windows.Forms.ContextMenuStrip mnuEntity;
        private System.Windows.Forms.ToolStripMenuItem mnuEntityDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuEntityRefresh;
        private System.Windows.Forms.ContextMenuStrip mnuQuery;
        private System.Windows.Forms.ToolStripMenuItem mnuQueryDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuQueryRefresh;
        private System.Windows.Forms.ToolStripMenuItem mnuQueryEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuEntityRename;
        private System.Windows.Forms.ToolStripMenuItem mnuEntityAddQuery;
        private System.Windows.Forms.ToolStripSeparator mnuEntitySeparator;



    }
}
