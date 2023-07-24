using AutoMapper;
using PFM.Commands;
using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<TransactionEntity, Transaction>();
            CreateMap<Transaction, TransactionEntity>();

            CreateMap<PagedSortedList<TransactionEntity>, PagedSortedList<Transaction>>();

            CreateMap<CategorizeTransactionCommand, TransactionEntity>(); 

            CreateMap<CreateTransactionCommand, TransactionEntity>();

            CreateMap<TransactionCSVCommand, TransactionEntity>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.id))
                .ForMember(d => d.BeneficiaryName, opts => opts.MapFrom(s => s.beneficiaryname))
                .ForMember(d => d.Date, opts => opts.MapFrom(s => s.date))
                .ForMember(d => d.Direction, opts => opts.MapFrom(s => s.direction))
                .ForMember(d => d.Amount, opts => opts.MapFrom(s => s.amount))
                .ForMember(d => d.Description, opts => opts.MapFrom(s => s.description))
                .ForMember(d => d.Currency, opts => opts.MapFrom(s => s.currency))
                .ForMember(d => d.Mcc, opts => opts.MapFrom(s => s.mcc))
                .ForMember(d => d.Kind, opts => opts.MapFrom(s => s.kind))
                .ForMember(d => d.CatCode, opts => opts.MapFrom(s => s.catcode))
                .ForMember(d => d.Splits, opts => opts.MapFrom(s => s.splits));


            CreateMap<CategoryEntity, Category>();
            CreateMap<Category, CategoryEntity>();

            CreateMap<Split, SplitEntity>();
            CreateMap<SplitEntity, Split>();

        }


    }
}
