using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using ProductManagement.Models;
using System.Text.Json;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(ILogger<ProductsController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _httpClientFactory = clientFactory;
        }


        /// <summary>
        /// Business logic to filter and update description of product with html tag
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="minprice"></param>
        /// <param name="maxprice"></param>
        /// <param name="size"></param>
        /// <param name="highlight"></param>
        /// <returns></returns>
        private List<Product> FilterProducts(ProductList productList, int? minprice, int? maxprice, string? size, string? highlight)
        {
            List<Product> filteredProducts = productList.products;
            if (productList is not null)
            {
                try
                {
                    if (minprice is not null)
                        filteredProducts = productList.products.FindAll(x => x.price >= minprice);
                    if (maxprice is not null)
                        filteredProducts = filteredProducts.FindAll(x => x.price <= maxprice);
                    if (size is not null)
                        filteredProducts = filteredProducts.FindAll(x => x.sizes.Contains(size));
                    if (highlight is not null)
                    {
                        var highlightColors= highlight.Split(',');
                        foreach (var product in filteredProducts)
                        {
                            foreach (var color in highlightColors)
                            {
                                if (product.description.ToLower().Contains(color.ToLower()))
                                {
                                    product.description = product.description.Replace(color.ToLower(),$"<em>{color.ToLower()}</em>");
                                }
                            }
                        }
                    }
                }
                catch(Exception ex) {
                    _logger.LogDebug(ex.Message); 
                    _logger.LogDebug(ex.StackTrace);
                }

            }
            return (filteredProducts);
        }

        /// <summary>
        /// API to filter product list based on miprice , maxprice , size etc
        /// </summary>
        /// <param name="filter : which consists of 4 optional query parameters minprice , maxprice , 
        /// size and highlight"></param>
        /// <returns>List of Products as per the filter conditions</returns>
        /// 
        [HttpGet]
        [Route("/filterproducts")]
        [OutputCache(Duration = CacheConfig.Duration, NoStore = true)]
        public async Task<IActionResult> GetProducts([FromQuery] RequestModel filter)
        {
            _logger.LogInformation($"Filter Products called with parameters minprice:{filter.minprice} , maxprice: {filter.maxprice},"
                                    + $"size: {filter.size}, highlight: {filter.highlight}");
            var request = new HttpRequestMessage(HttpMethod.Get, Constants.apiUrl);
            var httpClient = _httpClientFactory.CreateClient();
            List<Product> products = new List<Product>();
            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);


                if (response is not null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiStringResult = await response.Content.ReadAsStringAsync();
                    var apiResultList = JsonSerializer.Deserialize<ProductList>(apiStringResult);
                    if (apiResultList is null)
                    {
                        return NoContent();
                    }
                    products = FilterProducts(apiResultList, filter.minprice, filter.maxprice, filter.size, filter.highlight);

                }
                else
                {
                    return NotFound(Constants.noDataOrReachable);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                _logger.LogDebug(ex.StackTrace);
                return NotFound(Constants.noData);
            }

            _logger.LogInformation($"Returned Success result for: {filter.minprice} , maxprice: {filter.maxprice},"
                                    + $"size: {filter.size}, highlight: {filter.highlight}");
            return Ok(products);
        }
    }
}
