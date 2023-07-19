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
        }


    }
}
