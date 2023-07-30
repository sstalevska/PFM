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
using Microsoft.Extensions.Configuration;
using static PFM.Services.AutoCategorizationService;
using System.Linq.Expressions;
using System.Transactions;
using System.IO;
using System.Text.RegularExpressions;

namespace PFM.Services
{
    public class TransactionService : ITransactionService
    {
        PFM.Database.Repositories.ITransactionRepository _transactionRepository;
        IMapper _mapper;
        PFM.Services.IAutoCategorizationService _autoCategorizationService;
        IConfiguration _configuration;
        ICategoryRepository _categoryRepository;

        public TransactionService(ITransactionRepository transactionRepository,
            IMapper mapper,
            IAutoCategorizationService autoCategorizationService,
            IConfiguration configuration,
            ICategoryRepository categoryRepository
            )
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _autoCategorizationService = autoCategorizationService;
            _configuration = configuration;
            _categoryRepository = categoryRepository;
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

        public async Task<PagedSortedListResponse<Models.Transaction>> GetTransactions(
            string? transactionKind,
            string? startDate,
            string? endDate,
            int page,
            int pageSize,
            SortOrder sortOrder,
            string? sortBy)
        {
            List<ValidationError> errors = new List<ValidationError>();

            string? tk = transactionKind;
            string? sd = startDate;
            string? ed = endDate;
            int p = page;
            int ps = pageSize;
            SortOrder so = sortOrder;
            string? sb = sortBy;

            if(startDate != null)
            {
                try
                {
                    DateTime? parsedStartDate = DateTime.Parse(startDate);
                }
                catch
                {
                    errors.Add(new ValidationError("startDate", "invalid-format", "Invalid format for startDate."));
                    sd = null;
                }
            }
            if(endDate != null)
            {
                try
                {
                    DateTime? parsedendDate = DateTime.Parse(endDate);
                }
                catch
                {
                    errors.Add(new ValidationError("endDate", "invalid-format", "Invalid format for endDate."));
                    ed = null;
                }
            }
           if(transactionKind != null)
            {
                var kinds = Enum.GetValues(typeof(TransactionKind));
                bool flag = false;
                foreach (var kind in kinds)
                {
                    if (kind.Equals(transactionKind))
                    {
                        flag = true;
                    }
                }
                if(!flag)
                {
                    errors.Add(new ValidationError("transactionKinds", "invalid-data", "Transaction kind doesnt exist."));
                    tk = null;
                }
            }
           if((ps > 100))
            {
                ps = 100;
            }
           if(!(so==SortOrder.Asc || so == SortOrder.Desc))
            {
                errors.Add(new ValidationError("sortorder", "invalid-data", "SortOrder doesnt exist."));
                so = SortOrder.Asc;
            }
            if (sortBy != null)
            {
                if(!(sortBy.Equals("id") || sortBy.Equals("beneficiaryname")||
                    sortBy.Equals("date") || sortBy.Equals("direction") ||
                    sortBy.Equals("amount") || sortBy.Equals("description") ||
                    sortBy.Equals("kind") || sortBy.Equals("catcode") ||
                    sortBy.Equals("currency") || sortBy.Equals("mcc") 
                    ))
                {
                    errors.Add(new ValidationError("sortby", "invalid-data", "No such field in transactions."));
                    sb = null;
                }

            }



            var transactions = await _transactionRepository.GetTransactions(
           tk,
           sd,
           ed,
           p,
           ps,
           so,
           sb);

            var pagedSortedList = _mapper.Map<PagedSortedList<Models.Transaction>>(transactions);

            var pagedSortedListResponse = new PagedSortedListResponse<PFM.Models.Transaction>(pagedSortedList, errors);


            return pagedSortedListResponse;
        }




        public async Task<PFM.Models.Transaction> GetTransactionById(string id)
        {
            var transactionEntity = await _transactionRepository.GetTransactionById(id);

            if (transactionEntity == null)
            {
                return null;
            }

            return _mapper.Map<Models.Transaction>(transactionEntity);
        }


        public async Task<CategorizeTransactionResult> CategorizeTransaction(string Id, CategorizeTransactionCommand command)
        {
            

            var entity = _mapper.Map<TransactionEntity>(command);

            var existingTransaction = await _transactionRepository.GetTransactionById(Id);
            var result = new CategorizeTransactionResult();

            if (existingTransaction == null)
            {
                result.Errors.Add(new ValidationError("Transaction", "not-found", "Transaction not found."));
            }
            var cat = await _categoryRepository.GetCategoryByCode(command.catcode);
            if(cat == null)
            {
                result.Errors.Add(new ValidationError("Category", "not-found", "Category not found."));
                return result;
            }

            var categorizeResult = await _transactionRepository.CategorizeTransaction(entity);

            if (categorizeResult == null)
            {
                result.Errors.Add(new ValidationError("error", "unsuccessfull", "Transaction not categorized."));
            }

            result.Result = _mapper.Map<Models.Transaction>(categorizeResult);
            return result;
        }





        public async Task<IEnumerable<PFM.Database.Entities.TransactionEntity>> ReadCSV<TransactionEntity>(Stream file)
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
            List<PFM.Database.Entities.TransactionEntity> transactionEntities = new List<PFM.Database.Entities.TransactionEntity>();

            HashSet<string> processedTransactionIds = new HashSet<string>();

            foreach (var t in transactions)
            {
                // proverka za duplikat vo csv
                if (!processedTransactionIds.Contains(t.id))
                {
                    // proverka za duplikat vo baza
                    bool isDuplicateInDatabase = await _transactionRepository.IsDuplicateTransaction(t.id);

                    if (!isDuplicateInDatabase)
                    {
                        var transactionEntity = _mapper.Map<PFM.Database.Entities.TransactionEntity>(t);
                        transactionEntities.Add(transactionEntity);

                        // isprocesirano id se dodava vo hashset
                        processedTransactionIds.Add(t.id);
                    }

                }
            }

            await _transactionRepository.ImportTransactions(transactionEntities);

            return transactionEntities;
        }






        public async Task AutoCategorizeTransactions()
        {
            await _autoCategorizationService.AutoCategorizeTransactions();

        }

        public bool IsFileCsvFormat(Stream fileStream)
        {
            // Read the first few lines of the file
            using var reader = new StreamReader(fileStream);
            const int NumberOfLinesToCheck = 5;
            var lines = new string[NumberOfLinesToCheck];
            for (int i = 0; i < NumberOfLinesToCheck; i++)
            {
                if (reader.EndOfStream)
                    break;
                lines[i] = reader.ReadLine();
            }

            // Check if the content follows the CSV format
            const string CsvPattern = @"^([^,\r\n]*,){2,}[^,\r\n]*$";
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                    continue;

                if (!Regex.IsMatch(lines[i], CsvPattern))
                {
                    return false;
                }
            }

            return true;
        }


    }



}