using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;
using Nop.Plugin.Misc.CategorySpecAttribute.Services;
using Nop.Plugin.Misc.CategorySpecificationAttribute.Models;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
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
    public class AddProductSpecificationAttributeModel
    {
        public int ProductId { get; set; }
        public int[] SpecificationAttributeOptionIds { get; set; }
    }

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
        private readonly ICategoryService _categoryService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkContext _workContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly VendorSettings _vendorSettings;

        public CategorySpecificationAttributeController(IPermissionService permissionService, ISpecificationAttributeService specificationAttributeService, IProductService productService, IProductModelFactory productModelFactory, ICategorySpecificationAttributeService categorySpecificationAttributeService, INotificationService notificationService, ICategoryService categoryService, ILocalizationService localizationService, ICustomerActivityService customerActivityService, IWorkContext workContext, IUrlRecordService urlRecordService, VendorSettings vendorSettings)
        {
            _permissionService = permissionService;
            _specificationAttributeService = specificationAttributeService;
            _productService = productService;
            _productModelFactory = productModelFactory;
            _categorySpecificationAttributeService = categorySpecificationAttributeService;
            _notificationService = notificationService;
            _categoryService = categoryService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
            _workContext = workContext;
            _urlRecordService = urlRecordService;
            _vendorSettings = vendorSettings;
        }

        protected virtual async Task SaveCategoryMappingsAsync(Product product, ProductModel model)
        {
            var existingProductCategories = await _categoryService.GetProductCategoriesByProductIdAsync(product.Id, true);

            //delete categories
            foreach (var existingProductCategory in existingProductCategories)
                if (!model.SelectedCategoryIds.Contains(existingProductCategory.CategoryId))
                    await _categoryService.DeleteProductCategoryAsync(existingProductCategory);

            //add categories
            foreach (var categoryId in model.SelectedCategoryIds)
            {
                if (_categoryService.FindProductCategory(existingProductCategories, product.Id, categoryId) == null)
                {
                    //find next display order
                    var displayOrder = 1;
                    var existingCategoryMapping = await _categoryService.GetProductCategoriesByCategoryIdAsync(categoryId, showHidden: true);
                    if (existingCategoryMapping.Any())
                        displayOrder = existingCategoryMapping.Max(x => x.DisplayOrder) + 1;
                    await _categoryService.InsertProductCategoryAsync(new ProductCategory
                    {
                        ProductId = product.Id,
                        CategoryId = categoryId,
                        DisplayOrder = displayOrder
                    });
                }
            }
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
        public async Task<IActionResult> GetSpecificationAttributesByCategoryId(int categoryId, bool includeNonGroupedAttributes = true)
        {
            var categorySpecAttributeGroups = await _categorySpecificationAttributeService.GetByCategoryIdAsync(categoryId);
            var result = (from c in categorySpecAttributeGroups
                          select new
                          {
                              name = _specificationAttributeService.GetSpecificationAttributeGroupByIdAsync(c.SpecificationAttributeGroupId).Result.Name,
                              specificationAttributes = (from s in _specificationAttributeService.GetSpecificationAttributesByGroupIdAsync(c.SpecificationAttributeGroupId).Result
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

            result.Add(new
            {
                name = "default",
                specificationAttributes = (from s in _specificationAttributeService.GetSpecificationAttributesByGroupIdAsync().Result
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
            });
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

        [HttpPost]
        public virtual async Task<IActionResult> ProductSpecificationAttributeAdd(AddProductSpecificationAttributeModel model)
        {
            try
            {
                if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                    return AccessDeniedView();

                var product = await _productService.GetProductByIdAsync(model.ProductId);
                if (product == null)
                {
                    return BadRequest("No product found with the specified id");
                }

                if (await _workContext.GetCurrentVendorAsync() != null && product.VendorId != (await _workContext.GetCurrentVendorAsync()).Id)
                {
                    return BadRequest();
                }

                var specAttributeOptions = await _specificationAttributeService.GetSpecificationAttributeOptionsByIdsAsync(model.SpecificationAttributeOptionIds);
                foreach (var option in specAttributeOptions)
                {
                    var addModel = new AddSpecificationAttributeModel()
                    {
                        ProductId = model.ProductId,
                        AllowFiltering = true,
                        AttributeTypeId = (int)SpecificationAttributeType.Option,
                        DisplayOrder = 0,
                        ShowOnProductPage = true,
                        SpecificationAttributeOptionId = option.Id,
                    };
                    var psa = addModel.ToEntity<ProductSpecificationAttribute>();
                    //var psa = new ProductSpecificationAttribute()
                    //{
                    //    AllowFiltering = true,
                    //    AttributeType = SpecificationAttributeType.Option,
                    //    SpecificationAttributeOptionId = option.Id,
                    //    DisplayOrder = 0,
                    //    ShowOnProductPage = true,
                    //    ProductId = productId
                    //};
                    await _specificationAttributeService.InsertProductSpecificationAttributeAsync(psa);
                }
                return Json("ok");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateProduct(ProductModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //validate maximum number of products per vendor
            if (_vendorSettings.MaximumProductNumber > 0 && await _workContext.GetCurrentVendorAsync() != null
                && await _productService.GetNumberOfProductsByVendorIdAsync((await _workContext.GetCurrentVendorAsync()).Id) >= _vendorSettings.MaximumProductNumber)
            {
                _notificationService.ErrorNotification(string.Format(await _localizationService.GetResourceAsync("Admin.Catalog.Products.ExceededMaximumNumber"),
                    _vendorSettings.MaximumProductNumber));
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {
                //a vendor should have access only to his products
                if (await _workContext.GetCurrentVendorAsync() != null)
                    model.VendorId = (await _workContext.GetCurrentVendorAsync()).Id;

                //vendors cannot edit "Show on home page" property
                if (await _workContext.GetCurrentVendorAsync() != null && model.ShowOnHomepage)
                    model.ShowOnHomepage = false;

                model.Published = true;

                //product
                var product = model.ToEntity<Product>();
                product.CreatedOnUtc = DateTime.UtcNow;
                product.UpdatedOnUtc = DateTime.UtcNow;
                await _productService.InsertProductAsync(product);

                //search engine name
                model.SeName = await _urlRecordService.ValidateSeNameAsync(product, model.SeName, product.Name, true);
                await _urlRecordService.SaveSlugAsync(product, model.SeName, 0);

                //categories
                await SaveCategoryMappingsAsync(product, model);

                //quantity change history
                await _productService.AddStockQuantityHistoryEntryAsync(product, product.StockQuantity, product.StockQuantity, product.WarehouseId,
                    await _localizationService.GetResourceAsync("Admin.StockQuantityHistory.Messages.Edit"));

                //activity log
                await _customerActivityService.InsertActivityAsync("AddNewProduct",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewProduct"), product.Name), product);

                return Json(product.Id);
            }

            //if we got this far, something failed.
            return BadRequest();
        }
    }
}
