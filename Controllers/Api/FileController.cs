using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NAPASTUDENT.Controllers.AntiHack;

namespace NAPASTUDENT.Controllers.Api
{
    public class FileController : ApiController
    {

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/file/UploadAnhBiaSv")]
        public async Task<IHttpActionResult> UploadAnhBiaSv()
        {
            // nhớ là tên biến "image" phải đặt tương tự như tên đặt cho file gán vào formData khi gửi ajax
            // Ví dụ: var dataAnhBia = FormData();
            // dataAnhBia.append("fileM",file_muốn_gửi, fileName)
            // Nếu tên là "fileM" thì phải đặt tên biến ở đây là fileM thì ASP.NET mới nhận
            //var image = HttpContext.Current.Request.Files.Count > 0 ?
            //    HttpContext.Current.Request.Files[0] : null;
            const string StoragePath = @"Content\AnhBia\AnhSV\";
            
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            var image = HttpContext.Current.Request.Files["image"];
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                string fileName = null;
                string filePath = null;
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    fileName = file.Headers.ContentDisposition.FileName.Trim('\"') + ".png";
                    filePath = Path.Combine(HttpRuntime.AppDomainAppPath + StoragePath, fileName);
                    File.Copy(file.LocalFileName, filePath,true);
                }

                return Ok(@"\" + StoragePath + fileName);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return BadRequest();
        }   
        
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/file/UploadAnhBiaDonVi")]
        public async Task<IHttpActionResult> UploadAnhBiaDonVi()
        {
            // nhớ là tên biến "image" phải đặt tương tự như tên đặt cho file gán vào formData khi gửi ajax
            // Ví dụ: var dataAnhBia = FormData();
            // dataAnhBia.append("fileM",file_muốn_gửi, fileName)
            // Nếu tên là "fileM" thì phải đặt tên biến ở đây là fileM thì ASP.NET mới nhận
            //var image = HttpContext.Current.Request.Files.Count > 0 ?
            //    HttpContext.Current.Request.Files[0] : null;
            const string StoragePath = @"Content\AnhBia\AnhBiaDonVi\";
            
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            var image = HttpContext.Current.Request.Files["image"];
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                string fileName = null;
                string filePath = null;
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    fileName = file.Headers.ContentDisposition.FileName.Trim('\"') + ".png";
                    filePath = Path.Combine(HttpRuntime.AppDomainAppPath + StoragePath, fileName);
                    File.Copy(file.LocalFileName, filePath,true);
                }

                return Ok(@"\" + StoragePath + fileName);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return BadRequest();
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/file/UploadAnhBiaLop")]
        public async Task<IHttpActionResult> UploadAnhBiaLop()
        {
            // nhớ là tên biến "image" phải đặt tương tự như tên đặt cho file gán vào formData khi gửi ajax
            // Ví dụ: var dataAnhBia = FormData();
            // dataAnhBia.append("fileM",file_muốn_gửi, fileName)
            // Nếu tên là "fileM" thì phải đặt tên biến ở đây là fileM thì ASP.NET mới nhận
            //var image = HttpContext.Current.Request.Files.Count > 0 ?
            //    HttpContext.Current.Request.Files[0] : null;
            const string StoragePath = @"Content\AnhBia\AnhBiaLop\";
            
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            //var image = HttpContext.Current.Request.Files["image"];
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                string fileName = null;
                string filePath = null;
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    fileName = file.Headers.ContentDisposition.FileName.Trim('\"') + ".png";
                    filePath = Path.Combine(HttpRuntime.AppDomainAppPath + StoragePath, fileName);
                    File.Copy(file.LocalFileName, filePath,true);
                }

                return Ok(@"\" + StoragePath + fileName);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return BadRequest();
        }


    }
}
