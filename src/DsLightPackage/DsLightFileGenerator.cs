/*************************************************************************** 
 
Copyright (c) Microsoft Corporation. All rights reserved. 
This code is licensed under the Visual Studio SDK license terms. 
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF 
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY 
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR 
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT. 
 
***************************************************************************/

using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Designer.Interfaces;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSLangProj80;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

using deceed.DsLight.EditorGUI.Model;

namespace deceed.DsLightPackage
{
    [ComVisible(true)]
    [Guid(GuidList.guidDsLightPackagePkgString)]
    [ProvideObject(typeof(DsLightFileGenerator))]
    [CodeGeneratorRegistration(typeof(DsLightFileGenerator), "DsLightFileGenerator", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
    public class DsLightFileGenerator : IVsSingleFileGenerator, IObjectWithSite, IDisposable
    {
        private object site = null;
        private CodeDomProvider codeDomProvider = null;
        private ServiceProvider serviceProvider = null;

        /// <summary>
        /// Gets the code provider.
        /// </summary>
        private CodeDomProvider CodeProvider
        {
            get
            {
                if (codeDomProvider == null)
                {
                    IVSMDCodeDomProvider provider = (IVSMDCodeDomProvider)SiteServiceProvider.GetService(typeof(IVSMDCodeDomProvider).GUID);
                    if (provider != null)
                    {
                        codeDomProvider = (CodeDomProvider)provider.CodeDomProvider;
                    }
                }
                return codeDomProvider;
            }
        }

        /// <summary>
        /// Gets the site service provider.
        /// </summary>
        private ServiceProvider SiteServiceProvider
        {
            get
            {
                if (serviceProvider == null)
                {
                    IOleServiceProvider oleServiceProvider = site as IOleServiceProvider;
                    serviceProvider = new ServiceProvider(oleServiceProvider);
                }
                return serviceProvider;
            }
        }

        #region IVsSingleFileGenerator

        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = "." + CodeProvider.FileExtension;
            return VSConstants.S_OK;
        }

        public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            if (bstrInputFileContents == null)
            {
                throw new ArgumentException(bstrInputFileContents);
            }
            if (CodeProvider.FileExtension != "cs")
            {
                throw new ArgumentException("File extension must be .cs!");
            }

            // load XML data
            DataModel dataModel = new DataModel();
            dataModel.Load(bstrInputFileContents);

            // generate .cs file
            CodeGenerator generator = new CodeGenerator(dataModel, wszDefaultNamespace + "." + Path.GetFileNameWithoutExtension(wszInputFilePath));
            string content = generator.Generate();

            byte[] bytes = Encoding.UTF8.GetBytes(content);
            if (bytes == null)
            {
                rgbOutputFileContents[0] = IntPtr.Zero;
                pcbOutput = 0;
            }
            else
            {
                rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(bytes.Length);
                Marshal.Copy(bytes, 0, rgbOutputFileContents[0], bytes.Length);
                pcbOutput = (uint)bytes.Length;
            }

            return VSConstants.S_OK;
        }

        #endregion IVsSingleFileGenerator

        #region IObjectWithSite

        public void GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (site == null)
            {
                Marshal.ThrowExceptionForHR(VSConstants.E_NOINTERFACE);
            }

            // Query for the interface using the site object initially passed to the generator 
            IntPtr punk = Marshal.GetIUnknownForObject(site);
            int hr = Marshal.QueryInterface(punk, ref riid, out ppvSite);
            Marshal.Release(punk);
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(hr);
        }

        public void SetSite(object pUnkSite)
        {
            // Save away the site object for later use 
            site = pUnkSite;

            // These are initialized on demand via our private CodeProvider and SiteServiceProvider properties 
            codeDomProvider = null;
            serviceProvider = null;
        }

        #endregion IObjectWithSite

        #region IDisposable Members

        /// <summary>
        /// Release ressources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release ressources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (codeDomProvider != null)
                {
                    codeDomProvider.Dispose();
                    codeDomProvider = null;
                }
                if (serviceProvider != null)
                {
                    serviceProvider.Dispose();
                    serviceProvider = null;
                }
            }
        }

        #endregion
    }
}