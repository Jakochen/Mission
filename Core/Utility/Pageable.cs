using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public class Pageable
    {
        /// <summary>
        /// 第幾頁，從 1 開始 
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// 一頁幾筆資料
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 總頁數，唯讀
        /// </summary>
        public int TotalPages { get; private set; }
        /// <summary>
        /// 是否已是最後一頁，唯讀
        /// </summary>
        public bool EndOfPage { get; private set; }

        private int totalCount;
        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                if (totalCount == value)
                {
                    return;
                }
                totalCount = value;

                TotalPages = 0;
                EndOfPage = true;
                if (totalCount <= 0) {
                    return;
                }
                TotalPages = (TotalCount / PageCount) + ((TotalCount % PageCount) > 0 ? 1 : 0);
                EndOfPage = PageNo >= TotalPages;

            }
        }
        
        /// <summary>
        /// 下一頁
        /// </summary>
        public Pageable NextPage
        {
            get
            {
                if (PageNo <= 0 || PageCount <= 0 || TotalCount == 0 || PageNo >= TotalPages)
                {
                    return null;
                }
                return new Pageable(PageNo + 1, PageCount);
            }
        }

        /// <summary>
        /// 上一頁
        /// </summary>
        public Pageable PreviousPage
        {
            get
            {
                if (PageNo <= 0 || PageCount <= 0 || TotalCount == 0 || PageNo <= 1)
                {
                    return null;
                }
                return new Pageable(PageNo - 1, PageCount);
            }
        }

        public Pageable()
        {
            EndOfPage = true;
        }

        public Pageable(int pageNo, int pageCount)
        {
            PageNo = pageNo;
            PageCount = pageCount;
            EndOfPage = true;
        }
    }
}
