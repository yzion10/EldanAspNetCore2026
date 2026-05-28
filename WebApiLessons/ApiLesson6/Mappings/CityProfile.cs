using ApiLesson6.DTO;
using ApiLesson6.Entities;
using AutoMapper;

namespace ApiLesson6.Mappings
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
