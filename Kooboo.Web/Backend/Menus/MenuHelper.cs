//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using Kooboo.Data.Context;
using Kooboo.Sites.Repository;
using Kooboo.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Kooboo.Lib;
using System.Reflection;
using Kooboo.Web.Menus;

namespace Kooboo.Web.Menus
{
    public static class MenuHelper
    { 
        public static string AdminUrl(string relativeUrl)
        {
            return "/_Admin/" + relativeUrl;
        } 

        public static string AdminUrl(string relativeUrl, SiteDb siteDb)
        {
            Dictionary<string, string> para = new Dictionary<string, string>();
            if (siteDb != null)
            {
                para.Add("SiteId", siteDb.Id.ToString());
            }
            return Kooboo.Lib.Helper.UrlHelper.AppendQueryString("/_Admin/" + relativeUrl, para);
        }

        internal static MenuItem ConvertToOld(ICmsMenu menu, RenderContext Context)
        {
            Guid siteid = default(Guid);
            if (Context.WebSite != null)
            {
                siteid = Context.WebSite.Id;
            }

            if (menu != null && CanShow(menu, Context))
            {
                MenuItem result = new MenuItem();
                result.Icon = menu.Icon;
                result.Name = menu.Name;
                result.DisplayName = menu.GetDisplayName(Context);
                result.Icon = menu.Icon;
                if (siteid == default(Guid))
                {
                    result.Url = menu.Url;
                }
                else
                {
                    Dictionary<string, string> para = new Dictionary<string, string>();
                    para.Add("SiteId", siteid.ToString());
                    result.Url = Kooboo.Lib.Helper.UrlHelper.AppendQueryString(menu.Url, para); 
                } 

                //if (menu.Items != null && menu.Items.Any())
                //{
                //    foreach (var item in menu.Items)
                //    {
                //        var menuitem = ConvertToOld(item, Context);
                //        if (menuitem != null)
                //        {
                //            result.Items.Add(menuitem);
                //        }
                //    }
                //}
                return result;
            }

            return null;

        }

        public static bool CanShow(ICmsMenu menu, RenderContext context)
        {
            return true;
        }

        public static string ApiUrl<TApi>(string methodname) where TApi: Kooboo.Api.IApi
        {
            var modelname = GetApiName(typeof(TApi));
            return ApiUrl(modelname, methodname); 
        } 

        public static string ApiUrl(string modelname, string methodname)
        { 
            return "/_api/" + modelname + "/" + methodname;
        }

        public static string SiteObjectApiUrl<TSiteModel>(string methodname) where TSiteModel : Kooboo.Data.Interface.ISiteObject
        {
            var apiprovider = Web.SystemStart.CurrentApiProvider;

            var modelname = typeof(TSiteModel).Name; 

            foreach (var item in apiprovider.List)
            {
               if (item.Value.ModelName == modelname)
                {
                    return ApiUrl(modelname, methodname); 
                }
            }
            return null; 
        }

        private static string GetApiName(Type ApiType)
        {
            var method = ApiType.GetProperty("ModelName").GetGetMethod();
            var dynamicMethod = new DynamicMethod("meide", typeof(string),
                                                  Type.EmptyTypes);
            var generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldnull);
            generator.Emit(OpCodes.Call, method);
            generator.Emit(OpCodes.Ret);
            var silly = (Func<string>)dynamicMethod.CreateDelegate(
                           typeof(Func<string>));
            return silly();
        } 
         

    }
}
