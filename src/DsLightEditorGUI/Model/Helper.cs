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
using System.Text;

namespace deceed.DsLight.EditorGUI.Model
{
    internal static class Helper
    {
        /// <summary>
        /// Convert the given string into a valid C# identifier.
        /// </summary>
        /// <param name="name">input string</param>
        /// <returns>valid identifier</returns>
        public static string MakeSafeName(string name)
        {
            if (name.Length > 0 && Char.IsDigit(name[0]))
            {
                name = "_" + name;
            }
            char[] arr = name.ToCharArray();

            StringBuilder sb = new StringBuilder(name);
            for (int i = 0; i < sb.Length; i++)
            {
                if (!Char.IsLetterOrDigit(sb[i]))
                {
                    sb[i] = '_';
                }
            }
            return sb.ToString();
        }
    }
}
