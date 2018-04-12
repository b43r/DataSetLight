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
using System.ComponentModel;
using System.Drawing.Design;

namespace deceed.DsLight.EditorGUI
{
    /// <summary>
    /// Properties of the DataSet that are displayed in the properties window.
    /// </summary>
    public class EditorProperties : PropertiesBase
    {
        private DsLightEditor editor;
        private string connectionString;
        private string connectionStringValue;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="Editor">reference to editor control</param>
        public EditorProperties(DsLightEditor Editor)
        {
            editor = Editor;
        }

        /// <summary>
        /// Gets or sets the name of the connection string in the config file.
        /// </summary>
        [Category("Data")]
        [Description("Connection string to the Database.")]
        [Editor(typeof(ConnectionUITypeEditor), typeof(UITypeEditor))]
        public string ConnectionString
        {
            get { return connectionString; }
            set
            {
                if (connectionString != value)
                {
                    var csDict = ConnectionStringService.GetConnectionStrings();
                    if (csDict.ContainsKey(value))
                    {
                        connectionString = value;
                        connectionStringValue = csDict[connectionString];
                        editor.OnModifiedWithRefresh();
                    }
                    else if (value == "(None)")
                    {
                        connectionString = value;
                        connectionStringValue = string.Empty;
                        editor.OnModifiedWithRefresh();
                    }
                    else
                    {
                        // connection string renamed to a one not existing: cancel editing
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the actual connection string.
        /// </summary>
        [Browsable(false)]
        public string ConnectionStringValue {
            get { return connectionStringValue; }
            set
            {
                if (connectionStringValue != value)
                {
                    connectionStringValue = value;
                    editor.OnModifiedWithRefresh();
                }
            }
        }

        /// <summary>
        /// Returns the data-type of this object that is displayed in the properties window.
        /// </summary>
        /// <returns>name of data-type</returns>
        public override string GetClassName()
        {
            return "DataSet DsLight";
        }

        /// <summary>
        /// Returns the name of this object that is displayed in the properties window.
        /// </summary>
        /// <returns>entity name</returns>
        public override string GetComponentName()
        {
            return editor.Name;
        }

        /// <summary>
        /// Gets the IConnectionString interface.
        /// </summary>
        public IConnectionString ConnectionStringService
        {
            get { return editor.ConnectionStringService; }
        }
    }
}
