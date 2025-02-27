using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Common.Enums;
using DTOs.Appointment;
using DTOs.ServiceStylist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class AppointmentMapper : BaseMapper
    {
        public AppointmentMapper(IMapper mapper) : base(mapper)
        {

        }

        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<AppointmentDTO, DA.Entities.Appointment>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(s => (int)AppointmentStatus.Pending))
                .ForMember(dest => dest.ServiceStylists, opt => opt.Ignore());

            config.CreateMap<CreateAppointmentServiceStylistDTO, DA.Entities.ServiceStylist>();

            config.CreateMap<DA.Entities.ServiceStylist, ServiceStylistDTO>();
            
            config.CreateMap<DA.Entities.Appointment, AppointmentDTO>()
                .ForMember(dest => dest.Services, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
                 .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"));

            config.CreateMap<DA.Entities.Appointment, DisplayStylistAppointmentsDTO>()
                .ForMember(dest => dest.Services, opt => opt.Ignore())
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"));
                


            config.CreateMap<DA.Entities.Appointment, AppointmentDetailsDTO>()
                .ForMember(dest => dest.Services, opt => opt.Ignore())
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Client.Id));


            config.CreateMap<CreateAppointmentDTO, DA.Entities.Appointment>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(s => (int)AppointmentStatus.Pending))
                .ForMember(dest => dest.ServiceStylists, opt => opt.Ignore());

            config.CreateMap<ServiceStylistDTO, DA.Entities.ServiceStylist>();


        }

   }
}
