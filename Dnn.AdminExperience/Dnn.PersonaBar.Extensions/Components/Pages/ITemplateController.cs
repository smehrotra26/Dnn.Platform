﻿// 
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 
using System.Collections.Generic;
using Dnn.PersonaBar.Pages.Components.Dto;
using Dnn.PersonaBar.Pages.Services.Dto;
using DotNetNuke.Entities.Tabs;

namespace Dnn.PersonaBar.Pages.Components
{
    public interface ITemplateController
    {
        string SaveAsTemplate(PageTemplate template);

        IEnumerable<Template> GetTemplates();
        int GetDefaultTemplateId(IEnumerable<Template> templates);
        void CreatePageFromTemplate(int templateId, TabInfo tab, int portalId);
    }
}
