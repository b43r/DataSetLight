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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Microsoft.Data.ConnectionUI;

namespace deceed.DsLight.EditorGUI
{
    /// <summary>
    /// Editor for editing the connection string in the properties window.
    /// </summary>
    internal class ConnectionUITypeEditor : UITypeEditor
    {
        private EditorProperties editorProperties;
        private IWindowsFormsEditorService _editorService;
        private string oldConnectionStringName;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// This method is called every time the connection-string dropdown is opened in the properties grid.
        /// Create the ListBox and populate it with all connection-strings that are found in the settings file.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="provider">IServiceProvider</param>
        /// <param name="value">value of selected item</param>
        /// <returns>selected item</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            
            // use a list box
            ListBox lb = new ListBox();
            lb.SelectionMode = SelectionMode.One;
            lb.SelectedValueChanged += OnListBoxSelectedValueChanged;

            // add all connection-strings
            editorProperties = context.Instance as EditorProperties;
            if (editorProperties.ConnectionStringService != null)
            {
                Dictionary<string, string> allConnectionStrings = editorProperties.ConnectionStringService.GetConnectionStrings();
                foreach (var cs in allConnectionStrings)
                {
                    lb.Items.Add(cs.Key);
                    if (cs.Key == Convert.ToString(value))
                    {
                        lb.SelectedItem = cs.Key;
                    }
                }
            }
            lb.Items.Add("(New Connection...)");
            lb.Items.Add("(Edit)");
            lb.Items.Add("(None)");

            if (String.IsNullOrEmpty(Convert.ToString(value)))
            {
                lb.SelectedIndex = lb.Items.Count - 1;
            }

            oldConnectionStringName = Convert.ToString(value);

            // show this model stuff
            _editorService.DropDownControl(lb);
            if ((lb.SelectedItem == null) || (lb.Text == "(Edit)") || (lb.Text == "(New Connection...)"))
            {
                return value;
            }

            return lb.SelectedItem;
        }

        /// <summary>
        /// Close the dropdown if an entry has been selected.
        /// Show the "edit" or "new" connection dialog if either (Edit) or (New Connection...) has been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListBoxSelectedValueChanged(object sender, EventArgs e)
        {
            // close the drop down as soon as something is clicked
            _editorService.CloseDropDown();

            if ((sender as ListBox).Text == "(Edit)")
            {
                EditConnection(oldConnectionStringName);
            }
            else if ((sender as ListBox).Text == "(New Connection...)")
            {
                NewConnection((sender as ListBox));
            }
        }

        /// <summary>
        /// Show the dialog for editing an existing connection string and update the
        /// connection string in the settings file.
        /// </summary>
        /// <param name="connectionStringName">connection string name</param>
        private void EditConnection(string connectionStringName)
        {
            if (editorProperties.ConnectionStringService != null)
            {
                var dict = editorProperties.ConnectionStringService.GetConnectionStrings();
                if (dict.ContainsKey(connectionStringName))
                {
                    DataSource sqlDataSource = new DataSource("MicrosoftSqlServer", "Microsoft SQL Server");
                    sqlDataSource.Providers.Add(DataProvider.SqlDataProvider);
                    DataConnectionDialog dcd = new DataConnectionDialog();

                    dcd.DataSources.Add(sqlDataSource);
                    dcd.SelectedDataProvider = DataProvider.SqlDataProvider;
                    dcd.SelectedDataSource = sqlDataSource;
                    dcd.ConnectionString = dict[connectionStringName];

                    if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
                    {
                        editorProperties.ConnectionStringService.UpdateConnectionString(connectionStringName, dcd.ConnectionString);
                        editorProperties.ConnectionStringValue = dcd.ConnectionString;
                    }
                }
            }
        }

        /// <summary>
        /// Show the dialog for creating a new connection string and store the
        /// new connection string in the settings file.
        /// </summary>
        /// <param name="lb">ListBox</param>
        private void NewConnection(ListBox lb)
        {
            DataSource sqlDataSource = new DataSource("MicrosoftSqlServer", "Microsoft SQL Server");
            sqlDataSource.Providers.Add(DataProvider.SqlDataProvider);
            DataConnectionDialog dcd = new DataConnectionDialog();

            dcd.DataSources.Add(sqlDataSource);
            dcd.SelectedDataProvider = DataProvider.SqlDataProvider;
            dcd.SelectedDataSource = sqlDataSource;

            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {
                if (editorProperties.ConnectionStringService != null)
                {
                    string name = editorProperties.ConnectionStringService.AddConnectionString("ConnectionString", dcd.ConnectionString);
                    if (name.Contains(".Properties.Settings."))
                    {
                        // remove "...Properties.Settings." from the start of the connection string name
                        name = name.Substring(name.IndexOf(".Properties.Settings.") + ".Properties.Settings.".Length);
                    }
                    lb.Items.Insert(lb.Items.Count - 3, name);
                    lb.SelectedItem = name;
                }
            }
        }
    }
}
