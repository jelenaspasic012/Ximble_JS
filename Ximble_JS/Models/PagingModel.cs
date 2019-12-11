using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ximble_JS.Models
{
    public class PagingModel
    {
        #region Fields
        public const int maxPageSize = 20;
        public int pageSize = 5;
        #endregion

        #region Properties
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        #endregion

        #region Constructor
        public PagingModel()
        {
        }

        public PagingModel(int pageSize)
        {
            this.pageSize = pageSize;
        }

        #endregion
    }
}