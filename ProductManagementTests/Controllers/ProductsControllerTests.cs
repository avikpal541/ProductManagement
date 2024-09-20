using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManagement.Models;
using Xunit.Sdk;

namespace ProductManagement.Controllers.Tests
{
    [TestClass()]
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        //private readonly ILogger<ProductsController> _logger;
        private readonly Mock<ILogger<ProductsController>> _mocklogger;

        public ProductsControllerTests()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mocklogger = new Mock<ILogger<ProductsController>>();
        }

        /// <summary>
        /// Given : Get Products called
        /// When : no filter passed
        /// Then : Returen Success Response(200)  
        /// </summary>
        /// <returns> All Product List</returns>
        [TestMethod()]
        public async Task ProductsController_GetProducts_ReturnAll_WithOutFilter()
        {
            //Arrange
            _mockHttpClientFactory.Setup(_=>_.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
            
            RequestModel requestFilter = new RequestModel();
            ProductsController controller = new ProductsController( _mocklogger.Object,_mockHttpClientFactory.Object);
            
            // Act
            var result = await controller.GetProducts(requestFilter);
            var products = ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value as IEnumerable<Product>;
            
            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result).StatusCode);
            Assert.AreEqual(48, products.Count());
        }


        /// <summary>
        /// Given : Get Products called
        /// When : minprice filter passed
        /// Then : Returen Success Response(200)  
        /// </summary>
        /// <returns> return list based on filter condition</returns>
        [TestMethod()]
        public async Task ProductsController_GetProducts_ReturnFiltered_With_minprice_Filter()
        {
            //Arrange
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            RequestModel requestFilter = new RequestModel { minprice=20};
            ProductsController controller = new ProductsController(_mocklogger.Object, _mockHttpClientFactory.Object);

            // Act
            var result = await controller.GetProducts(requestFilter);
            var products = ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value as IEnumerable<Product>;

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result).StatusCode);
            Assert.AreEqual(18, products.Count());
        }


        /// <summary>
        /// Given : Get Products called
        /// When : maxprice filter passed
        /// Then : Returen Success Response(200)  
        /// </summary>
        /// <returns> return list based on filter condition</returns>
        [TestMethod()]
        public async Task ProductsController_GetProducts_RertunFiltered_With_maxprice_Filter()
        {
            //Arrange
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            RequestModel requestFilter = new RequestModel { maxprice = 20 };
            ProductsController controller = new ProductsController(_mocklogger.Object, _mockHttpClientFactory.Object);

            // Act
            var result = await controller.GetProducts(requestFilter);
            var products = ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value as IEnumerable<Product>;

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result).StatusCode);
            Assert.AreEqual(33, products.Count());
        }


        /// <summary>
        /// Given : Get Products called
        /// When : minprice and maxprice filter passed
        /// Then : Returen Success Response(200)  
        /// </summary>
        /// <returns> return list based on filter condition</returns>
        [TestMethod()]
        public async Task ProductsController_GetProducts_ReturnFiltered_With_minpriceAndmaxprice_Filter()
        {
            //Arrange
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            RequestModel requestFilter = new RequestModel { minprice = 18,maxprice=20 };
            ProductsController controller = new ProductsController(_mocklogger.Object, _mockHttpClientFactory.Object);

            // Act
            var result = await controller.GetProducts(requestFilter);
            var products = ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value as IEnumerable<Product>;

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result).StatusCode);
            Assert.AreEqual(9, products.Count());
        }

        /// <summary>
        /// Given : Get Products called
        /// When : minprice,maxprice and highlight in Green and Blue filter passed
        /// Then : Returen Success Response(200)  
        /// </summary>
        /// <returns> return list based on filter condition also Blue and Green in Html tag</returns>
        [TestMethod()]
        public async Task ProductsController_GetProducts_ReturnFiltered_With_minprice_maxpriceAndhighlightInGreen_Filter()
        {
            //Arrange
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            RequestModel requestFilter = new RequestModel { minprice = 18, maxprice = 20 ,highlight="Blue,Green"};
            ProductsController controller = new ProductsController(_mocklogger.Object, _mockHttpClientFactory.Object);

            // Act
            var result = await controller.GetProducts(requestFilter);
            var products = ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value as IEnumerable<Product>;

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result).StatusCode);
            var productWithGreenInDescription=products.FirstOrDefault(x => x.description.Contains("green"));
            Assert.IsTrue(productWithGreenInDescription.description.Contains("<em>green</em>"));
            var productWithBlueInDescription = products.FirstOrDefault(x => x.description.Contains("blue"));
            Assert.IsTrue(productWithBlueInDescription.description.Contains("<em>blue</em>"));
            Assert.AreEqual(9, products.Count());
        }


        /// <summary>
        /// Given : Get Products called
        /// When : minprice,maxprice and size filter passed
        /// Then : Returen Success Response(200)  
        /// </summary>
        /// <returns> return list based on filter condition</returns>
        [TestMethod()]
        public async Task ProductsController_GetProducts_ReturnFiltered_With_minprice_maxpriceAndSize_Filter()
        {
            //Arrange
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            RequestModel requestFilter = new RequestModel { minprice = 18, maxprice = 20, size="small" };
            ProductsController controller = new ProductsController(_mocklogger.Object, _mockHttpClientFactory.Object);

            // Act
            var result = await controller.GetProducts(requestFilter);
            var products = ((Microsoft.AspNetCore.Mvc.ObjectResult)result).Value as IEnumerable<Product>;

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((Microsoft.AspNetCore.Mvc.ObjectResult)result).StatusCode);
            Assert.AreEqual(6, products.Count());
        }

        
    }
}