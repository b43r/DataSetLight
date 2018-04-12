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

namespace deceed.DsLight.EditorGUI
{
    /// <summary>
    /// Dialog for renaming an entity.
    /// </summary>
    public partial class RenameEntity : Form
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        public RenameEntity()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        public string EntityName
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }

        /// <summary>
        /// Only allow letters, digits and the underscore character.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.KeyChar) && (e.KeyChar != 95) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }
        }
    }
}
