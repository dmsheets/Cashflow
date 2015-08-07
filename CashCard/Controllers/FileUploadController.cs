using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mime;

namespace CashCard.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /FileUpload/
     
        //public FilePathResult Image()
        //{
        //    string filename = Request.Url.AbsolutePath.Replace("/home/image", "");
        //    string contentType = "";
        //    var filePath = new FileInfo(Server.MapPath("~/images") + filename);

        //    var index = filename.LastIndexOf(".") + 1;
        //    var extension = filename.Substring(index).ToUpperInvariant();

        //    // Fix for IE not handling jpg image types
        //    contentType = string.Compare(extension, "JPG") == 0 ? "image/jpeg" : string.Format("image/{0}", extension);

        //    return File(filePath.FullName, contentType);
        //}

        [HttpPost]
        public ContentResult UploadFiles()
        {
            try
            {
                var r = new List<UploadFilesResult>();

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;

                    if (hpf.ContentLength == 0)
                        continue;
                    string fileName = DateTime.Now.Ticks.ToString() + "_" + Path.GetFileName(hpf.FileName);

                    string savedFileName = Path.Combine(Server.MapPath("~/images"), fileName);
                    string savedFileNameThumbnail = Path.Combine(Server.MapPath("~/images/thumbnails"),fileName);
                    hpf.SaveAs(savedFileName);

                    //Read image back from file and create thumbnail from it
                    var imageFile = savedFileName;
                    using (var srcImage = Image.FromFile(imageFile))
                    using (var newImage = new Bitmap(70, 70))
                    using (var graphics = Graphics.FromImage(newImage))
                    using (var stream = new MemoryStream())
                    {
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.DrawImage(srcImage, new Rectangle(0, 0, 70, 70));
                        //newImage.Save(stream, ImageFormat.Png);
                        //var thumbNew = File(stream.ToArray(), "image/png");

                        newImage.Save(savedFileNameThumbnail);

                    }


                    r.Add(new UploadFilesResult()
                    {
                        Name = fileName,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType
                    });
                }
                return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");

            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content(ex.Message, MediaTypeNames.Text.Plain);
            }


        }
        [HttpPost]
        public JsonResult DeleteFile(string fileName)
        {
            string error = "";
            try
            {
                string savedFileName = Path.Combine(Server.MapPath("~/images"), fileName);
                string savedFileNameThumbnail = Path.Combine(Server.MapPath("~/images/thumbnails"), fileName);
                if (System.IO.File.Exists(savedFileName))
                {
                    System.IO.File.Delete(savedFileName);
                }

                if (System.IO.File.Exists(savedFileNameThumbnail))
                {
                    System.IO.File.Delete(savedFileNameThumbnail);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return Json(new { Error = error });

        }

	}
}