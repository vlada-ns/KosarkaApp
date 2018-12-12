using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KosarkaApp.Interfaces;
using Moq;
using KosarkaApp.Models;
using KosarkaApp.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Linq;

namespace KosarkaApp.Tests.Controllers
{
    [TestClass]
    public class KosarkasiControllerTest
    {
        [TestMethod]
        public void GetReturns200AndObject()
        {
            // Arrange
            var mockRepository = new Mock<IKosarkasRepository>();
            mockRepository.Setup(x => x.GetById(50)).Returns(new Kosarkas { Id = 50, Naziv = "Test50", Godina = 1980, BrojUtakmica = 50, ProsecanBrojPoena = 10.8m, KlubId = 1 });

            var controller = new KosarkasiController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(50);
            var contentResult = actionResult as OkNegotiatedContentResult<Kosarkas>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(50, contentResult.Content.Id);
        }

        [TestMethod]
        public void GetReturns404()
        {
            // Arrange
            var mockRepository = new Mock<IKosarkasRepository>();
            var controller = new KosarkasiController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(51);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutReturns400()
        {
            //Arrange
            var mockRepository = new Mock<IKosarkasRepository>();
            var controller = new KosarkasiController(mockRepository.Object);

            //Act
            IHttpActionResult actionResult = controller.Put(54, new Kosarkas { Id = 100, Naziv = "Test100", Godina = 1980, BrojUtakmica = 50, ProsecanBrojPoena = 10.8m, KlubId = 1 });

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostReturnsMultipleObjects()
        {
            // Arrange
            List<Kosarkas> kosarkasi = new List<Kosarkas>();
            kosarkasi.Add(new Kosarkas { Id = 56, Naziv = "Test56", Godina = 1980, BrojUtakmica = 50, ProsecanBrojPoena = 10.8m, KlubId = 1 });
            kosarkasi.Add(new Kosarkas { Id = 57, Naziv = "Test57", Godina = 1980, BrojUtakmica = 50, ProsecanBrojPoena = 10.8m, KlubId = 1 });

            Pretraga pretraga = new Pretraga() { Najmanje = 45, Najvise = 55 };

            var mockRepository = new Mock<IKosarkasRepository>();
            mockRepository.Setup(x => x.GetPretraga(45, 55)).Returns(kosarkasi.AsEnumerable());
            var controller = new KosarkasiController(mockRepository.Object);

            // Act
            //IEnumerable<Festival> result = controller.Get();
            dynamic response = controller.PostPretraga(pretraga);
            var result = (IEnumerable<dynamic>)response.Content;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(kosarkasi.Count, result.ToList().Count);
            Assert.AreEqual(kosarkasi.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(kosarkasi.ElementAt(1), result.ElementAt(1));

            // DODATAK
            Assert.AreEqual(response.GetType().GetGenericTypeDefinition(), typeof(OkNegotiatedContentResult<>));
        }
    }
}
