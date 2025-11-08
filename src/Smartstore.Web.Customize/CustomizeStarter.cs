using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Smartstore.Core.Content.Menus;
using Smartstore.Engine;
using Smartstore.Engine.Builders;
using Smartstore.Web.Infrastructure.Menus;

namespace Smartstore.Web.Customize
{
    public class CustomizeStarter : StarterBase
    {
        public override int Order => StarterOrdering.Last + 1;
        public override void ConfigureServices(IServiceCollection services, IApplicationContext appContext)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Insert(0, new CustomizeViewLocationExpander());
            });
        }
        public override bool Matches(IApplicationContext appContext)
        {
            // Only run when the application is installed
            return appContext.IsInstalled;
        }
    }
}
