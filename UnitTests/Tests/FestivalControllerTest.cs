using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FestiFact3.Controllers;
using Model.Abstract;
using Model.Concrete;
using FestiFact3.App_Start;

namespace UnitTests.Tests
{
    [TestClass]
    public class FestivalControllerTest
    {

        [TestMethod]
        public void ListFestivalTest()
        {
            var mockFestival = new Moq.Mock<IFestivalRepository>();
            var controller = new FestivalController(mockFestival.Object);
            var result = controller.Index(null);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }
    }
}