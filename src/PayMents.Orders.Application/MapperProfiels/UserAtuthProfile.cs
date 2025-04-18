using AutoMapper;
using PayMent.Orders.Domain.Items;
using PayMents.Orders.Application.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.MapperProfiels;

public class UserAtuthProfile : Profile
{
    public UserAtuthProfile() 
    {
        CreateMap<UserRegisterDto, UserIdentity>()
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom((src  => src.Phone)))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

        CreateMap<UserIdentity, UserResponse>()
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Roles, opt => opt.Ignore())  
            .ForMember(dest => dest.Token, opt => opt.Ignore()); 
    }
}
