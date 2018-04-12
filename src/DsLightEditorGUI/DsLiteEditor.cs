/*
 * DsLight
 * 
 * Copyright (c) 2014..2018 by Simon Baer
 * 
 * This program is free software; you can redistribute it and/or modify it under the terms
 * of the GNU General Public License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with this program;
 * If not, see http://www.gnu.org/licenses/.
 *
 */

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using deceed.DsLight.EditorGUI.Model;

namespace deceed.DsLight.EditorGUI
{
    public partial class DsLightEditor : UserControl
    {
        internal DataModel dataModel;

        private Point mouseDownLocation;
        private Entity selectedEntity = null;
        private Query selectedQuery = null;
        private Entity draggingEntity = null;
        private int draggingXOffs = 0;
        private int draggingYOffs = 0;
        private Rectangle moveRect;

        private List<Query> okQueryList = new List<Query>();
        private Timer okQueryTimer = new Timer();

        public event EventHandler Modified;
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        private IConnectionString connectionStringService;

        /// <summary>
        /// Initialize a new instance.
        /// </summary>
        public DsLightEditor()
            : this(null)
        {
        }

        /// <summary>
        /// Initialize a new instance.
        /// </summary>
        /// <param name="connectionStringService">IConnectionString interface</param>
        public DsLightEditor(IConnectionString connectionStringService)
        {
            InitializeComponent();

            this.connectionStringService = connectionStringService;
            dataModel = new DataModel(this);

            okQueryTimer.Interval = 1000;
            okQueryTimer.Tick += okQueryTimer_Tick;
        }

        /// <summary>
        /// Gets the IConnectionString interface.
        /// </summary>
        public IConnectionString ConnectionStringService
        {
            get { return connectionStringService; }
        }

        /// <summary>
        /// Optionally a connection string can be set. If no connection string is set using this method,
        /// the connection string is retrieved from the App.config or Web.config file.
        /// </summary>
        /// <param name="connectionString">connection string</param>
        public void SetConnectionString(string connectionString)
        {
            dataModel.ConnectionStringOverride = connectionString;
        }

        #region DsLightEditor events

        /// <summary>
        /// Redraw all entities.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_Paint(object sender, PaintEventArgs e)
        {
            // draw background
            using (HatchBrush crossBrush = new HatchBrush(HatchStyle.DarkHorizontal, Color.FromArgb(219, 224, 229), Color.FromArgb(230, 235, 237)))
            {
                e.Graphics.FillRectangle(crossBrush, new Rectangle(0, 0, Width, Height));
            }
            
            // draw entities
            int maxWidth = 0;
            int maxHeight = 0;
            foreach (Entity entity in dataModel.Entities)
            {
                entity.Draw(e.Graphics, AutoScrollPosition);
                if (entity.X + entity.Width > maxWidth)
                {
                    maxWidth = entity.X + entity.Width;
                }
                if (entity.Y + entity.Height > maxHeight)
                {
                    maxHeight = entity.Y + entity.Height;
                }
            }
            AutoScrollMinSize = new Size(maxWidth, maxHeight);
        }

