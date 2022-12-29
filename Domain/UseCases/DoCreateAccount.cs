using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;
using AdaCredit.Utils;
using Bogus;
using AdaCredit.Infra.Repositories;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoCreateAccount : IUseCase
  {
    public bool Run(string param1, string param2)
    {
      throw new NotImplementedException();
    }

    public Account Run()
    {
      Account account;
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

        // conferindo se conta é unica
        if (!AccountRepository.RegisteredAccounts.Any(e => e.Number == account.Number))
          IsUniqueAccount = true;

      } while (!IsUniqueAccount);

      return account;
    }
  }
}
