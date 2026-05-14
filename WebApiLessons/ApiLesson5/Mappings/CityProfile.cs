using ApiLesson5.DTO;
using ApiLesson5.Entities;
using AutoMapper;

namespace ApiLesson5.Mappings
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDTO>();
            CreateMap<City, CityWithoutLandMarkDTO>();
        }
    }
}
