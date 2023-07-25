using AutoMapper;
using PFM.Commands;
using PFM.Database.Repositories;
using PFM.Models;
using PFM.Models.Enums;
using PFM.Mappings;
using PFM.Database.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace PFM.Services
{
    public class TransactionService : ITransactionService
    {
        ITransactionRepository _transactionRepository;
        IMapper _mapper;
        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        private async Task<bool> CheckIfTransactionExists(string id)
        {
            var transaction = await _transactionRepository.GetTransactionById(id);
            if (transaction == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<PagedSortedList<Models.Transaction>> GetTransactions(
            string? transactionKind,
            string? startDate,
            string? endDate,
            int page,
            int pageSize,
            SortOrder sortOrder,
            string? sortBy)
        {
            var transactions = await _transactionRepository.GetTransactions(
           transactionKind,
           startDate,
           endDate,
           page,
           pageSize,
           sortOrder,
           sortBy);
            return _mapper.Map<PagedSortedList<Models.Transaction>>(transactions);
        }



        public async Task<Models.Transaction> GetTransactionById(string id)
        {
            var transactionEntity = await _transactionRepository.GetTransactionById(id);

            if (transactionEntity == null)
            {
                return null;
            }

            return _mapper.Map<Models.Transaction>(transactionEntity);
        }

        public async Task<Models.Transaction> CreateTransaction(CreateTransactionCommand command)
        {
            var entity = _mapper.Map<TransactionEntity>(command);

            var existingTransaction = await _transactionRepository.GetTransactionById(command.Id);
            if (existingTransaction != null)
            {
                return null;
            }
            var result = await _transactionRepository.CreateTransaction(entity);

            return _mapper.Map<Models.Transaction>(result);
        }
        public async Task<Models.Transaction> CategorizeTransaction(string Id, CategorizeTransactionCommand command)
        {
            var entity = _mapper.Map<TransactionEntity>(command);

            var existingTransaction = await _transactionRepository.GetTransactionById(Id);
            if (existingTransaction == null)
            {
                return null;
            }
            var result = await _transactionRepository.CategorizeTransaction(entity);

            return _mapper.Map<Models.Transaction>(result);
        }


        public async Task<bool> DeleteTransaction(string id)
        {
            return await _transactionRepository.DeleteTransaction(id);
        }


        public IEnumerable<Transaction> ReadCSV<Transaction>(Stream file)
        {


            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace("-", ""),
                HeaderValidated = null,
                 MissingFieldFound = null
            };

            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, config);
            var transactions = csv.GetRecords<TransactionCSVCommand>();
            List<TransactionEntity> transactionEntities = new List<TransactionEntity>();
            foreach (var t in transactions)
            {
                var transactionEntity = _mapper.Map<TransactionEntity>(t);
                transactionEntities.Add(transactionEntity);
            }
            _transactionRepository.ImportTransactions(transactionEntities);
            var trans = csv.GetRecords<Transaction>();
            return trans;

        }

    }
   
 }

