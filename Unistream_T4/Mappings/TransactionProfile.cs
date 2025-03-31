using AutoMapper;
using Unistream_T4.Dto;
using Unistream_T4.Models.Abstractions;
using Unistream_T4.Models.Transactions;

namespace Unistream_T4.Mappings
{
    public class TransactionProfile:Profile
    {
        public TransactionProfile()
        {
            //Маппинг входящих Dto
            CreateMap<TransactionDto, Transaction>()
            .Include<TransactionDto, DebitTransaction>()
            .Include<TransactionDto, CreditTransaction>()
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(s => s.DateTime.UtcDateTime))
            .ForMember(dest => dest.TimeZoneOffset, opt => opt.MapFrom(s => s.DateTime.Offset));
            CreateMap<TransactionDto, DebitTransaction>();
            CreateMap<TransactionDto, CreditTransaction>();

            //Маппинг Dto-ответов
            CreateMap<Transaction, AddTransactionResponseDto>()
                .ForMember(dest=>dest.ClientBalance,opt=>opt.MapFrom(s=>s.ClientBalanceAfterAddTransaction))
                .ForMember(dest => dest.InsertDateTime, opt => opt.MapFrom(s => s.InsertDateTime.ToLocalTime()));
            CreateMap<Transaction, RevertTransactionResponseDto>()
                .ForMember(dest => dest.ClientBalance, opt => opt.MapFrom(s => s.ClientBalanceAfterRevertTransaction))
                .ForMember(dest => dest.RevertDateTime, opt => opt.MapFrom(s => s.RevertDateTime!.Value.ToLocalTime()));
            CreateMap<ClientBalance, ClientBalanceReponseDto>()
                .ForMember(dest => dest.ClientBalance, opt => opt.MapFrom(s => s.Balance))
                .ForMember(dest => dest.BalanceDateTime, opt => opt.MapFrom(s => s.LastCalculationDateTime.ToLocalTime()));
        }

    }
}
