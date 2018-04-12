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

namespace deceed.DsLight.EditorGUI
{
    /// <summary>
    /// Dialog for displaying compatibility errors.
    /// </summary>
    public partial class CompatibilityError : Form
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="errorDetails">list of errors</param>
        /// <param name="queryName">name of the erroneous query</param>
        public CompatibilityError(List<string> errorDetails, string queryName)
        {
            InitializeComponent();

            lblText.Text = String.Format("The result returned by the new query '{0}' is not compatible with this entity.", queryName);
            listBox1.Items.AddRange(errorDetails.ConvertAll(x => (object)x).ToArray());
        }
    }
}
