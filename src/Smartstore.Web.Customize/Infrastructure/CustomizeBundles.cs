using Smartstore.Engine;
using Smartstore.Web.Bundling;

namespace Smartstore.Web.Customize.Infrastructure
{
    public class CustomizeBundles : IBundleProvider
    {
        public int Priority => 200;

        public void RegisterBundles(IApplicationContext appContext, IBundleCollection bundles)
        {
            if (!appContext.IsInstalled)
            {
                return;
            }
            var lib = "/customize/";
            bundles.Add(new StyleBundle("/bundle/css/site-customization.css").Include(
                lib + "site.css"));
        }
    }
}
