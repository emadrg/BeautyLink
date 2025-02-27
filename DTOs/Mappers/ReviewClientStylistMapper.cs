using AutoMapper;
using DA.Entities;
using DTOs.City;
using DTOs.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class ReviewClientStylistMapper : BaseMapper
    {
        public ReviewClientStylistMapper(IMapper mapper) : base(mapper)
        {
           
        }

        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<ReviewClientStylist, DisplayClientStylistReviewDTO>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"))
                .ForMember(dest => dest.StylistName, opt => opt.MapFrom(src => $"{src.Stylist.User.FirstName} {src.Stylist.User.LastName}"));

            config.CreateMap<CreateClientStylistReviewDTO, ReviewClientStylist>();

            config.CreateMap<UpdateClientStylistReviewDTO, ReviewClientStylist>();

        }
    }
}
