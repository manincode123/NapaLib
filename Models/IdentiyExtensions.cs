using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace NAPASTUDENT.Models
{
    public static class IdentityExtensions
    {
        public static int GetSinhVienId(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            return ci != null ? Int32.Parse( ci.FindFirstValue("SinhVienId")) : 0;
        }
        public static string GetTenNguoiDung(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                return ci.FindFirstValue("TenNguoiDung");
            }
            return null;
        }
    }

}