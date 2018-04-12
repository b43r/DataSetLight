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

namespace deceed.DsLightPackage
{
    static class GuidList
    {
        public const string guidDsLightPackagePkgString = "149b837d-391c-43ea-95f0-2f2d2567eb7f";
        public const string guidDsLightPackageCmdSetString = "5735e084-de41-45c1-89ec-328c544cbc1d";
        public const string guidDsLightPackageEditorFactoryString = "543ab780-591b-4975-9a4c-1a9a308afa9b";

        public static readonly Guid guidDsLightPackageCmdSet = new Guid(guidDsLightPackageCmdSetString);
        public static readonly Guid guidDsLightPackageEditorFactory = new Guid(guidDsLightPackageEditorFactoryString);
    }
}