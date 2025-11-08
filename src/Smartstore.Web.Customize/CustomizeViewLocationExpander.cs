using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Smartstore.Web.Customize
{
    public class CustomizeViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["customization"] = "true";
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            var areaName = context.AreaName;
            var customizeLocations = new List<string>();

            if (!string.IsNullOrEmpty(areaName))
            {
                // Area-specific customize locations with highest priority
                customizeLocations.AddRange(
                [
                    $"/Areas/Customize/{areaName}/Views/Shared/Components/{{1}}/{{0}}.cshtml",
                    $"/Areas/Customize/{areaName}/Views/Shared/Components/{{0}}/{{1}}.cshtml",
                    $"/Areas/Customize/{areaName}/Views/Shared/{{0}}.cshtml",
                    $"/Areas/Customize/{areaName}/Views/{{1}}/{{0}}.cshtml",
                    
                    // Fall back to general customize area locations
                    "/Areas/Customize/Views/Shared/Components/{1}/{0}.cshtml",
                    "/Areas/Customize/Views/Shared/Components/{0}/{1}.cshtml",
                    "/Areas/Customize/Views/Shared/{0}.cshtml",
                    "/Areas/Customize/Views/{1}/{0}.cshtml"
                ]);
            }
            else
            {
                // Non-area customize locations with highest priority
                customizeLocations.AddRange(
                [
                    "/Views/Customize/Shared/Components/{1}/{0}.cshtml",
                    "/Views/Customize/Shared/Components/{0}/{1}.cshtml",
                    "/Views/Customize/Shared/{0}.cshtml",
                    "/Views/Customize/{1}/{0}.cshtml"
                ]);
            }


            return customizeLocations.Concat(viewLocations);
        }
    }
}
