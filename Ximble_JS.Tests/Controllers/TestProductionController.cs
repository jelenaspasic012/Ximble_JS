using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ximble_JS.Controllers;
using Ximble_JS.Helpers;
using Ximble_JS.Models;
using Moq;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;

namespace Ximble_JS.Tests.Controllers
{
    [TestClass]
    public class TestProductionController
    {
        private IProductionRepository repository;
        private ProductionController controller;
        PagingModel pagingModel;

        public TestProductionController()
        {
            controller = new ProductionController(repository);
            pagingModel = new PagingModel();
        }

        [TestMethod]
        public void GetProductsTest()
        {
            //test NotFound data 
            string productName = "Production test";
            IHttpActionResult actionResult = controller.Get(pagingModel, productName);
            var notFoundRes = actionResult as NotFoundResult;
            Assert.IsNotNull(notFoundRes);

            actionResult = controller.Get(pagingModel);
            var allData = actionResult as OkNegotiatedContentResult<List<Product>>;
            Assert.IsNotNull(allData);
            Assert.IsTrue(allData.Content.Any());
        }

        [TestMethod]
        public void GetProductsByName()
        {
            string name = "Blade";
            pagingModel = new PagingModel(1);

            IHttpActionResult actionResult = controller.Get(pagingModel, name);
            var result = actionResult as OkNegotiatedContentResult<List<Product>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Content.FirstOrDefault().Name);
        }

        [TestMethod]
        public void GetProductsByNameAndSelStartDate()
        {
            string name = "Crown Race";
            string selStartDate = "2008-04-30";
            pagingModel = new PagingModel(1);

            IHttpActionResult actionResult = controller.Get(pagingModel, name, selStartDate);
            var result = actionResult as OkNegotiatedContentResult<List<Product>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Content.FirstOrDefault().Name);
            Assert.AreEqual(selStartDate, result.Content.FirstOrDefault().SellStartDate.ToString("yyyy-MM-dd"));
        }
    }
}
