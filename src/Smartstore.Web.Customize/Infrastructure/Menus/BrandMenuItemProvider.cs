using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smartstore.Collections;
using Smartstore.Core.Catalog.Brands;
using Smartstore.Core.Content.Menus;
using Smartstore.Core.Data;
using Smartstore.Core.Localization;
using Smartstore.Core.Seo;

namespace Smartstore.Web.Infrastructure.Menus
{
    [MenuItemProvider("brands", AppendsMultipleItems = true)]
    public class BrandMenuItemProvider : MenuItemProviderBase
    {
        private readonly SmartDbContext _db;
        private readonly IUrlHelper _urlHelper;

        public BrandMenuItemProvider(SmartDbContext db, IUrlHelper urlHelper)
        {
            _db = db;
            _urlHelper = urlHelper;
        }

        public override async Task<TreeNode<MenuItem>> AppendAsync(MenuItemProviderRequest request)
        {
            // Early exit if the menu item doesn't exist yet (before migration)
            if (request?.Entity == null)
            {
                return null;
            }
            
            if (request.IsEditMode)
            {
                var item = ConvertToMenuItem(request);
                item.Summary = T("Providers.MenuItems.FriendlyName.Brands");
                item.Icon = "fas fa-tags";

                AppendToParent(request, item);
                return null;
            }

            // Add group header if specified
            if (request.Entity.BeginGroup)
            {
                AppendToParent(request, new MenuItem
                {
                    IsGroupHeader = true,
                    Text = request.Entity.GetLocalized(x => x.ShortDescription)
                });
            }

            // Get manufacturers
            var manufacturers = await _db.Manufacturers
                .AsNoTracking()
                .ApplyStandardFilter(true)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            // Create brand parent menu item
            var brandsMenuItem = new MenuItem
            {
                Id = "brands",
                Text = request.Entity.GetLocalized(x => x.Title) ?? "Brands",
                MenuItemId = request.Entity.Id,
                MenuId = request.Entity.MenuId,
                Visible = request.Entity.Published,
                PermissionNames = request.Entity.PermissionNames
            };

            // Add link to view all manufacturers
            brandsMenuItem.RouteName = "ManufacturerAll";

            var brandsNode = AppendToParent(request, brandsMenuItem);

            // Add manufacturers as children
            foreach (var manufacturer in manufacturers)
            {
                var manufacturerItem = new MenuItem
                {
                    Id = $"manufacturer-{manufacturer.Id}",
                    Text = manufacturer.GetLocalized(x => x.Name),
                    EntityId = manufacturer.Id,
                    EntityName = nameof(Manufacturer),
                    RouteName = "Manufacturer",
                    ImageId = manufacturer.MediaFileId,
                    Visible = true
                };

                manufacturerItem.RouteValues.Add("SeName", await manufacturer.GetActiveSlugAsync());

                brandsNode.Append(manufacturerItem);
            }

            return null; // Don't traverse children
        }

        protected override Task ApplyLinkAsync(MenuItemProviderRequest request, TreeNode<MenuItem> node)
        {
            return Task.CompletedTask;
        }
    }
}