using System.Net;

namespace ProductManagementTests
{
    [TestClass()]
    public class ProductManagementIntegrationTest
    {
        /// <summary>
        /// Given : Get Products called
        /// When : no filter passed
        /// Then : Returen Success Response (200)        
        /// <returns>Success Response</returns>
        [TestMethod()]
        public async Task ProductsController_GetProducts_Positive()
        {
            //Arrange
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5079/");

            HttpResponseMessage response = await client.GetAsync("filterproducts");
            var products = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        /// <summary>
        /// Given : Get Products called
        /// When : size is out of listed sizes
        /// Then : Returen Bad Request (400)
        /// </summary>
        /// <returns> Bad Request (400)</returns>
        [TestMethod()]
        public async Task GetProducts_ReturnValidationError_BadRequest_When_sizeIsOutOfList()
        {
            //Arrange
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5079/");

            HttpResponseMessage response = await client.GetAsync("filterproducts?size=x-large");
            var products = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }
    }
}
