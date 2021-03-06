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

using System;
using System.Collections;

namespace deceed.DsLight.EditorGUI
{
    /// <summary>
    /// Arguments for the 'SelectionChanged' event.
    /// </summary>
    public class SelectionChangedEventArgs : EventArgs
    {
        public SelectionChangedEventArgs()
        {
            SelectableObjects = new ArrayList();
            SelectedObject = new ArrayList();
        }

        /// <summary>
        /// Gets or sets a list of selectable objects.
        /// </summary>
        public ArrayList SelectableObjects { get; set; }

        /// <summary>
        /// Gets or sets a list of selected objects.
        /// </summary>
        public ArrayList SelectedObject { get; private set; }

        /// <summary>
        /// Adds an object to the list of selected objects.
        /// </summary>
        /// <param name="obj">selected object</param>
        public void SetSelectedObject(object obj)
        {
            SelectedObject.Clear();
            SelectedObject.Add(obj);
        }
    }
}
