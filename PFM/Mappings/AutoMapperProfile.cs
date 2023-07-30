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


            CreateMap<TransactionCSVCommand, TransactionEntity>();
            CreateMap<CategoryCSVCommand, CategoryEntity>();


            CreateMap<CategoryEntity, Category>();
            CreateMap<Category, CategoryEntity>();

            CreateMap<Split, SplitEntity>();
            CreateMap<SplitEntity, Split>();


            CreateMap<PagedSortedListResponse<TransactionEntity>, PagedSortedList<Transaction>>();



        }


    }
}
