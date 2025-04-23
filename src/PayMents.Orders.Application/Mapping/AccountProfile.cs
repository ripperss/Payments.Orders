using AutoMapper;
using PayMents.Orders.Application.Models.Account;
using PayMent.Orders.Domain.Items;

namespace PayMents.Orders.Application.Mapping;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<UserIdentity, AccountRequest>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

        CreateMap<AccountResponse, UserIdentity>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));
    }
} 