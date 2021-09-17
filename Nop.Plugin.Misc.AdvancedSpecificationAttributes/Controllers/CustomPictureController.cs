using Microsoft.AspNetCore.Mvc;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Controllers
{
    public class CustomPictureController : BasePluginController
    {
        #region Fields

        private readonly IPictureService _pictureService;

        #endregion

        #region Ctor

        public CustomPictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        #endregion

        #region Methods

        [HttpPost]
        //do not validate request token (XSRF)
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Upload()
        {
            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
                return Json(new { success = false, message = "No file uploaded" });

            var picture = await _pictureService.InsertPictureAsync(httpPostedFile, string.Empty);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.

            if (picture == null)
                return Json(new { success = false, message = "Wrong file format" });

            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = (await _pictureService.GetPictureUrlAsync(picture, 100)).Url
            });
        }

        #endregion
    }
}
