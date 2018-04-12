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
using System.Data;
using System.IO;
using System.Xml;

namespace deceed.DsLight.EditorGUI.Model
{
    /// <summary>
    /// Data model of a DsLight dataset.
    /// </summary>
    public class DataModel
    {
        private DsLightEditor editor;
        private string connectionStringShortName;
        private bool useWebConfigForConnectionString;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public DataModel()
        {
            Entities = new List<Entity>();
            DialogWidth = 406;
            DialogHeight = 371;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="editor">reference to editor control</param>
        public DataModel(DsLightEditor editor)
        {
            this.editor = editor;
            Entities = new List<Entity>();
            EditorProperties = new EditorProperties(editor);
            DialogWidth = 406;
            DialogHeight = 371;
        }

        /// <summary>
        /// Gets or sets the connection string if it should not be read from the config file.
        /// </summary>
        public string ConnectionStringOverride { get; set; }

        /// <summary>
        /// Gets a flag whether the connection string is stored in Web.config.
        /// </summary>
        public bool UseWebConfigForConnectionString
        {
            get { return useWebConfigForConnectionString; }
        }

        /// <summary>
        /// Gets or sets the name of the connection string in the App.config or Web.config file without the "...Properties.Settings" prefix.
        /// </summary>
        public string ConnectionStringShortName
        {
            get { return connectionStringShortName; }
            set
            {
                connectionStringShortName = value;
                int lastDot = connectionStringShortName.LastIndexOf('.');
                if (lastDot != -1)
                {
                    connectionStringShortName = connectionStringShortName.Substring(lastDot + 1);
                }
            }
        }

        /// <summary>
        /// Returns the connection string of the data set.
        /// </summary>
        /// <returns>connection string</returns>
        public string GetConnectionString()
        {
            if (!String.IsNullOrEmpty(ConnectionStringOverride))
            {
                return ConnectionStringOverride;
            }
            var csDict = editor.ConnectionStringService.GetConnectionStrings();
            if ((EditorProperties.ConnectionString != null) && csDict.ContainsKey(EditorProperties.ConnectionString))
            {
                return csDict[EditorProperties.ConnectionString];
            }
            return EditorProperties.ConnectionStringValue;
        }

        /// <summary>
        /// Gets the list of entities.
        /// </summary>
        public List<Entity> Entities { get; private set; }

        /// <summary>
        /// Gets or sets the DataSet properties that are displayed in the property window.
        /// </summary>
        public EditorProperties EditorProperties { get; set; }

        /// <summary>
        /// Gets or sets the width of the query dialog.
        /// </summary>
        public int DialogWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the query dialog.
        /// </summary>
        public int DialogHeight { get; set; }

        /// <summary>
        /// Load data model from an XML string.
        /// </summary>
        /// <param name="content">XML document as string</param>
        /// <returns>true if successful</returns>
        public bool Load(string content)
        {
            bool result = false;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(content);
                result = ParseXml(doc);
            }
            catch
            { }
            return result;
        }

