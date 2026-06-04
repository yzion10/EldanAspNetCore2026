using ApiLesson7.DTO;
using ApiLesson7.Entities;
using AutoMapper;

namespace ApiLesson7.Mappings
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDTO>();
            CreateMap<City, CityWithoutLandMarkDTO>();
            CreateMap<LandMark, LandMarkDto>();
            CreateMap<LandMarkForCreateDTO, LandMark>();
            CreateMap<LandMarkForUpdateDTO, LandMark>();
        }
    }
}
