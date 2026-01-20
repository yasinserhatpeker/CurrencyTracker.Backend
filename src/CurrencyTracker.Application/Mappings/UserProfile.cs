using System;
using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Domain.Entities;

namespace CurrencyTracker.Application.Mappings;

public class UserProfile : Profile
{
   public UserProfile()
    {
        CreateMap<CreateUserDTO,User>().ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)); // CreateMap<Source,Destination>(); src-> password == dest-> passwordHash
        CreateMap<User,UserResponseDTO>();

    }
}
