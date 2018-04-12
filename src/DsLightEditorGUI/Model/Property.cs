﻿/*
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

using System.Data;

namespace deceed.DsLight.EditorGUI.Model
{
    /// <summary>
    /// This class represents a column of an entity.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the C# data-type.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the SQL data-type.
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// Gets a flag whether the C# data-type is nullable.
        /// </summary>
        public bool IsNullable
        {
            get { return TypeName.EndsWith("?"); }
        }
    }
}
