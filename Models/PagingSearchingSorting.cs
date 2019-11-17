using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class PagingSearchingSorting
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int StartRecord { get; set; } //Index record để bắt đầu để query (dùng cho Skip(StartRecord))

        public string SearchTerm { get; set; }

        public string OrderColumn { get; set; }

        public string OrderType { get; set; }

        public int Draw { get; set; }

        public PagingSearchingSorting()
        {
            PageSize = 20;
            PageIndex = 1;
            StartRecord = 0;
            SearchTerm = "";
            OrderColumn = "";
            OrderType = "ASC";
        }

    }
}