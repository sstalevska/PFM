using PFM.Commands;
using PFM.Database.Entities;
using PFM.Database.Repositories;
using PFM.Exceptions;
using PFM.Models;

namespace PFM.Services
{
    public class SplitService : ISplitService
    {
        ISplitRepository _splitRepository;
        ITransactionRepository _transactionRepository;
        ICategoryRepository _categoryRepository;
        public SplitService(ISplitRepository splitRepository, ITransactionRepository transactionRepository, ICategoryRepository categoryRepository)
        {
            _splitRepository = splitRepository;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ValidationError>> SplitTransaction(string transactionId, List<SplitCommand> splits)
        {
            List<ValidationError> errors = new List<ValidationError>();

            //Validacija dali postoi transakcijata
            var transaction = await _transactionRepository.GetTransactionById(transactionId);
            if (transaction == null)
            {
                var e = new ValidationError(
                    "transaction", 
                    "not-found", 
                    "Transaction with id {transactionId} not found."
                );
                errors.Add(e);
               // return errors;
                //throw new NotFoundException("Transaction not found.");
            }

            // Validacija za splits
            var totalAmount = 0.0;
            foreach (var split in splits)
            {
                var category = await _categoryRepository.GetCategoryByCode(split.catcode);
                if (category == null)
                {
                    var e = new ValidationError(
                   "category",
                   "not-found",
                   "Category with code {split.catcode} not found."
               );
                    errors.Add(e);
                    // throw new NotFoundException($"Category with code {split.catcode} not found.");
                }
                if(split.amount <= 0)
                {
                    var e = new ValidationError(
                    "amount",
                    "invalid-amount",
                    "Split amount must be greater than zero."
                );
                    errors.Add(e);
                    // throw new InvalidDataException($"Split amount must be greater than zero.");

                }
                totalAmount += split.amount;
            }
            if ((transaction != null) &&(totalAmount!= transaction.amount))
            {
                var e = new ValidationError(
                   "amount",
                   "invalid-amount",
                   "Sum of split amounts must be equal to the transaction amount."
               );
                errors.Add(e);
                //  throw new InvalidDataException($"Sum of split amounts must be equal to the transaction amount.");

            }

            
            // Brishenje ako ima postoechki splits
            await _splitRepository.DeleteSplitsByTransactionId(transactionId);

            if(splits.Count < 2)
            {
                var e = new ValidationError(
                  "split",
                  "invalid-data",
                  "Splits must be two or more."
              );
                errors.Add(e);
                //  throw new InvalidDataException("Splits must be two or more.");
            }

            if(transaction != null)
            {
                // Kreiranje i dodavanje novi splits
                var splitEntities = new List<SplitEntity>();
                foreach (var split in splits)
                {

                    var category = await _categoryRepository.GetCategoryByCode(split.catcode);
                    var splitEntity = new SplitEntity
                    {
                        transactionid = transactionId,
                        catcode = category.code,
                        amount = split.amount
                    };
                    splitEntities.Add(splitEntity);
                }

                await _splitRepository.AddSplits(splitEntities);
            }
           

            return errors;
        }

    }
}