        /// <summary>
        /// Select the entity or query below the mouse when a mouse-button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_MouseDown(object sender, MouseEventArgs e)
        {
            // unselect entity/query
            UnselectAll();

            Point p = new Point(e.X - AutoScrollPosition.X, e.Y - AutoScrollPosition.Y);

            Entity entity = EntityAtPoint(p);
            if (entity != null)
            {
                entity.IsSelected = true;
                selectedEntity = entity;

                // make the selected entity the current one
                dataModel.Entities.Remove(selectedEntity);
                dataModel.Entities.Add(selectedEntity);

                draggingXOffs = p.X - entity.X;
                draggingYOffs = p.Y - entity.Y;

                selectedQuery = entity.GetQueryAtPosition(draggingYOffs);
                if (selectedQuery != null)
                {
                    selectedQuery.IsSelected = true;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (draggingYOffs < 20)
                    {
                        draggingEntity = entity;
                    }
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                mouseDownLocation = p; // new Point(e.X, e.Y);

                if (selectedQuery != null)
                {
                    mnuQuery.Show(this, mouseDownLocation);
                }
                else if (selectedEntity != null)
                {
                    mnuEntityDelete.Text = String.Format("Delete entity \"{0}\"", selectedEntity.Name);
                    mnuEntityRename.Text = String.Format("Rename entity \"{0}\"...", selectedEntity.Name);
                    mnuEntity.Show(this, mouseDownLocation);
                }
                else
                {
                    mnuEmpty.Show(this, mouseDownLocation);
                }
            }

            Refresh();
            OnSelectionChanged();
        }

        /// <summary>
        /// Move an entity that has been dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_MouseUp(object sender, MouseEventArgs e)
        {
            if (!moveRect.IsEmpty)
            {
                ControlPaint.DrawReversibleFrame(moveRect, Color.Black, FrameStyle.Dashed);
                moveRect = Rectangle.Empty;
            }

            if (draggingEntity != null)
            {
                Point p = new Point(e.X - AutoScrollPosition.X, e.Y - AutoScrollPosition.Y);
                draggingEntity.X = p.X - draggingXOffs;
                draggingEntity.Y = p.Y - draggingYOffs;
                if (draggingEntity.X < 0)
                {
                    draggingEntity.X = 0;
                }
                if (draggingEntity.Y < 0)
                {
                    draggingEntity.Y = 0;
                }
                OnModifiedWithRefresh();
            }

            draggingEntity = null;
        }

        /// <summary>
        /// Handle dragging of an entity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button == System.Windows.Forms.MouseButtons.Left) && (draggingEntity != null))
            {
                if (!moveRect.IsEmpty)
                {
                    ControlPaint.DrawReversibleFrame(moveRect, Color.Black, FrameStyle.Dashed);
                }
                moveRect = RectangleToScreen(new Rectangle(e.X - draggingXOffs, e.Y - draggingYOffs, draggingEntity.Width, draggingEntity.Height));
                ControlPaint.DrawReversibleFrame(moveRect, Color.Black, FrameStyle.Dashed);
            }
        }

        /// <summary>
        /// A double-click on the entity opens the 'rename entity' dialog, a double-click on a query opens the 'edit query' dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point p = new Point(e.X - AutoScrollPosition.X, e.Y - AutoScrollPosition.Y);
                Entity entity = EntityAtPoint(p);
                if (entity != null)
                {
                    Query query = entity.GetQueryAtPosition(draggingYOffs);
                    if ((query != null) && (query == selectedQuery))
                    {
                        mnuQueryEdit_Click(sender, EventArgs.Empty);
                    }
                    else
                    {
                        mnuEntityRename_Click(sender, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Handle the mouse click on the "Add..." label on an entity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button == System.Windows.Forms.MouseButtons.Left) && (draggingEntity == null))
            {
                Point p = new Point(e.X - AutoScrollPosition.X, e.Y - AutoScrollPosition.Y);
                Entity entity = EntityAtPoint(p);
                if (entity != null)
                {
                    int yOffs = p.Y - entity.Y;
                    if (yOffs > entity.Height - 20)
                    {
                        AddNewQuery(entity);
                    }
                }
            }
        }

        /// <summary>
        /// Show the label to add new entities in the center of the work area.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_SizeChanged(object sender, EventArgs e)
        {
            lblAdd.Left = (Width - lblAdd.Width) / 2;
            lblAdd.Top = (Height - lblAdd.Height) / 2;
        }

