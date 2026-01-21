using System;
using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Domain.Entities;

namespace CurrencyTracker.Application.Mappings;

public class MappingProfile : Profile
{
   public MappingProfile()
    {
        CreateMap<CreateUserDTO,User>().ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)); // CreateMap<Source,Destination>(); src-> password == dest-> passwordHash
        CreateMap<User,UserResponseDTO>();
        CreateMap<UpdateUserDTO,User>().ForMember(dest =>dest.PasswordHash,opt => opt.MapFrom(src => src.Password));

        CreateMap<CreatePortfolioDTO,Portfolio>();
        CreateMap<Portfolio,PortfolioResponseDTO>();



    }
}
