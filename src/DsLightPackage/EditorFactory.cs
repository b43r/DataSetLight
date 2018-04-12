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
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace deceed.DsLightPackage
{
    /// <summary>
    /// Factory for creating our editor object. Extends from the IVsEditoryFactory interface
    /// </summary>
    [Guid(GuidList.guidDsLightPackageEditorFactoryString)]
    public sealed class EditorFactory : IVsEditorFactory, IDisposable
    {
        private DsLightPackagePackage editorPackage;
        private ServiceProvider vsServiceProvider;

        public EditorFactory(DsLightPackagePackage package)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering {0} constructor", this.ToString()));

            this.editorPackage = package;
        }

        /// <summary>
        /// Since we create a ServiceProvider which implements IDisposable we
        /// also need to implement IDisposable to make sure that the ServiceProvider's
        /// Dispose method gets called.
        /// </summary>
        public void Dispose()
        {
            if (vsServiceProvider != null)
            {
                vsServiceProvider.Dispose();
            }
        }

        #region IVsEditorFactory Members

        /// <summary>
        /// Used for initialization of the editor in the environment
        /// </summary>
        /// <param name="psp">pointer to the service provider. Can be used to obtain instances of other interfaces
        /// </param>
        /// <returns></returns>
        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            vsServiceProvider = new ServiceProvider(psp);
            return VSConstants.S_OK;
        }

        public object GetService(Type serviceType)
        {
            return vsServiceProvider.GetService(serviceType);
        }

        // This method is called by the Environment (inside IVsUIShellOpenDocument::
        // OpenStandardEditor and OpenSpecificEditor) to map a LOGICAL view to a 
        // PHYSICAL view. A LOGICAL view identifies the purpose of the view that is
        // desired (e.g. a view appropriate for Debugging [LOGVIEWID_Debugging], or a 
        // view appropriate for text view manipulation as by navigating to a find
        // result [LOGVIEWID_TextView]). A PHYSICAL view identifies an actual type 
        // of view implementation that an IVsEditorFactory can create. 
        //
        // NOTE: Physical views are identified by a string of your choice with the 
        // one constraint that the default/primary physical view for an editor  
        // *MUST* use a NULL string as its physical view name (*pbstrPhysicalView = NULL).
        //
        // NOTE: It is essential that the implementation of MapLogicalView properly
        // validates that the LogicalView desired is actually supported by the editor.
        // If an unsupported LogicalView is requested then E_NOTIMPL must be returned.
        //
        // NOTE: The special Logical Views supported by an Editor Factory must also 
        // be registered in the local registry hive. LOGVIEWID_Primary is implicitly 
        // supported by all editor types and does not need to be registered.
        // For example, an editor that supports a ViewCode/ViewDesigner scenario
        // might register something like the following:
        //        HKLM\Software\Microsoft\VisualStudio\<version>\Editors\
        //            {...guidEditor...}\
        //                LogicalViews\
        //                    {...LOGVIEWID_TextView...} = s ''
        //                    {...LOGVIEWID_Code...} = s ''
        //                    {...LOGVIEWID_Debugging...} = s ''
        //                    {...LOGVIEWID_Designer...} = s 'Form'
        //
        public int MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
        {
            pbstrPhysicalView = null;    // initialize out parameter

            // we support only a single physical view
            if (VSConstants.LOGVIEWID_Primary == rguidLogicalView)
            {
                return VSConstants.S_OK;        // primary view uses NULL as pbstrPhysicalView
            }
            else if (VSConstants.LOGVIEWID.TextView_guid == rguidLogicalView)
            {
                // Our editor supports FindInFiles, therefore we need to declare support for LOGVIEWID_TextView.
                // In addition our EditorPane implements IVsCodeWindow and we also provide the 
                // VSSettings (pkgdef) metadata statement that we support LOGVIEWID_TextView via the following
                // attribute on our Package class:
                // [ProvideEditorLogicalView(typeof(EditorFactory), VSConstants.LOGVIEWID.TextView_string)]

                pbstrPhysicalView = null; // our primary view implements IVsCodeWindow
                return VSConstants.S_OK;
            }
            else
            {
                return VSConstants.E_NOTIMPL;   // you must return E_NOTIMPL for any unrecognized rguidLogicalView values
            }
        }

        public int Close()
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Used by the editor factory to create an editor instance. the environment first determines the 
        /// editor factory with the highest priority for opening the file and then calls 
        /// IVsEditorFactory.CreateEditorInstance. If the environment is unable to instantiate the document data 
        /// in that editor, it will find the editor with the next highest priority and attempt to so that same 
        /// thing. 
        /// NOTE: The priority of our editor is 32 as mentioned in the attributes on the package class.
        /// 
        /// Since our editor supports opening only a single view for an instance of the document data, if we 
        /// are requested to open document data that is already instantiated in another editor, or even our 
        /// editor, we return a value VS_E_INCOMPATIBLEDOCDATA.
        /// </summary>
        /// <param name="grfCreateDoc">Flags determining when to create the editor. Only open and silent flags 
        /// are valid
        /// </param>
        /// <param name="pszMkDocument">path to the file to be opened</param>
        /// <param name="pszPhysicalView">name of the physical view</param>
        /// <param name="pvHier">pointer to the IVsHierarchy interface</param>
        /// <param name="itemid">Item identifier of this editor instance</param>
        /// <param name="punkDocDataExisting">This parameter is used to determine if a document buffer 
        /// (DocData object) has already been created
        /// </param>
        /// <param name="ppunkDocView">Pointer to the IUnknown interface for the DocView object</param>
        /// <param name="ppunkDocData">Pointer to the IUnknown interface for the DocData object</param>
        /// <param name="pbstrEditorCaption">Caption mentioned by the editor for the doc window</param>
        /// <param name="pguidCmdUI">the Command UI Guid. Any UI element that is visible in the editor has 
        /// to use this GUID. This is specified in the .vsct file
        /// </param>
        /// <param name="pgrfCDW">Flags for CreateDocumentWindow</param>
        /// <returns></returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public int CreateEditorInstance(
                        uint grfCreateDoc,
                        string pszMkDocument,
                        string pszPhysicalView,
                        IVsHierarchy pvHier,
                        uint itemid,
                        System.IntPtr punkDocDataExisting,
                        out System.IntPtr ppunkDocView,
                        out System.IntPtr ppunkDocData,
                        out string pbstrEditorCaption,
                        out Guid pguidCmdUI,
                        out int pgrfCDW)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering {0} CreateEditorInstace()", this.ToString()));

            // Initialize to null
            ppunkDocView = IntPtr.Zero;
            ppunkDocData = IntPtr.Zero;
            pguidCmdUI = GuidList.guidDsLightPackageEditorFactory;
            pgrfCDW = 0;
            pbstrEditorCaption = null;

            // Validate inputs
            if ((grfCreateDoc & (VSConstants.CEF_OPENFILE | VSConstants.CEF_SILENT)) == 0)
            {
                return VSConstants.E_INVALIDARG;
            }
            if (punkDocDataExisting != IntPtr.Zero)
            {
                return VSConstants.VS_E_INCOMPATIBLEDOCDATA;
            }

            // find the IVsProject to which the editor belongs
            IVsProject currentProject = null;
            IVsSolution solution = (IVsSolution)GetService(typeof(SVsSolution));

            // enumerate all projects in solution
            foreach (IVsProject project in solution.GetLoadedProjects())
            {
                // check if the project contains the current editor item
                if (project.IsDocumentInProject2(pszMkDocument) == itemid)
                {
                    currentProject = project;
                    break;
                }
            }

            // get type of project in which the data set is contained
            EditorPane.SettingsModel settingsModel = EditorPane.SettingsModel.Unknown;
            object pvar;
            ((IVsHierarchy)currentProject).GetProperty((uint)Microsoft.VisualStudio.VSConstants.VSITEMID.Root, (int)__VSHPROPID5.VSHPROPID_OutputType, out pvar);
            var outputType = (__VSPROJOUTPUTTYPE)Convert.ToInt32(pvar);

            if (outputType == __VSPROJOUTPUTTYPE.VSPROJ_OUTPUTTYPE_WINEXE)
            {
                // WinForms or WPF: store connection string in settings file
                settingsModel = EditorPane.SettingsModel.Settings;
            }
            else if (outputType == __VSPROJOUTPUTTYPE.VSPROJ_OUTPUTTYPE_EXE)
            {
                // Console: store connection string in settings file
                settingsModel = EditorPane.SettingsModel.Settings;
            }
            else if (outputType == __VSPROJOUTPUTTYPE.VSPROJ_OUTPUTTYPE_LIBRARY)
            {
                // DLL or web
                var aggregatableProject = (Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject)currentProject;
                string projectTypeGuids;
                aggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuids);
                projectTypeGuids = projectTypeGuids.ToUpper();
                if (projectTypeGuids.Contains("{349C5851-65DF-11DA-9384-00065B846F21}"))
                {
                    // WebApplication: store connection string in web.config
                    settingsModel = EditorPane.SettingsModel.WebConfig;
                }
                else if (projectTypeGuids.Contains("{E24C65DC-7377-472B-9ABA-BC803B73C61A}"))
                {
                    // web site: store connection string in web.config
                    settingsModel = EditorPane.SettingsModel.WebConfig;
                }
                else
                {
                    // DLL: store connection string in settings file
                    settingsModel = EditorPane.SettingsModel.Settings;
                }
            }
            else
            {
                MessageBox.Show(
                    String.Format("DataSet Light does support this type of project ({0}).", outputType),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return VSConstants.E_NOTIMPL;
            }
           
            // Create the Document (editor)
            EditorPane NewEditor = new EditorPane(editorPackage, currentProject, settingsModel);
            ppunkDocView = Marshal.GetIUnknownForObject(NewEditor);
            ppunkDocData = Marshal.GetIUnknownForObject(NewEditor);
            pbstrEditorCaption = "";
            return VSConstants.S_OK;
        }

        #endregion

    }
}
