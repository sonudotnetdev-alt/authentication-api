using Authentication.Core.Dto;
using Authentication.Core.Entities;
using Authentication.Core.Interface;
using Authentication_Api.Cache;
using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;

namespace Authentication_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [EnableRateLimiting("fixed")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheProvider _cacheProvider;
        public CategoryController(IUnitOfWork Uow, ICacheProvider cacheProvider)
        {
            _uow = Uow;
            _cacheProvider= cacheProvider;
            //_mapper = mapper;
        }

        //here caching implemented-In-Memory Caching
        //In-memory caching refers to storing cached data
        //in the memory of the web server where the application is running.

        [HttpGet("get-all-category")]
        [ApiVersion("1.0")]
        //[Produces("application/xml")]
        public async Task<ActionResult> Get()
        {
            if (!_cacheProvider.TryGetValue(CacheKey.Category, out IReadOnlyList<Category> category))
            {
                category = await _uow.CategoryRepository.GetAllAsync();
                if (category != null)
                {
                    var cacheEntryOption = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                        SlidingExpiration = TimeSpan.FromSeconds(30),
                        Size = 1024
                    };
                    _cacheProvider.Set(CacheKey.Category, category, cacheEntryOption);
                }
            }
            return Ok(category);
        }


        //[HttpGet("get-all-category")]
        //[ApiVersion("2.0")]
        //public async Task<ActionResult> GetV2()
        //{
        //    var category = await _uow.CategoryRepository.GetAllAsync();
        //    if (category == null)
        //    {
        //        return BadRequest(StatusCodes.Status404NotFound);
        //    }
        //    return Ok(category);
        //}


        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var category = await _uow.CategoryRepository.GetAsync(id);
            if (category == null)
                return BadRequest($"Not found this id = [{id}]");
               return Ok(category);
            //return Ok(_mapper.Map<Category, ListCategoryDto>(category));
        }

        [HttpPost("add-new-category")]
        public async Task<ActionResult> post(CategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var res = _mapper.Map<Category>(categoryDto);
                    var new_category = new Category
                    {
                        Name = categoryDto.Name,
                        Description = categoryDto.Description
                    };
                    await _uow.CategoryRepository.AddAsync(new_category);
                    return Ok(new_category);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut("update-exiting-category-by-id/{id}")]
        public async Task<ActionResult> put(int id, CategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exiting_category = await _uow.CategoryRepository.GetAsync(id);
                    if (exiting_category != null)
                    {
                        exiting_category.Name = categoryDto.Name;
                        exiting_category.Description = categoryDto.Description;
                        //_mapper.Map(categoryDto, exiting_category);
                    }
                    await _uow.CategoryRepository.UpdateAsync(id, exiting_category);
                    return Ok(exiting_category);
                }
                return BadRequest($"Category id [{id}] Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }       

        [HttpDelete("delete-category-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var exiting_category = await _uow.CategoryRepository.GetAsync(id);
                if (exiting_category != null)
                {
                    await _uow.CategoryRepository.DeleteAsync(id);
                    return Ok($"This Category [{exiting_category.Name}] Is deleted ");
                }
                return BadRequest($"This Category [{exiting_category.Id}] Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
