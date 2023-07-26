using PFM.Commands;
using PFM.Database.Entities;
using PFM.Database.Repositories;
using PFM.Exceptions;

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

        public async Task SplitTransaction(string transactionId, List<SplitCommand> splits)
        {
            //Validacija dali postoi transakcijata
            var transaction = await _transactionRepository.GetTransactionById(transactionId);
            if (transaction == null)
            {
                throw new NotFoundException("Transaction not found.");
            }

            // Validacija dali postoi kategorijata
            foreach (var split in splits)
            {
                var category = await _categoryRepository.GetCategoryByCode(split.catcode);
                if (category == null)
                {
                    throw new NotFoundException($"Category with code {split.catcode} not found.");
                }
            }

            // Brishenje ako ima postoechki splits
            await _splitRepository.DeleteSplitsByTransactionId(transactionId);

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

    }
}
