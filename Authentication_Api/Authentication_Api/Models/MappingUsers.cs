using Authentication.Core.Dto;
using Authentication.Core.Entities;
using AutoMapper;

namespace Authentication_Api.Models
{
    public class MappingUsers:Profile
    {
        public MappingUsers() {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
