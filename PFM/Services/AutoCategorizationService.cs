using DynamicExpresso.Exceptions;
using DynamicExpresso;
using PFM.Database.Entities;
using PFM.Models;
using PFM.Database.Repositories;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;


namespace PFM.Services
{

    public class AutoCategorizationService : IAutoCategorizationService
    {
        private readonly IConfiguration _configuration;
        private readonly ITransactionRepository _transactionRepository;

        public AutoCategorizationService(IConfiguration configuration, ITransactionRepository transactionRepository)
        {
            _configuration = configuration;
            _transactionRepository = transactionRepository;
        }

        public async Task AutoCategorizeTransactions()
        {
            var transactions = await _transactionRepository.GetTransactionsWithoutCategory();
            var rules = GetAutoCategorizationRules();

            foreach (var transaction in transactions)
            {
                var matchingRule = FindMatchingRule(transaction, rules);

                if (matchingRule != null)
                {
                    transaction.catcode = matchingRule.CatCode;
                }
            }

            await _transactionRepository.SaveChanges();
        }

        private List<AutoCategorizationRule> GetAutoCategorizationRules()
        {
            return _configuration.GetSection("AutoCategorizationRules").Get<List<AutoCategorizationRule>>();
        }

        private AutoCategorizationRule FindMatchingRule(TransactionEntity transaction, List<AutoCategorizationRule> rules)
        {
            var interpreter = new Interpreter();

            interpreter.SetVariable("beneficiaryname", transaction.beneficiaryname);
            interpreter.SetVariable("mcc", transaction.mcc);

            foreach (var rule in rules)
            {
                try
                {
                    var parsedPredicate = interpreter.Parse(rule.Predicate);
                    var isMatch = parsedPredicate.Invoke();

                    if (isMatch is bool matchResult && matchResult)
                    {
                        return rule;
                    }
                }
                catch (ParseException ex)
                {
                    var errorMessage = $"Error parsing the predicate for rule: {rule.Title}.";
                    throw new ParseException(errorMessage, ex.Position);
                }
            }

            return null;
        }
    }
}

