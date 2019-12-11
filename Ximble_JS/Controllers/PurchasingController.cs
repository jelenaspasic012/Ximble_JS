using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ximble_JS.Helpers;
using Ximble_JS.Models;

namespace Ximble_JS.Controllers
{
    public class PurchasingController : ApiController
    {
        #region Constructor
        IPurchasingRepository _repository;

        public PurchasingController()
        {
        }

        public PurchasingController(IPurchasingRepository repository)
        {
            _repository = repository;
        }

        public List<ProductionEntities> ProductionEntities = new List<ProductionEntities>();

        public PurchasingController(List<ProductionEntities> productionEntities)
        {
            this.ProductionEntities = productionEntities;
        }
        #endregion

        #region Get 
        /// <summary>
        /// Calculate results grouped by days
        /// Task b)
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/purchasing/{startDate}/{endDate}")]
        public IHttpActionResult Get([FromUri]PagingModel pagingModel, string startDate, string endDate)
        {
            using (PurchasingEntities entities = new PurchasingEntities())
            {
                List<PurchaseOrderDetail> purchaseList = entities.PurchaseOrderDetails.ToList();

                DateTime? start = null;
                if (startDate != null)
                    start = DateTime.Parse(startDate);
                DateTime? end = null;
                if (endDate != null)
                    end = DateTime.Parse(endDate);

                var results = from purchase in purchaseList
                              where purchase.DueDate >= start && purchase.DueDate <= end
                              group purchase by purchase.DueDate into data
                              select new Result
                              {
                                  SumOfTraffic = data.Sum(x => x.LineTotal),
                                  OrderQty = data.Sum(x => x.OrderQty),
                                  SumAll = data.Sum(x => x.LineTotal + x.OrderQty)
                              };

                if (results.Count() == 0)
                    return NotFound();
                else
                {
                    //Calculate paging
                    int totalResults = results.Count();
                    int currentPage = pagingModel.PageNumber;
                    int pageSize = pagingModel.PageSize;
                    int pagesNumber = (int)Math.Ceiling((double)totalResults / (double)pageSize);

                    var resultsPart = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                    return Ok(resultsPart);
                }
            }
        }
        #endregion

        #region Additional classes
        public class Result
        {
            public Result() { }

            public Result(decimal sumOfTraffic, int orderQty, decimal sumAll)
            {
                this.SumOfTraffic = sumOfTraffic;
                this.OrderQty = orderQty;
                this.SumAll = sumAll;
            }

            public decimal SumOfTraffic { get; set; }
            public int OrderQty { get; set; }
            public decimal SumAll { get; set; }
        }
        #endregion
    }
}
