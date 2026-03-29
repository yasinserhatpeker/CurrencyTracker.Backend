using System;
using AutoMapper;
using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.DTOs.Portfolios;
using CurrencyTracker.Application.DTOs.Transactions;
using CurrencyTracker.Application.DTOs.Users;
using CurrencyTracker.Domain.Entities;

namespace CurrencyTracker.Application.Mappings;

public class MappingProfile : Profile
{
   public MappingProfile()
    {
        CreateMap<CreateUserDTO,User>().ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        CreateMap<User,UserResponseDTO>();
        CreateMap<UpdateUserDTO,User>().ForMember(dest =>dest.PasswordHash,opt => opt.Ignore());

       // Portfolio Mapping
        CreateMap<CreatePortfolioDTO,Portfolio>();
        CreateMap<Portfolio,PortfolioResponseDTO>();
        CreateMap<UpdatePortfolioDTO,Portfolio>();

        // Transaction Mapping
        CreateMap<CreateTransactionDTO,Transaction>();
        CreateMap<Transaction,TransactionResponseDTO>().ForMember(dest => dest.TotalValue, opt => opt.MapFrom (src => src.Price * src.Quantity)); // totalValue = Price * Quantity
        CreateMap<UpdateTransactionDTO,Transaction>();


    }
}
