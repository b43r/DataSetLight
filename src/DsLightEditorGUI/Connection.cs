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
using System.Windows.Forms;

using Microsoft.Data.ConnectionUI;

namespace deceed.DsLight.EditorGUI
{
    /// <summary>
    /// Dialog for chosing a connection string or create a new one.
    /// </summary>
    public partial class Connection : Form
    {
        private IConnectionString connectionStringService;
        private Dictionary<string, string> csDict;

        private class CsListItem
        {
            public CsListItem(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return String.Format("{0} ({1})", Name, Value);
            }
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="connectionStringService">IConnectionString</param>
        public Connection(IConnectionString connectionStringService)
        {
            this.connectionStringService = connectionStringService;

            InitializeComponent();

            csDict = connectionStringService.GetConnectionStrings();
            foreach (var kvp in csDict)
            {
                lstConnection.Items.Add(new CsListItem(kvp.Key, kvp.Value));
            }
            btnNext.Enabled = false;
        }

        /// <summary>
        /// Gets the name of the selected connection string.
        /// </summary>
        public string ConnectionStringName
        {
            get
            {
                if (lstConnection.SelectedIndex != -1)
                {
                    return (lstConnection.SelectedItem as CsListItem).Name;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the the selected connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (lstConnection.SelectedIndex != -1)
                {
                    return (lstConnection.SelectedItem as CsListItem).Value;
                }
                return null;
            }
        }

        /// <summary>
        /// Enable the 'next' button only if a connection string has been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnNext.Enabled = lstConnection.SelectedIndex != -1;
        }

        /// <summary>
        /// Show the dialog for creating a new connection string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            DataSource sqlDataSource = new DataSource("MicrosoftSqlServer", "Microsoft SQL Server");
            sqlDataSource.Providers.Add(DataProvider.SqlDataProvider);
            DataConnectionDialog dcd = new DataConnectionDialog();

            dcd.DataSources.Add(sqlDataSource);
            dcd.SelectedDataProvider = DataProvider.SqlDataProvider;
            dcd.SelectedDataSource = sqlDataSource;

            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {
                string name = connectionStringService.AddConnectionString("ConnectionString", dcd.ConnectionString);
                csDict.Add(name, dcd.ConnectionString);
                lstConnection.Items.Add(new CsListItem(name, dcd.ConnectionString));
            }
        }
    }
}
