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

namespace deceed.DsLight.EditorGUI.DB
{
    /// <summary>
    /// Class that represents the metadata of an SQL query or stored-procedure.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Gets or sets the list of parameters.
        /// </summary>
        public List<SPParam> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the list of columns.
        /// </summary>
        public List<Column> Columns { get; set; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public Metadata()
        {
            Parameters = new List<SPParam>();
            Columns = new List<Column>();
        }
    }
}
