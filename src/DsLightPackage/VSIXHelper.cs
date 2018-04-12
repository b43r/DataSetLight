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

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace deceed.DsLightPackage
{
    /// <summary>
    /// Extension methods for working with IVs... objects.
    /// </summary>
    public static class VSIXHelper
    {
        /// <summary>
        /// Gets an enumerator with all projects (IVsProject) in the solution.
        /// </summary>
        /// <param name="solution">IVsSolution</param>
        /// <returns>enumerator of IVsProject</returns>
        public static IEnumerable<IVsProject> GetLoadedProjects(this IVsSolution solution)
        {
            IEnumHierarchies enumerator;
            Guid guid = Guid.Empty;
            solution.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_ALLINSOLUTION, ref guid, out enumerator);
            uint fetched = 0;
            IVsHierarchy[] hierarchy = new IVsHierarchy[1] { null };
            for (enumerator.Reset(); enumerator.Next(1, hierarchy, out fetched) == VSConstants.S_OK && fetched == 1; /*nothing*/)
            {
                yield return (IVsProject)hierarchy[0];
            }
        }

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        /// <param name="project">IVsProject</param>
        /// <returns>project name</returns>
        public static string GetProjectName(this IVsProject project)
        {
            string projectName;
            project.GetMkDocument((uint)Microsoft.VisualStudio.VSConstants.VSITEMID.Root, out projectName);
            return projectName;
        }

        /// <summary>
        /// Gets the default namespace of the project.
        /// </summary>
        /// <param name="project">IVsProject</param>
        /// <returns>default namespace</returns>
        public static string GetDefaultNamespace(this IVsProject project)
        {
            object pvar;
            ((IVsHierarchy)project).GetProperty((uint)Microsoft.VisualStudio.VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_DefaultNamespace, out pvar);
            return Convert.ToString(pvar);
        }

        /// <summary>
        /// Check whether the project contains a document. 
        /// </summary>
        /// <param name="project">IVsProject</param>
        /// <param name="pszMkDocument">full path of document</param>
        /// <returns>true if project contains document</returns>
        public static uint IsDocumentInProject2(this IVsProject project, string pszMkDocument)
        {
            int found;
            uint itemId;
            VSDOCUMENTPRIORITY[] prio = new VSDOCUMENTPRIORITY[1];
            project.IsDocumentInProject(pszMkDocument, out found, prio, out itemId);
            return found != 0 ? itemId : 0;
        }

        /// <summary>
        /// Returns the item id of the given special file in the project. If the file does not exist, it is created.
        /// </summary>
        /// <param name="project">IVsProject</param>
        /// <param name="fileID">type of file</param>
        /// <returns>item id</returns>
        public static uint GetProjectSpecialFile(this IVsProject project, __PSFFILEID2 fileID)
        {
            var projectSpecialFile = (IVsProjectSpecialFiles)project;
            uint itemId;
            string itemName;
            if (projectSpecialFile.GetFile((int)fileID, (uint)__PSFFLAGS.PSFF_CreateIfNotExist, out itemId, out itemName) == VSConstants.S_OK)
            {
                return itemId;
            }
            return 0;
        }

        /// <summary>
        /// Returns the full path name of the given special file in the project. If the file does not exist, it is created.
        /// </summary>
        /// <param name="project">IVsProject</param>
        /// <param name="fileID">type of file</param>
        /// <param name="createIfMissing">whether to create the file if it is missind</param>
        /// <returns>path and filename</returns>
        public static string GetProjectSpecialFileName(this IVsProject project, int fileID, bool createIfMissing)
        {
            var projectSpecialFile = (IVsProjectSpecialFiles)project;
            uint itemId;
            string itemName;
            uint flags = (uint)__PSFFLAGS.PSFF_FullPath;
            if (createIfMissing)
            {
                flags |=  (uint)__PSFFLAGS.PSFF_CreateIfNotExist;
            }
            if (projectSpecialFile.GetFile((int)fileID,  flags, out itemId, out itemName) == VSConstants.S_OK)
            {
                return itemName;
            }
            return string.Empty;
        }

        /// <summary>
        /// Runs the custom tool of a project item.
        /// </summary>
        /// <param name="project">IVsProject</param>
        /// <param name="itemId">item id</param>
        public static void RunCustomToolOfItem(this IVsProject project, uint itemId)
        {
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP;
            project.GetItemContext(itemId, out ppSP);
            Microsoft.VisualStudio.Shell.ServiceProvider itemContextService = new Microsoft.VisualStudio.Shell.ServiceProvider(ppSP);
            EnvDTE.ProjectItem templateItem = (EnvDTE.ProjectItem)itemContextService.GetService(typeof(EnvDTE.ProjectItem));
            VSLangProj.VSProjectItem vsProjectItem = templateItem.Object as VSLangProj.VSProjectItem;
            vsProjectItem.RunCustomTool();
        }
    }
}
