﻿using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Data.Language;

namespace Kooboo.Web.Menus.SideBarMenu.System
{
    public class Text : ISideBarMenu
    {
        public EnumSideBarParent Parent => EnumSideBarParent.System;

        public string Name => "Text";

        public string Icon => "";

        public string Url => "System/KConfig";

        public int Order => 3;

        public List<ICmsMenu> SubItems { get; set; }

        public string GetDisplayName(RenderContext Context)
        {
            return Hardcoded.GetValue("Text", Context);
        }
    }
}

//new MenuItem{ Name = Hardcoded.GetValue("Text",context), Url = AdminUrl("System/KConfig", siteDb), ActionRights = Sites.Authorization.Actions.Systems.Configs }
