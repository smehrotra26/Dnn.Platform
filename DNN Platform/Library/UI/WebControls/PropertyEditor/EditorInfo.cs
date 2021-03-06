﻿// 
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 
#region Usings

using System;
using System.Web.UI.WebControls;

using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Profile;

#endregion

namespace DotNetNuke.UI.WebControls
{
    /// -----------------------------------------------------------------------------
    /// Project:    DotNetNuke
    /// Namespace:  DotNetNuke.UI.WebControls
    /// Class:      EditorInfo
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditorInfo class provides a helper class for the Property Editor
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// -----------------------------------------------------------------------------
    public class EditorInfo
    {
        public EditorInfo()
        {
            Visible = true;
        }

        public object[] Attributes { get; set; }

        public string Category { get; set; }

        public Style ControlStyle { get; set; }

        public PropertyEditorMode EditMode { get; set; }

        public string Editor { get; set; }

        public LabelMode LabelMode { get; set; }

        public string Name { get; set; }

        public bool Required { get; set; }

        public string ResourceKey { get; set; }

        public string Type { get; set; }

        public UserInfo User { get; set; }

        public object Value { get; set; }

        public string ValidationExpression { get; set; }

        public bool Visible { get; set; }

        public ProfileVisibility ProfileVisibility { get; set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetEditor gets the appropriate Editor based on ID
        /// properties
        /// </summary>
        /// <param name="editorType">The Id of the Editor</param>
        /// -----------------------------------------------------------------------------
        public static string GetEditor(int editorType)
        {
            string editor = "UseSystemType";
            if (editorType != Null.NullInteger)
            {
                var objListController = new ListController();
                ListEntryInfo definitionEntry = objListController.GetListEntryInfo("DataType", editorType);
                if (definitionEntry != null)
                {
                    editor = definitionEntry.TextNonLocalized;
                }
            }
            return editor;
        }

        public static string GetEditor(string editorValue)
        {
            string editor = "UseSystemType";
            var objListController = new ListController();
            ListEntryInfo definitionEntry = objListController.GetListEntryInfo("DataType", editorValue);
            if (definitionEntry != null)
            {
                editor = definitionEntry.TextNonLocalized;
            }
            return editor;
        }
    }
}
