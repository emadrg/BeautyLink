using AutoMapper;
using DA.Entities;
using DTOs.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class ReviewClientSalonMaper : BaseMapper
    {
        public ReviewClientSalonMaper(IMapper mapper) : base(mapper)
        {

        }

        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<ReviewClientSalon, DisplayClientSalonReviewDTO>()
                 .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"));

            config.CreateMap<CreateClientSalonReviewDTO, ReviewClientSalon>();

            config.CreateMap<UpdateClientSalonReviewDTO, ReviewClientSalon>();

        }
    }
}