        /// <summary>
        /// Load data model from an XML file.
        /// </summary>
        /// <param name="file">XML file</param>
        /// <returns>true if successful</returns>
        public bool LoadFromFile(string file)
        {
            bool result = false;
            if (File.Exists(file))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    result = ParseXml(doc);
                }
                catch
                {}
            }
            return result;
        }

        /// <summary>
        /// Parse an XML document and fill entity list.
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <returns>true if successful</returns>
        private bool ParseXml(XmlDocument doc)
        {
            Entities.Clear();

            useWebConfigForConnectionString = false;
            XmlNode csNode = doc.SelectSingleNode("DsLight/ConnectionString");
            if (csNode != null)
            {
                if ((csNode.Attributes["storage"] != null) && (csNode.Attributes["storage"].Value == "webconfig"))
                {
                    useWebConfigForConnectionString = true;
                }
                if (String.IsNullOrEmpty(ConnectionStringOverride))
                {
                    ConnectionStringShortName = csNode.Attributes["name"].Value;
                    if (EditorProperties != null)
                    {
                        EditorProperties.ConnectionStringValue = csNode.Attributes["connectionString"].Value;
                        EditorProperties.ConnectionString = csNode.Attributes["name"].Value;
                    }
                }
            }

            XmlNode dlgSizeNode = doc.SelectSingleNode("DsLight/AddQueryDialog");
            if (dlgSizeNode != null)
            {
                if (dlgSizeNode.Attributes["width"] != null)
                {
                    DialogWidth = Convert.ToInt32(dlgSizeNode.Attributes["width"].Value);
                }
                if (dlgSizeNode.Attributes["height"] != null)
                {
                    DialogHeight = Convert.ToInt32(dlgSizeNode.Attributes["height"].Value);
                }
            }

            foreach (XmlNode entityNode in doc.SelectNodes("DsLight/Entities/Entity"))
            {
                Entity entity = new Entity(editor)
                {
                    Name = entityNode.Attributes["name"].Value,
                    X = Convert.ToInt32(entityNode.Attributes["x"].Value),
                    Y = Convert.ToInt32(entityNode.Attributes["y"].Value)
                };
                Entities.Add(entity);

                foreach (XmlNode propNode in entityNode.SelectNodes("Properties/Property"))
                {
                    Property prop = new Property
                    {
                        Name = propNode.Attributes["name"].Value,
                        TypeName = propNode.Attributes["type"].Value,
                        DbType = propNode.Attributes["dbType"].Value,
                    };
                    entity.Properties.Add(prop);
                }

                foreach (XmlNode queryNode in entityNode.SelectNodes("Queries/Query"))
                {
                    Query query = new Query(editor, entity)
                    {
                        Name = queryNode.Attributes["name"].Value,
                        ExecuteMethod = (ExecuteMethod)Enum.Parse(typeof(ExecuteMethod), queryNode.Attributes["method"].Value),
                        CommandText = queryNode.Attributes["command"].Value,
                        CommandType = (System.Data.CommandType)Enum.Parse(typeof(System.Data.CommandType), queryNode.Attributes["type"].Value),
                        ReturnType = "object",
                        ShowError = queryNode.Attributes["error"] != null
                    };
                    entity.Queries.Add(query);

                    foreach (XmlNode paramNode in queryNode.SelectNodes("Parameters/Parameter"))
                    {
                        query.Parameters.Add(new DB.SPParam
                        {
                            Name = paramNode.Attributes["name"].Value,
                            DbType = (DbType)Enum.Parse(typeof(DbType), paramNode.Attributes["dbType"].Value),
                            SysType = paramNode.Attributes["sysType"].Value,
                            IsOutput = paramNode.Attributes["output"] != null
                        });
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Save the data model into an XML file.
        /// </summary>
        /// <param name="file">XML file</param>
        public void Save(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", ""));

            XmlElement root = doc.CreateElement("DsLight");
            AddAttribute(root, "version", "1.0");
            doc.AppendChild(root);

            XmlElement connectionString = doc.CreateElement("ConnectionString");
            AddAttribute(connectionString, "name", EditorProperties.ConnectionString);
            AddAttribute(connectionString, "connectionString", EditorProperties.ConnectionStringValue);
            AddAttribute(connectionString, "storage", editor.ConnectionStringService.UseWebConfigForConnectionString ? "webconfig" : "settings");
            root.AppendChild(connectionString);

            XmlElement dlgSizeNode = doc.CreateElement("AddQueryDialog");
            AddAttribute(dlgSizeNode, "width", DialogWidth);
            AddAttribute(dlgSizeNode, "height", DialogHeight);
            root.AppendChild(dlgSizeNode);
            
            XmlElement entitiesRoot = doc.CreateElement("Entities");
            root.AppendChild(entitiesRoot);

            foreach (Entity entity in Entities)
            {
                XmlElement entityNode = doc.CreateElement("Entity");
                AddAttribute(entityNode, "name", entity.Name);
                AddAttribute(entityNode, "x", entity.X);
                AddAttribute(entityNode, "y", entity.Y);
                entitiesRoot.AppendChild(entityNode);

                XmlElement propRoot = doc.CreateElement("Properties");
                entityNode.AppendChild(propRoot);

                foreach (Property prop in entity.Properties)
                {
                    XmlElement propNode = doc.CreateElement("Property");
                    AddAttribute(propNode, "type", prop.TypeName);
                    AddAttribute(propNode, "name", prop.Name);
                    AddAttribute(propNode, "dbType", prop.DbType);
                    propRoot.AppendChild(propNode);
                }

                XmlElement queryRoot = doc.CreateElement("Queries");
                entityNode.AppendChild(queryRoot);

                foreach (Query query in entity.Queries)
                {
                    XmlElement queryNode = doc.CreateElement("Query");
                    AddAttribute(queryNode, "name", query.Name);
                    AddAttribute(queryNode, "method", query.ExecuteMethod);
                    AddAttribute(queryNode, "command", query.CommandText);
                    AddAttribute(queryNode, "type", query.CommandType);
                    if (query.ShowError)
                    {
                        AddAttribute(queryNode, "error", "error");
                    }
                    queryRoot.AppendChild(queryNode);

                    XmlElement paramRoot = doc.CreateElement("Parameters");
                    queryNode.AppendChild(paramRoot);
                    foreach (DB.SPParam param in query.Parameters)
                    {
                        XmlElement paramNode = doc.CreateElement("Parameter");
                        AddAttribute(paramNode, "name", param.Name);
                        AddAttribute(paramNode, "dbType", param.DbType.ToString());
                        AddAttribute(paramNode, "sysType", param.SysType);
                        if (param.IsOutput)
                        {
                            AddAttribute(paramNode, "output", "output");
                        }
                        paramRoot.AppendChild(paramNode);
                    }
                }
            }

            doc.Save(file);
        }

        /// <summary>
        /// Helper method to add an XML attribute to an element.
        /// </summary>
        /// <param name="element">XmlElement</param>
        /// <param name="name">name of attribute</param>
        /// <param name="value">attribute value</param>
        private void AddAttribute(XmlElement element, string name, object value)
        {
            XmlAttribute attr = element.OwnerDocument.CreateAttribute(name);
            attr.Value = Convert.ToString(value);
            element.Attributes.Append(attr);
        }
    }
}
