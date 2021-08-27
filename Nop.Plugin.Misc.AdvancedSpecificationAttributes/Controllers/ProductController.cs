using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Controllers
{
    [AutoValidateAntiforgeryToken]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    public class ProductController : BasePluginController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IProductService _productService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkContext _workContext;
        private readonly IPictureService _pictureService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly VendorSettings _vendorSettings;

        #endregion

        #region Ctor

        public ProductController(IPermissionService permissionService, ISpecificationAttributeService specificationAttributeService, IProductService productService, IProductModelFactory productModelFactory, ICategoryService categoryService, ILocalizationService localizationService, ICustomerActivityService customerActivityService, IWorkContext workContext, IPictureService pictureService, IUrlRecordService urlRecordService, VendorSettings vendorSettings)
        {
            _permissionService = permissionService;
            _specificationAttributeService = specificationAttributeService;
            _productService = productService;
            _productModelFactory = productModelFactory;
            _categoryService = categoryService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
            _workContext = workContext;
            _pictureService = pictureService;
            _urlRecordService = urlRecordService;
            _vendorSettings = vendorSettings;
        }

        #endregion

        #region Utilities
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
        #endregion

        #region Methods

        #region Product Create
        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //validate maximum number of products per vendor
            if (_vendorSettings.MaximumProductNumber > 0 && await _workContext.GetCurrentVendorAsync() != null
                && await _productService.GetNumberOfProductsByVendorIdAsync((await _workContext.GetCurrentVendorAsync()).Id) >= _vendorSettings.MaximumProductNumber)
            {
                return RedirectToAction("List", "Product");
            }

            //prepare model
            var model = await _productModelFactory.PrepareProductModelAsync(new ProductModel(), null);

            return View("~/Plugins/Misc.AdvancedSpecificationAttributes/Views/Product/Create.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create(ProductModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return Unauthorized();

            //validate maximum number of products per vendor
            if (_vendorSettings.MaximumProductNumber > 0 && await _workContext.GetCurrentVendorAsync() != null
                && await _productService.GetNumberOfProductsByVendorIdAsync((await _workContext.GetCurrentVendorAsync()).Id) >= _vendorSettings.MaximumProductNumber)
            {
                return BadRequest();
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
                model.ProductTypeId = (int)ProductType.SimpleProduct;
                model.ProductTemplateId = 1;

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

                return Ok(product.Id);
            }

            //if we got this far, something failed.
            return BadRequest();
        }

        #endregion

        #region Product specification attributes

        [HttpPost]
        public virtual async Task<IActionResult> ProductSpecificationAttributeAdd(AddProductSpesificationAttributeOptionsModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return Unauthorized();

            var product = await _productService.GetProductByIdAsync(model.ProductId);
            if (product == null)
            {
                return BadRequest();
            }

            if (await _workContext.GetCurrentVendorAsync() != null && product.VendorId != (await _workContext.GetCurrentVendorAsync()).Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
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

                    await _specificationAttributeService.InsertProductSpecificationAttributeAsync(psa);
                }
                return Ok();
            }
            //if we got this far, something failed.
            return BadRequest();
        }

        #endregion

        #region Product picture

        public virtual async Task<IActionResult> ProductPictureAdd(int pictureId, int displayOrder,
            string overrideAltAttribute, string overrideTitleAttribute, int productId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (pictureId == 0)
                throw new ArgumentException();

            //try to get a product with the specified id
            var product = await _productService.GetProductByIdAsync(productId)
                ?? throw new ArgumentException("No product found with the specified id");

            //a vendor should have access only to his products
            if (await _workContext.GetCurrentVendorAsync() != null && product.VendorId != (await _workContext.GetCurrentVendorAsync()).Id)
                return RedirectToAction("List");

            if ((await _productService.GetProductPicturesByProductIdAsync(productId)).Any(p => p.PictureId == pictureId))
                return Json(new { Result = false });

            //try to get a picture with the specified id
            var picture = await _pictureService.GetPictureByIdAsync(pictureId)
                ?? throw new ArgumentException("No picture found with the specified id");

            await _pictureService.UpdatePictureAsync(picture.Id,
                await _pictureService.LoadPictureBinaryAsync(picture),
                picture.MimeType,
                picture.SeoFilename,
                overrideAltAttribute,
                overrideTitleAttribute);

            await _pictureService.SetSeoFilenameAsync(pictureId, await _pictureService.GetPictureSeNameAsync(product.Name));

            await _productService.InsertProductPictureAsync(new ProductPicture
            {
                PictureId = pictureId,
                ProductId = productId,
                DisplayOrder = displayOrder
            });

            return Json(new { Result = true });
        }

        #endregion

        #endregion
    }
}