        /// <summary>
        /// If the delete-key is pressed, the selected query or entity is deleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (selectedQuery != null)
                {
                    DeleteSelectedQuery();
                }
                else if (selectedEntity != null)
                {
                    DeleteSelectedEntity();
                }
            }
        }

        /// <summary>
        /// Invalidate client area when the control is scrolled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Invalidate the client area if the control is resized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DsLightEditor_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        #endregion

        /// <summary>
        /// This timer hides the 'ok' tick marks on queries that have been successfully refreshed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okQueryTimer_Tick(object sender, EventArgs e)
        {
            okQueryTimer.Stop();
            lock (((ICollection)okQueryList).SyncRoot)
            {
                foreach (Query query in okQueryList)
                {
                    query.ShowOk = false;
                }
                okQueryList.Clear();
            }
            Refresh();
        }

        /// <summary>
        /// Check the compatibility of the given metadata with the given entity and return
        /// a list of errors.
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="md">metadata</param>
        /// <param name="breakOnError">true to stop on first error</param>
        /// <returns>list of errors</returns>
        private List<string> CheckCompatibility(Entity entity, DB.Metadata md, bool breakOnError = false)
        {
            List<string> errorDetails = new List<string>();
            List<Property> propertyList = new List<Property>(entity.Properties);
            foreach (DB.Column column in md.Columns)
            {
                Property property = propertyList.FirstOrDefault(x => x.Name == column.Name);
                if (property == null)
                {
                    errorDetails.Add(String.Format("Additional column: {0} {1}", column.SysType, column.Name));
                    if (breakOnError)
                    {
                        break;
                    }
                }
                else if (property.TypeName != column.SysType)
                {
                    // wrong type
                    errorDetails.Add(String.Format("Column '{0}' is of type '{1}' instead of '{2}'.", column.Name, column.SysType, property.TypeName));
                    propertyList.Remove(property);
                    if (breakOnError)
                    {
                        break;
                    }
                }
                else
                {
                    // everything ok
                    propertyList.Remove(property);
                }
            }

            // all properties still in this list are missing
            foreach (Property property in propertyList)
            {
                errorDetails.Add(String.Format("Missing column: {0} {1}", property.TypeName, property.Name));
                if (breakOnError)
                {
                    break;
                }
            }

            return errorDetails;
        }

        /// <summary>
        /// Show the dialog for adding a new entity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblAdd_Click(object sender, EventArgs e)
        {
            mouseDownLocation = new Point(10, 10);
            AddNewEntity();
        }

        /// <summary>
        /// Returns the entity at the given mouse position.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Entity EntityAtPoint(Point p)
        {
            foreach (Entity entity in dataModel.Entities)
            {
                if (p.X > entity.X && p.X < entity.X + entity.Width && p.Y > entity.Y && p.Y < entity.Y + entity.Height)
                {
                    return entity;
                }
            }
            return null;
        }

        /// <summary>
        /// Raise the 'Modified' event to inform the environment that something has changed
        /// and refresh the control.
        /// </summary>
        public void OnModifiedWithRefresh()
        {
            if (Modified != null)
            {
                Modified(this, EventArgs.Empty);
            }

            Refresh();
        }

        /// <summary>
        /// Show the dialog for adding a new entity.
        /// </summary>
        private void AddNewEntity()
        {
            using (NewEntity newEntity = new NewEntity())
            {
                newEntity.Location = PointToScreen(mouseDownLocation);
                if (newEntity.ShowDialog() == DialogResult.OK)
                {
                    if (dataModel.Entities.Any(x => x.Name == newEntity.EntityName))
                    {
                        MessageBox.Show(this, "Cannot add a new entity with this name because another entity with the name '" + newEntity.EntityName + "' already exists.", "Add entity failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        dataModel.Entities.Add(new Entity(this) { Name = newEntity.EntityName, X = mouseDownLocation.X, Y = mouseDownLocation.Y });
                        lblAdd.Visible = false;
                        OnModifiedWithRefresh();
                        OnSelectionChanged();
                    }
                }
            }
        }
        
        /// <summary>
        /// Show the dialog for adding a new query to an entity.
        /// </summary>
        /// <param name="entity">the entity to which a query should be added</param>
        private void AddNewQuery(Entity entity)
        {
            string connectionString = dataModel.GetConnectionString();
            if (String.IsNullOrEmpty(connectionString))
            {
                using (Connection frm = new Connection(connectionStringService))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        connectionString = frm.ConnectionString;
                        dataModel.EditorProperties.ConnectionString = frm.ConnectionStringName;
                    }
                }
            }

            if (!String.IsNullOrEmpty(connectionString))
            {
                using (AddQuery addQuery = new AddQuery(connectionString, entity.Queries))
                {
                    addQuery.Width = dataModel.DialogWidth;
                    addQuery.Height = dataModel.DialogHeight;
                    addQuery.Text = "Add a new query to entity \"" + entity.Name + "\"";
                    if (addQuery.ShowDialog(this) == DialogResult.OK)
                    {
                        try
                        {
                            // get query metadata from DB
                            DB.Analyzer analyzer = new DB.Analyzer(dataModel.GetConnectionString());
                            DB.Metadata md = analyzer.GetQueryMetadata(addQuery.CommandText, addQuery.CommandType);

                            bool addOk = true;
                            bool incompatibility = false;
                            if (addQuery.ExecuteMethod == ExecuteMethod.Reader)
                            {
                                if (entity.Properties.Count == 0)
                                {
                                    entity.SetColumns(md);
                                }
                                else
                                {
                                    List<string> errorDetails = CheckCompatibility(entity, md);
                                    if (errorDetails.Count > 0)
                                    {
                                        using (CompatibilityError errorDlg = new CompatibilityError(errorDetails, addQuery.QueryName))
                                        {
                                            if (errorDlg.ShowDialog(this) == DialogResult.OK)
                                            {
                                                entity.SetColumns(md);
                                                incompatibility = true;
                                            }
                                            else
                                            {
                                                addOk = false;
                                            }
                                        }
                                    }
                                }
                            }

                            if (addOk)
                            {
                                Query query = new Query(this, entity)
                                {
                                    Name = addQuery.QueryName,
                                    CommandText = addQuery.CommandText,
                                    CommandType = addQuery.CommandType,
                                    ExecuteMethod = addQuery.ExecuteMethod,
                                    ReturnType = "object"
                                };
                                query.Parameters.AddRange(md.Parameters);
                                entity.Queries.Add(query);

                                if (incompatibility)
                                {
                                    RefreshQuery(query, entity, true, true);
                                    okQueryTimer.Start();
                                }
                            }

                            OnModifiedWithRefresh();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "An error occured while adding the new query:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            Query query = new Query(this, entity)
                            {
                                Name = addQuery.QueryName,
                                CommandText = addQuery.CommandText,
                                CommandType = addQuery.CommandType,
                                ExecuteMethod = addQuery.ExecuteMethod,
                                ReturnType = "object",
                                ShowError = true
                            };
                            entity.Queries.Add(query);
                            OnModifiedWithRefresh();
                        }
                    }
                    dataModel.DialogWidth = addQuery.Width;
                    dataModel.DialogHeight = addQuery.Height;
                }
            }
        }

        /// <summary>
        /// Save entities into an XML file.
        /// </summary>
        /// <param name="file">file to save data from</param>
        public void SaveData(string file)
        {
            dataModel.Save(file);
        }

        /// <summary>
        /// Load entities from XML file.
        /// </summary>
        /// <param name="file">file to load data from</param>
        public void LoadData(string file)
        {
            if (dataModel.LoadFromFile(file))
            {
                lblAdd.Visible = (dataModel.Entities.Count == 0);
                Refresh();
                OnSelectionChanged();
            }
        }

        /// <summary>
        /// Raise the 'SelectionChanged' event and pass a list of selectable objects
        /// and the currently selected object. These objectgs are then displayed in the properties window.
        /// </summary>
        private void OnSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                var e = new SelectionChangedEventArgs();
                e.SelectableObjects.Add(dataModel.EditorProperties);
                e.SelectableObjects.AddRange(dataModel.Entities);

                if (selectedQuery != null)
                {
                    e.SelectableObjects.Add(selectedQuery);
                    e.SetSelectedObject(selectedQuery);
                }
                else if (selectedEntity != null)
                {
                    e.SetSelectedObject(selectedEntity);
                }
                else
                {
                    e.SetSelectedObject(dataModel.EditorProperties);
                }
                SelectionChanged(this, e);
            }
        }

        /// <summary>
        /// This method is called when the selected object in the properties window is changed.
        /// </summary>
        /// <param name="selectedObject">currently selected object</param>
        public void SetSelectedObject(object selectedObject)
        {
            if (selectedObject == dataModel.EditorProperties)
            {
                // editor itself is selected, deselect everything
                UnselectAll();
                Refresh();
                OnSelectionChanged();
            }
            else if (selectedObject is Entity)
            {
                // an entity is selected
                UnselectAll();
                selectedEntity = selectedObject as Entity;
                selectedEntity.IsSelected = true;

                // make the selected entity the current one
                dataModel.Entities.Remove(selectedEntity);
                dataModel.Entities.Add(selectedEntity);

                Refresh();
                OnSelectionChanged();
            }
            else if (selectedObject is Query)
            {
                // not supported yet!
                UnselectAll();
                Refresh();
                OnSelectionChanged();
            }
        }

        /// <summary>
        /// If an entity and/or query is selected, unselect them.
        /// </summary>
        private void UnselectAll()
        {
            if (selectedEntity != null)
            {
                selectedEntity.IsSelected = false;
                selectedEntity = null;
            }
            if (selectedQuery != null)
            {
                selectedQuery.IsSelected = false;
                selectedQuery = null;
            }
        }

        #region context menu events

        /// <summary>
        /// Show the dialog for adding a new entity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuEmptyNew_Click(object sender, EventArgs e)
        {
            AddNewEntity();
        }
      
        /// <summary>
        /// Delete the selected entity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuEntityDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedEntity();
        }

        /// <summary>
        /// Show a dialog for renaming the selected entity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuEntityRename_Click(object sender, EventArgs e)
        {
            if (selectedEntity != null)
            {
                using (RenameEntity renameEntity = new RenameEntity())
                {
                    renameEntity.EntityName = selectedEntity.Name;
                    if (renameEntity.ShowDialog(this) == DialogResult.OK)
                    {
                        if (selectedEntity.Name != renameEntity.EntityName)
                        {
                            if (dataModel.Entities.Any(x => x.Name == renameEntity.EntityName))
                            {
                                MessageBox.Show(this, "Cannot rename the entity because another entity with the name '" + renameEntity.EntityName + "' already exists.", "Rename failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                selectedEntity.Name = renameEntity.EntityName;
                                OnModifiedWithRefresh();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Show the dialog for adding a new query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuEntityAddQuery_Click(object sender, EventArgs e)
        {
            if (selectedEntity != null)
            {
                AddNewQuery(selectedEntity);
            }
        }

        /// <summary>
        /// Refresh all queries in the entity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuEntityRefresh_Click(object sender, EventArgs e)
        {
            if (selectedEntity != null)
            {
                foreach (Query query in selectedEntity.Queries.Where(x => x.ExecuteMethod != ExecuteMethod.Reader))
                {
                    RefreshQuery(query, selectedEntity);
                }

                Query firstReaderQuery = selectedEntity.Queries.FirstOrDefault(x => x.ExecuteMethod == ExecuteMethod.Reader);
                if (firstReaderQuery != null)
                {
                    RefreshQuery(firstReaderQuery, selectedEntity, true);
                }

                OnModifiedWithRefresh();
                okQueryTimer.Start();
            }
        }

        /// <summary>
        /// Delete the selected query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuQueryDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedQuery();
        }

        /// <summary>
        /// Refresh the selected query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuQueryRefresh_Click(object sender, EventArgs e)
        {
            if (selectedQuery != null && selectedEntity != null)
            {
                RefreshQuery(selectedQuery, selectedEntity);

                OnModifiedWithRefresh();
                okQueryTimer.Start();
            }
        }

        /// <summary>
        /// Refresh the given query. If the query is a 'reader' query, the entity is updated with
        /// the columns of the query and alls other 'reader' queries are checked for compatibility.
        /// </summary>
        /// <param name="query">the query to refresh</param>
        /// <param name="entity">the entity</param>
        /// <param name="refreshAll">if true, all reader queries are refreshed, even if the entity columns have not changed</param>
        /// <param name="suppressErrorMsg">if true, no MsgBox is displayed if queries are incompatible</param>
        private void RefreshQuery(Query query, Entity entity, bool refreshAll = false, bool suppressErrorMsg = false)
        {
            bool columnsChanged = false;
            try
            {
                DB.Analyzer analyzer = new DB.Analyzer(dataModel.GetConnectionString());
                DB.Metadata md = analyzer.GetQueryMetadata(query.CommandText, query.CommandType);

                if (query.ExecuteMethod == ExecuteMethod.Reader)
                {
                    columnsChanged = entity.SetColumns(md);
                }
                query.Parameters.Clear();
                query.Parameters.AddRange(md.Parameters);

                ShowQueryOkFeedback(query);
            }
            catch (Exception ex)
            {
                query.ShowError = true;
                Refresh();
                MessageBox.Show(this, "An error occured while refreshing the query '" + query.Name + "':\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // if entity columns have been changed, check all other 'reader' queries
            if (columnsChanged || refreshAll)
            {
                List<string> erroneousQueries = new List<string>();
                foreach (Query otherQuery in entity.Queries)
                {
                    if (otherQuery != query && otherQuery.ExecuteMethod == ExecuteMethod.Reader)
                    {
                        try
                        {
                            DB.Analyzer analyzer = new DB.Analyzer(dataModel.GetConnectionString());
                            DB.Metadata md = analyzer.GetQueryMetadata(otherQuery.CommandText, otherQuery.CommandType);
                            if (CheckCompatibility(entity, md, true).Count > 0)
                            {
                                if (!otherQuery.ShowError)
                                {
                                    otherQuery.ShowError = true;
                                    erroneousQueries.Add(otherQuery.Name);
                                }
                            }
                            else
                            {
                                ShowQueryOkFeedback(otherQuery);
                            }
                        }
                        catch (Exception ex)
                        {
                            otherQuery.ShowError = true;
                            Refresh();
                            MessageBox.Show(this, "An error occured while refreshing the query '" + otherQuery.Name + "':\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                Refresh();
                if (!suppressErrorMsg)
                {
                    if (erroneousQueries.Count == 1)
                    {
                        MessageBox.Show(
                        this,
                        "The result returned by the query '" + erroneousQueries[0] + "' is not compatible with the entity '" + entity.Name + "'.",
                        "Incompatible query",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                    else if (erroneousQueries.Count > 1)
                    {
                        MessageBox.Show(
                            this,
                            "The result returned by the following queries is not compatible with the entity '" + entity.Name + "':\r\n - " + String.Join("\r\n - ", erroneousQueries),
                            "Incompatible queries",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                Refresh();
            }
        }

        /// <summary>
        /// Show the 'ok' mark of the selected query for 1 second.
        /// </summary>
        /// <param name="query">the query</param>
        /// <param name="startTimer">whether to start the 1 second timer</param>
        private void ShowQueryOkFeedback(Query query)
        {
            query.ShowError = false;
            query.ShowOk = true;
            lock (((ICollection)okQueryList).SyncRoot)
            {
                okQueryList.Add(query);
            }
        }

        /// <summary>
        /// Edit the selected query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuQueryEdit_Click(object sender, EventArgs e)
        {
            if (selectedQuery != null && selectedEntity != null)
            {
                using (AddQuery addQuery = new AddQuery(selectedQuery, dataModel.GetConnectionString(), selectedEntity.Queries))
                {
                    addQuery.Width = dataModel.DialogWidth;
                    addQuery.Height = dataModel.DialogHeight;
                    addQuery.Text = String.Format("Edit query \"{0}\" of entity \"{1}\"", selectedQuery.Name, selectedEntity.Name);
                    if (addQuery.ShowDialog(this) == DialogResult.OK)
                    {
                        selectedQuery.Name = addQuery.QueryName;
                        selectedQuery.CommandText = addQuery.CommandText;
                        selectedQuery.CommandType = addQuery.CommandType;
                        selectedQuery.ExecuteMethod = addQuery.ExecuteMethod;
                        selectedQuery.ReturnType = "object";

                        RefreshQuery(selectedQuery, selectedEntity);
                        OnModifiedWithRefresh();
                        okQueryTimer.Start();
                    }
                    dataModel.DialogWidth = addQuery.Width;
                    dataModel.DialogHeight = addQuery.Height;
                }
            }
        }

        /// <summary>
        /// Delete the selected entity.
        /// </summary>
        private void DeleteSelectedEntity()
        {
            if (selectedEntity != null)
            {
                dataModel.Entities.Remove(selectedEntity);
                selectedEntity = null;
                selectedQuery = null;

                if (dataModel.Entities.Count == 0)
                {
                    lblAdd.Visible = true;
                }

                OnModifiedWithRefresh();
                OnSelectionChanged();
            }
        }

        /// <summary>
        /// Delete the selected query. If the last 'reader' query is deleted, all columns are deleted.
        /// </summary>
        private void DeleteSelectedQuery()
        {
            if (selectedQuery != null && selectedEntity != null)
            {
                selectedEntity.Queries.Remove(selectedQuery);
                if (!selectedEntity.Queries.Any(x => x.ExecuteMethod == ExecuteMethod.Reader))
                {
                    // remove columns if the last 'reader' query was removed
                    selectedEntity.Properties.Clear();
                }
                selectedQuery = null;
                if (selectedEntity != null)
                {
                    selectedEntity.IsSelected = false;
                    selectedEntity = null;
                }

                OnModifiedWithRefresh();
            }
        }

        #endregion
    }
}
