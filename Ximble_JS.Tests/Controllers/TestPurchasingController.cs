using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Ximble_JS.Controllers;
using Ximble_JS.Helpers;
using Ximble_JS.Models;
using static Ximble_JS.Controllers.PurchasingController;

namespace Ximble_JS.Tests.Controllers
{
    [TestClass]
    public class TestPurchasingController
    {
        private IPurchasingRepository repository;
        private PurchasingController controller;
        PagingModel pagingModel;

        public TestPurchasingController()
        {
            controller = new PurchasingController(repository);
            pagingModel = new PagingModel();
        }

        [TestMethod]
        public void GetPurchasingTest()
        {
            string startDate1 = "9999-12-31";
            string endDate = "9999-12-31";

            //test NotFound data 
            IHttpActionResult actionResult = controller.Get(pagingModel, startDate1, endDate);
            var notFoundRes = actionResult as NotFoundResult;
            Assert.IsNotNull(notFoundRes);          
        }

        [TestMethod]
        public void GetPurchasingTest2()
        {
            string startDate = "2011-04-30";
            string endDate = "2011-04-30";

            Result expectedResult = new Result(9491.5180M, 563, 10054.5180M);
            
            IHttpActionResult actionResult = controller.Get(pagingModel, startDate, endDate);
            var result = actionResult as OkNegotiatedContentResult<List<Result>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.OrderQty, result.Content.FirstOrDefault().OrderQty);
            Assert.AreEqual(expectedResult.SumAll, result.Content.FirstOrDefault().SumAll);     
            Assert.AreEqual(expectedResult.SumOfTraffic, result.Content.FirstOrDefault().SumOfTraffic);

        }
    }
}
