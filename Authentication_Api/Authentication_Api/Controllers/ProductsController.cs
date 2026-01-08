using Authentication.Core.Dto;
using Authentication.Core.Entities;
using Authentication.Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using System.Text.Json;

namespace Authentication_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _cache;
        public ProductsController(IUnitOfWork unitOfWork,IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _cache = distributedCache;
            //int a = 5;int b = 10;
            //a= a + b; //a =15;
            //b=a-b; // b= 5;
            //a = a - b;//b=10

        }
        //[HttpGet("get-all-product")]
        //public async Task<IActionResult> get()
        //{
        //    var cacheKey = "GET_ALL_PRODUCTS";
        //    List<ProductDto> products;
        //    try
        //    {
        //        // Get data from cache
        //        var cachedData = await _cache.GetStringAsync(cacheKey);
        //        if (cachedData != null)
        //        {
        //            // Deserialize cached data
        //            products = JsonSerializer.Deserialize<List<ProductDto>>(cachedData) ?? new List<ProductDto>();
        //        }
        //        else
        //        {
        //            //fetch data from database
        //            var productsData = await _unitOfWork.ProductRepository.GetAllAsync();
        //            if (productsData != null)
        //            {
        //                products = productsData.Select(p => new ProductDto
        //                {
        //                    Id = p.Id,
        //                    Name = p.Name,
        //                    Description = p.Description,
        //                    Price = p.Price
        //                }).ToList();
        //                // Serialize and save data to cache
        //                var serializedData = JsonSerializer.Serialize(products);
        //                var cacheOptions = new DistributedCacheEntryOptions()
        //                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        //                await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);
        //            }
        //        }
        //            return Ok(productsData);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "An error occurred while retrieving products.", details = ex.Message });
        //    }
        //    finally 
        //    { 
        //    }
        //    //var productCollection = await _unitOfWork.ProductRepository.GetAllAsync();
        //    //if (productCollection != null)
        //    //{
        //    //    return Ok(productCollection);
        //    //}
        //    //return BadRequest("Not Found");
        //}

        //Previously added code without caching

        [HttpGet("get-all-product")]
        public async Task<IActionResult> get()
        {
            var productCollection = await _unitOfWork.ProductRepository.GetAllAsync();
            if (productCollection != null)
            {
                return Ok(productCollection);
            }
            return BadRequest("Not Found");
        }

    }
}
