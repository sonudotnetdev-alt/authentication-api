using Authentication.Core.Dto;
using Authentication.Core.Entities;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Authentication_Api.Models
{
    public class MappingCategory : Profile
    {
        public MappingCategory()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ListCategoryDto, Category>().ReverseMap();
        }
    }
}
