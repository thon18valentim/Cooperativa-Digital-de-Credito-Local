using AdaCredit.Domain.Entities;
using Bogus;
using AdaCredit.Infra.Repositories;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoCreateAccount : IUseCase
  {
    Account? account;

    public bool Run(IEnumerable<IUseCaseParameter> parameter = null)
    {
      bool IsUniqueAccount = false;

      do
      {

        account = new Faker<Account>()
          .CustomInstantiator(f => new Account())
          .RuleFor(a => a.Number, f => f.Random.ReplaceNumbers("#####-#"))
          .FinishWith((f, a) =>
          {

            a.Id = AccountRepository.GetNextId();
            a.AgencyNumber = "0001";
            a.Balance = 0.00M;

          });

        if (!AccountRepository.RegisteredAccounts.Any(e => e.Number == account.Number))
          IsUniqueAccount = true;

      } while (!IsUniqueAccount);

      return true;
    }

    public Account? GetAccount()
      => account;
  }
}
