using AutoMapper;
using CarCatalogApi.DTOs;
using CarCatalogApi.Entities;

namespace CarCatalogApi.Mappings;

public class CarCatalogProfile : Profile
{
    public CarCatalogProfile()
    {
        CreateMap<Manufacturer, ManufacturerDto>();
        CreateMap<Manufacturer, ManufacturerWithoutModelsDto>();
        CreateMap<ManufacturerForCreateDto, Manufacturer>();
        CreateMap<ManufacturerForUpdateDto, Manufacturer>();

        // מיפוי של CarModel ל- CarModelDto כולל מיפוי של שם היצרן
        CreateMap<CarModel, CarModelDto>();
            //.ForMember(destination => destination.ManufacturerName,
              //  options => options.MapFrom(source => source.Manufacturer.Name));

        CreateMap<CarModel, CarModelWithoutSubModelsDto>();
            //.ForMember(destination => destination.ManufacturerName,
              //  options => options.MapFrom(source => source.Manufacturer.Name));

        CreateMap<CarModelForCreateDto, CarModel>();
        CreateMap<CarModelForUpdateDto, CarModel>();

        CreateMap<CarSubModel, CarSubModelDto>();
            //.ForMember(destination => destination.CarModelName,
              //  options => options.MapFrom(source => source.CarModel.Name));

        CreateMap<CarSubModelForCreateDto, CarSubModel>();
        CreateMap<CarSubModelForUpdateDto, CarSubModel>();
    }
}
