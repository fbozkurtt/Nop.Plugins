using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;
using Nop.Plugin.Misc.CategorySpecAttribute.Services;
using Nop.Plugin.Misc.CategorySpecificationAttribute.Models;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.CategorySpecificationAttribute.Controllers
{
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    public class CategorySpecificationAttributeController : BasePluginController
    {
        private readonly IPermissionService _permissionService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IProductService _productService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly ICategorySpecificationAttributeService _categorySpecificationAttributeService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly VendorSettings _vendorSettings;

        public CategorySpecificationAttributeController(
            IPermissionService permissionService,
            ISpecificationAttributeService specificationAttributeService,
            IProductService productService,
            IProductModelFactory productModelFactory,
            ICategorySpecificationAttributeService categorySpecificationAttributeService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            VendorSettings vendorSettings)
        {
            _permissionService = permissionService;
            _specificationAttributeService = specificationAttributeService;
            _productService = productService;
            _productModelFactory = productModelFactory;
            _categorySpecificationAttributeService = categorySpecificationAttributeService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _workContext = workContext;
            _vendorSettings = vendorSettings;
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(SpecificationAttributeGroupCategoryModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAttributes))
                return Unauthorized();

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
                return Unauthorized();

            await _categorySpecificationAttributeService.CreateAsync(model);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecificationAttributesByCategoryId(int categoryId)
        {
            var categorySpecAttributeGroups = await _categorySpecificationAttributeService.GetByCategoryIdAsync(categoryId);
            var result = (from c in categorySpecAttributeGroups
                          select new
                          {
                              id = c.Id,
                              name = _specificationAttributeService.GetSpecificationAttributeGroupByIdAsync(c.SpecificationAttributeGroupId).Result.Name,
                              specificationAttributes = (from s in _specificationAttributeService.GetSpecificationAttributesByGroupIdAsync(c.Id).Result
                                                         select new
                                                         {
                                                             name = s.Name,
                                                             options = (from o in _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttributeAsync(s.Id).Result
                                                                        select new
                                                                        {
                                                                            id = o.Id,
                                                                            name = o.Name
                                                                        }).ToList()
                                                         }).ToList()
                          }).ToList();
            return Json(result);
        }

        public virtual async Task<IActionResult> CreateProduct()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //validate maximum number of products per vendor
            if (_vendorSettings.MaximumProductNumber > 0 && await _workContext.GetCurrentVendorAsync() != null
                && await _productService.GetNumberOfProductsByVendorIdAsync((await _workContext.GetCurrentVendorAsync()).Id) >= _vendorSettings.MaximumProductNumber)
            {
                _notificationService.ErrorNotification(string.Format(await _localizationService.GetResourceAsync("Admin.Catalog.Products.ExceededMaximumNumber"),
                _vendorSettings.MaximumProductNumber));
                return RedirectToAction("List", "Product");
            }

            //prepare model
            var model = await _productModelFactory.PrepareProductModelAsync(new ProductModel(), null);

            return View("~/Plugins/Misc.CategorySpecificationAttribute/Views/CreateProduct.cshtml", model);
        }
    }
}
