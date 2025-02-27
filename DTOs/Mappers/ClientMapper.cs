using AutoMapper;
using DTOs.Appointment;
using DTOs.Client;
using DTOs.ServiceStylist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class ClientMapper : BaseMapper
    {
        public ClientMapper(IMapper mapper) : base(mapper)
        {

        }
        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.User, ViewClientDetailsDTO>()
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                 .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture != null ? src.ProfilePicture.Path : null));

            config.CreateMap<DA.Entities.ServiceStylist, ServiceStylistDTO>()
                 .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.Service.Name))
                 .ForMember(dest => dest.Stylist, opt => opt.MapFrom(src => $"{src.Stylist.User.FirstName} {src.Stylist.User.LastName}"));


        }
    }
}