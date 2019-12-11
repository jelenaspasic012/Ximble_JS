using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Ximble_JS.Helpers;
using Ximble_JS.Models;

namespace Ximble_JS.Controllers
{
    public class ProductionController : ApiController
    {
        #region Constructor
        IProductionRepository _repository;

        public ProductionController()
        {
        }

        public ProductionController(IProductionRepository repository)
        {
            _repository = repository;
        }

        public List<ProductionEntities> ProductionEntities = new List<ProductionEntities>();

        public ProductionController(List<ProductionEntities> productionEntities)
        {
            this.ProductionEntities = productionEntities;
        }
        #endregion

        #region Get
        /// <summary>
        /// Return products based on parameters
        /// Task a)
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="name"></param>
        /// <param name="selDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/production/{name?}/{selDate?}/{keywords?}")]
        public IHttpActionResult Get([FromUri]PagingModel pagingModel, string name = null, string selDate = null, string keywords = null)
        {
            using (ProductionEntities entities = new ProductionEntities())
            {
                List<int> productDescIds = new List<int>();
                List<int> productModelIds = new List<int>();

                if (keywords != null)
                {
                    List<string> keywordList = keywords.Split(',').ToList();

                    //get product modelids for each keyword 
                    foreach (var key in keywordList)
                    {
                        IEnumerable<ProductModelProductDescriptionCulture> desc = entities.ProductModelProductDescriptionCultures.
                            Where(x => x.ProductDescription.Description.Contains(key));

                        productDescIds.AddRange(desc.Select(x => x.ProductDescriptionID));
                        productModelIds.AddRange(entities.ProductModelProductDescriptionCultures
                                        .Where(x => productDescIds.Contains(x.ProductDescriptionID))
                                        .Select(y => y.ProductModelID).ToList());
                    }
                }

                DateTime? date = null;
                if (selDate != null)
                    date = DateTime.Parse(selDate);

                List <Product> products = entities.Products.Where(x => (x.Name == name || name == null)
                                              && (x.SellStartDate == date || selDate == null)
                                              && (productModelIds.Any(g=> g == x.ProductModelID) || keywords == null)).ToList();         
                if (products.Count == 0)
                    return NotFound();
                else
                {
                    //Calculate paging
                    if (pagingModel == null)
                        pagingModel = new PagingModel();

                    int totalResults = products.Count();
                    int currentPage = pagingModel.PageNumber;
                    int pageSize = pagingModel.PageSize;
                    int pagesNumber = (int)Math.Ceiling((double)totalResults / (double)pageSize);

                    List<Product> resultsPart = products.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                    return Ok(resultsPart);
                }
            }
        }
        #endregion
    }
}
