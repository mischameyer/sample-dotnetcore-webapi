using AutoMapper;
using Backend.Dtos;
using Backend.Entities;

namespace backend.Mapper
{
    public class ProfileValueMapper: Profile
    {
        public ProfileValueMapper() {
            
            CreateMap<Value, ValueDto>().ReverseMap();
            CreateMap<Value, ValueUpdateDto>().ReverseMap();
            CreateMap<Value, ValueCreateDto>().ReverseMap();
        }
    }
}
