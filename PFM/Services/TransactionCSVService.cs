using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using PFM.Database.Repositories;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using PFM.Database.Entities;
using static Npgsql.PostgresTypes.PostgresCompositeType;
using System.Reflection;
using System;
using PFM.Commands;

namespace PFM.Services
{
    public class TransactionCSVService : ITransactionCSVService
    {
        ITransactionRepository _transactionRepository;
        IMapper _mapper;


        public TransactionCSVService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public IEnumerable<Transaction> ReadCSV<Transaction>(Stream file)
        {


            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace("-", ""),
               HeaderValidated = null,
              // MissingFieldFound = null
            };

            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, config);
            var transactions = csv.GetRecords<TransactionCSVCommand>();
            List<TransactionEntity> transactionEntities = new List<TransactionEntity>();    
            foreach( var t in transactions)
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
