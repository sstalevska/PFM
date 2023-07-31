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

        public async Task<List<ValidationError>> AutoCategorizeTransactions()
        {
            var transactions = await _transactionRepository.GetTransactionsWithoutCategory();
            var rules = GetAutoCategorizationRules();

            var errors = new List<ValidationError>();


            foreach (var t in transactions)
            {
                if(errors.Count > 0)
                {
                    return errors;
                }
                var response = FindMatchingRule(t, rules);

                if (response.Rule != null)
                {
                    t.catcode = response.Rule.CatCode;
                }else if(response.Errors.Count >0){
                    errors.AddRange(response.Errors);
                }
            }

            await _transactionRepository.SaveChanges();
            return errors;
        }

        private List<AutoCategorizationRule> GetAutoCategorizationRules()
        {
            return _configuration.GetSection("AutoCategorizationRules").Get<List<AutoCategorizationRule>>();
        }

        public RuleErrorListResponse<AutoCategorizationRule> FindMatchingRule(TransactionEntity transaction, List<AutoCategorizationRule> rules)
        {
            var errors = new List<ValidationError>();

            var r = new AutoCategorizationRule();
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
                        r = rule;
                    }
                }
                catch 
                {
                    var error = new ValidationError("rule-parsing", "invalid-predicate", $"Error parsing the predicate for rule: {rule.Title}");
                    errors.Add(error);
                    r = null;
                }
            }

            var response  = new RuleErrorListResponse<AutoCategorizationRule>(r, errors);
            return response;
        }
    }
}

