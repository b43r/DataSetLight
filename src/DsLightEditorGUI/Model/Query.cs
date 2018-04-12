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

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace deceed.DsLight.EditorGUI.Model
{
    /// <summary>
    /// This class represents an SQL query or stored-procedure call in a dataset.
    /// </summary>
    public class Query : PropertiesBase
    {
        private DsLightEditor editor;
        private Entity entity;
        private List<DB.SPParam> parameters = new List<DB.SPParam>();
        private string name;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="editor">reference to editor control</param>
        /// <param name="entity">entity to which this query belongs</param>
        public Query(DsLightEditor editor, Entity entity)
        {
            this.editor = editor;
            this.entity = entity;
        }

        /// <summary>
        /// Gets or sets the name of the query.
        /// </summary>
        [Description("The name of the method that is generated.")]
        public string Name
        {
            get { return name; }
            set
            {
                string newValue = Helper.MakeSafeName(value);
                if (name != newValue)
                {
                    if (entity.Queries.Any(x => x.Name == newValue))
                    {
                        MessageBox.Show(editor, "Cannot rename the query because another query with the name '" + newValue + "' already exists.", "Rename failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        name = newValue;
                        if (editor != null)
                        {
                            editor.OnModifiedWithRefresh();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the command text of the SQL command.
        /// </summary>
        [Description("The query or stored procedure to be executed against the Database.")]
        [ReadOnly(true)]
        public string CommandText { get; set; }
        
        /// <summary>
        /// Gets or sets teh execute method of the SQL command.
        /// </summary>
        [Description("ExecuteMethod for the query.")]
        [ReadOnly(true)]
        public ExecuteMethod ExecuteMethod { get; set; }

        /// <summary>
        /// Gets or sets the type fo the SQL command.
        /// </summary>
        [Description("Command type of this query or function.")]
        [ReadOnly(true)]
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the return type of the query as a string.
        /// </summary>
        [Browsable(false)]
        public string ReturnType { get; set; }

        /// <summary>
        /// Gets or sets a flag whether this query is selected.
        /// </summary>
        [Browsable(false)]
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets a flag whether this query should be displayed as erroneous.
        /// </summary>
        [Browsable(false)]
        public bool ShowError { get; set; }

        /// <summary>
        /// Gets or sets a flag whether this query should be displayed as verified.
        /// </summary>
        [Browsable(false)]
        public bool ShowOk { get; set; }

        /// <summary>
        /// Gets the list of parameters for this query.
        /// </summary>
        [Browsable(false)]
        public List<DB.SPParam> Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Returns the data-type of this object that is displayed in the properties window.
        /// </summary>
        /// <returns>name of data-type</returns>
        public override string GetClassName()
        {
            return "Query";
        }

        /// <summary>
        /// Returns the name of this object that is displayed in the properties window.
        /// </summary>
        /// <returns>entity name</returns>
        public override string GetComponentName()
        {
            return Name;
        }
    }
}
