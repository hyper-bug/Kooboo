﻿using Kooboo.Api;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; 

namespace Kooboo.Web.Api.Implementation
{
    public class CssOptimization : IApi
    {
        public string ModelName => "CssOptimization";

        public bool RequireSite => true;

        public bool RequireUser => true;

        public List<OptimizationViewModel> List(ApiCall call)
        {
            var site = call.WebSite;

            List<OptimizationViewModel> result = new List<OptimizationViewModel>();

            var unusedList = Kooboo.Sites.Service.CleanerService.GetUnusedRules(call.Context).Result;

            foreach (var item in unusedList)
            {
                var model = new OptimizationViewModel();
                model.Id = item.Id;
                model.Content = item.CssText;
                result.Add(model);
            } 
            return result; 
        }

        public void Delete(ApiCall call, Guid[] ids)
        {
            var sitedb = call.WebSite.SiteDb();
            List<CmsCssRule> rules = new List<CmsCssRule>(); 

            foreach (var item in ids)
            {
                var rule = sitedb.CssRules.Get(item); 
                if (rule !=null)
                {
                    rules.Add(rule); 
                }
            }

            foreach (var item in rules.GroupBy(o=>o.ParentStyleId))
            {
                var rulelist = item.ToList();

                List<CmsCssRuleChanges> changes = new List<CmsCssRuleChanges>();

                foreach (var delRule in rulelist)
                {
                    CmsCssRuleChanges change = new CmsCssRuleChanges();
                    change.CssRuleId = delRule.Id;
                    change.ChangeType = ChangeType.Delete;
                    changes.Add(change); 
                }

                sitedb.CssRules.UpdateStyle(changes, item.Key);  
            } 
             
        }
    }

    public class OptimizationViewModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

    }
}
