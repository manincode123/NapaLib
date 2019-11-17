using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class GroupCVLop
    {
        public Group Group { get; set; }
        public int GroupId { get; set; }
        public ChucVu ChucVu { get; set; }
        public int ChucVuId { get; set; }
    }
}