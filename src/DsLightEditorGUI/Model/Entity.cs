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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace deceed.DsLight.EditorGUI.Model
{
    /// <summary>
    /// This class represents an entity in the dataset.
    /// </summary>
    public class Entity : PropertiesBase
    {
        private DsLightEditor editor;
        private List<Property> properties = new List<Property>();
        private List<Query> queries = new List<Query>();
        private string name;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="editor">reference to editor control</param>
        public Entity(DsLightEditor editor)
        {
            this.editor = editor;
        }

        /// <summary>
        /// Gets or sets the entity name.
        /// </summary>
        [Description("The name of t1he entity.")]
        public string Name
        {
            get { return name; }
            set
            {
                string newValue = Helper.MakeSafeName(value);
                if (name != newValue)
                {
                    if ((editor != null) && editor.dataModel.Entities.Any(x => x.name == newValue))
                    {
                        MessageBox.Show(editor, "Cannot rename the entity because another entity with the name '" + newValue + "' already exists.", "Rename failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Gets or sets the X position of the entity.
        /// </summary>
        [Browsable(false)]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y position of the entity.
        /// </summary>
        [Browsable(false)]
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the entity.
        /// </summary>
        [Browsable(false)]
        public int Width { get; private set; }

        /// <summary>
        /// Gets or sets the height of the entity.
        /// </summary>
        [Browsable(false)]
        public int Height { get; private set; }

        /// <summary>
        /// Gets or sets a flag whether the entity is selected.
        /// </summary>
        [Browsable(false)]
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets the list of properties.
        /// </summary>
        [Browsable(false)]
        public List<Property> Properties
        {
            get { return properties; }
        }

        /// <summary>
        /// Gets the list of queries.
        /// </summary>
        [Browsable(false)]
        public List<Query> Queries
        {
            get { return queries; }
        }

        /// <summary>
        /// Draw the entity to a Graphics object.
        /// </summary>
        /// <param name="g">Graphics</param>
        public void Draw(Graphics g, Point offset)
        {
            const int GrayBlockWidth = 20;

            int yOffset = 0;
            Font font = new Font("Arial", 10, FontStyle.Bold);
            Pen pen = new Pen(IsSelected ? Color.FromArgb(153, 180, 209) : Color.FromArgb(51, 153, 255), 2);
            Brush brush = new SolidBrush(IsSelected ? Color.FromArgb(153, 180, 209) : Color.FromArgb(160, 160, 160));
            Brush grayBrush = new SolidBrush(Color.FromArgb(240, 240, 240));

            try
            {
                // measure column width
                float typeColWidth = 0;
                float nameColWidth = 0;
                foreach (Property prop in properties)
                {
                    SizeF sizeType = g.MeasureString(prop.TypeName, SystemFonts.DefaultFont);
                    if (sizeType.Width > typeColWidth)
                    {
                        typeColWidth = sizeType.Width;
                    }
                    SizeF sizeName = g.MeasureString(prop.Name, SystemFonts.DefaultFont);
                    if (sizeName.Width > nameColWidth)
                    {
                        nameColWidth = sizeName.Width;
                    }
                }
                typeColWidth += (10 + GrayBlockWidth);
                nameColWidth += 10;

                // measure title
                int totalMinWidth = (int)g.MeasureString(Name, font).Width + 10;

                // measure queries
                foreach (Query query in queries)
                {
                    int queryWidth = (int)g.MeasureString(query.Name, SystemFonts.DefaultFont).Width + 10 + GrayBlockWidth;
                    if (queryWidth > totalMinWidth)
                    {
                        totalMinWidth = queryWidth;
                    }
                }

                // measure "Add..." link
                totalMinWidth = Math.Max((int)g.MeasureString("Add...", SystemFonts.DefaultFont).Width + 10 + GrayBlockWidth, totalMinWidth);

                if (typeColWidth + nameColWidth < totalMinWidth)
                {
                    nameColWidth = totalMinWidth - typeColWidth;
                }

                Width = (int)Math.Round(typeColWidth + nameColWidth);
                Height = 21 + (properties.Count * 20) + 1 + (queries.Count * 20) + 1 + 20;

                g.FillRectangle(Brushes.White, X + offset.X, Y + offset.Y, Width, Height);

                // header
                g.FillRectangle(brush, X + offset.X, Y + offset.Y, Width, 20);
                g.DrawString(Name, font, Brushes.White, X + offset.X + 5, Y + offset.Y + 2);
                g.DrawLine(Pens.Black, X + offset.X, Y + offset.Y + 20, X + offset.X + Width, Y + offset.Y + 20);
                yOffset += 21;

                // gray block at left
                g.FillRectangle(grayBrush, new Rectangle(X + offset.X, Y + offset.Y + yOffset, 20, Height - yOffset));

                // properties
                g.DrawLine(Pens.LightGray, X + offset.X + typeColWidth, Y + offset.Y + yOffset + 1, X + offset.X + typeColWidth, Y + offset.Y + yOffset + (properties.Count * 20));
                foreach (Property prop in properties)
                {
                    g.DrawString(prop.TypeName, SystemFonts.DefaultFont, Brushes.Black, X + offset.X + 4 + GrayBlockWidth, Y + offset.Y + yOffset + 4);
                    g.DrawString(prop.Name, SystemFonts.DefaultFont, Brushes.Black, X + offset.X + typeColWidth + 5, Y + offset.Y + yOffset + 4);
                    g.DrawLine(Pens.Gray, X + offset.X, Y + offset.Y + yOffset + 20, X + offset.X + Width, Y + offset.Y + yOffset + 20);
                    yOffset += 20;
                }
                g.DrawLine(Pens.Black, X + offset.X, Y + offset.Y + yOffset, X + offset.X + Width, Y + offset.Y + yOffset);
                g.DrawLine(Pens.Black, X + offset.X, Y + offset.Y + yOffset + 1, X + offset.X + Width, Y + offset.Y + yOffset + 1);
                yOffset += 1;

                // queries
                foreach (Query query in queries)
                {
                    if (query.IsSelected)
                    {
                        g.FillRectangle(brush, new Rectangle(X + offset.X, Y + offset.Y + yOffset + 1, Width, 19));
                    }

                    if (query.ShowError)
                    {
                        g.DrawImage(deceed.DsLight.EditorGUI.Properties.Resources.error, X + offset.X + 4, Y + offset.Y + yOffset + 4);
                    }
                    else if (query.ShowOk)
                    {
                        g.DrawImage(deceed.DsLight.EditorGUI.Properties.Resources.ok, X + offset.X + 4, Y + offset.Y + yOffset + 4);
                    }
                    else
                    {
                        g.DrawImage(deceed.DsLight.EditorGUI.Properties.Resources.sql, X + offset.X + 4, Y + offset.Y + yOffset + 4);
                    }

                    g.DrawString(query.Name, SystemFonts.DefaultFont, query.IsSelected ? Brushes.White : Brushes.Black, X + offset.X + 4 + GrayBlockWidth, Y + offset.Y + yOffset + 4);
                    g.DrawLine(Pens.Gray, X + offset.X, Y + offset.Y + yOffset + 20, X + offset.X + Width, Y + offset.Y + yOffset + 20);
                    yOffset += 20;
                }
                g.DrawLine(Pens.Black, X + offset.X, Y + offset.Y + yOffset, X + offset.X + Width, Y + offset.Y + yOffset);
                g.DrawLine(Pens.Black, X + offset.X, Y + offset.Y + yOffset + 1, X + offset.X + Width, Y + offset.Y + yOffset + 1);
                yOffset += 1;

                // Footer
                g.DrawString("Add...", SystemFonts.DefaultFont, Brushes.Black, X + offset.X + 4 + GrayBlockWidth, Y + offset.Y + yOffset + 4);
                yOffset += 20;

                g.DrawRectangle(pen, X + offset.X, Y + offset.Y, Width, yOffset);
            }
            finally
            {
                font.Dispose();
                pen.Dispose();
                brush.Dispose();
                grayBrush.Dispose();
            }
        }
        
        /// <summary>
        /// Returns the query at the given Y offset.
        /// </summary>
        /// <param name="y">y-offset from top</param>
        /// <returns>Query or null</returns>
        public Query GetQueryAtPosition(int y)
        {
            y = y - 21 - (properties.Count * 20) - 1;
            if (y > 0)
            {
                y = y / 20;
                if (y < queries.Count)
                {
                    return queries[y];
                }
            }
            return null;
        }

        /// <summary>
        /// Copy columns from meta-data object to this entity if they have changed.
        /// </summary>
        /// <param name="md">Metadata</param>
        /// <returns>true if columns have changed</returns>
        public bool SetColumns(DB.Metadata md)
        {
            bool changed = (Properties.Count != md.Columns.Count);

            if (!changed)
            {
                for (int i = 0; i < Properties.Count; i++)
                {
                    if ((Properties[i].Name != md.Columns[i].Name) ||
                        (Properties[i].TypeName != md.Columns[i].SysType) ||
                        (Properties[i].DbType != md.Columns[i].DbType))
                    {
                        changed = true;
                        break;
                    }
                }
            }

            if (changed)
            {
                Properties.Clear();
                foreach (DB.Column col in md.Columns)
                {
                    Properties.Add(new Property
                    {
                        Name = col.Name,
                        TypeName = col.SysType,
                        DbType = col.DbType
                    });
                }
            }

            return changed;
        }

        /// <summary>
        /// Returns the data-type of this object that is displayed in the properties window.
        /// </summary>
        /// <returns>name of data-type</returns>
        public override string GetClassName()
        {
            return "Entity";
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
