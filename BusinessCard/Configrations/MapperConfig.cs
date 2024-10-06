using AutoMapper;
using BusinessCard.core.Data;
using BusinessCard.core.DTO.BusinessCards;

namespace BusinessCard.Api.Configrations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {

            CreateMap<Businesscards,CreateBusinessCardDto>().ReverseMap();
            CreateMap<Businesscards, GetBusinessCardDto>().ReverseMap();
            CreateMap<Businesscards, UpdateBusinessCardDto>().ReverseMap();



        }


    }
}
