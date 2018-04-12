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

using System.Data;

namespace deceed.DsLight.EditorGUI.DB
{
    /// <summary>
    /// Class that represents a parameter to an SQL query or stored-procedure.
    /// </summary>
    public class SPParam
    {
        public string Name { get; set; }
        public string SysType { get; set; }
        public DbType DbType { get; set; }
        public bool IsOutput { get; set; }
    }
}
