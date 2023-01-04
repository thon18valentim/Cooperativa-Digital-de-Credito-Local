using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Enums;
using AdaCredit.Utils;
using Bogus;
using AdaCredit.Infra.Repositories;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoCreateClient : IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters = null)
    {
      Client client;
      Account account = new();
      bool IsUniqueCpf = false;

      do
      {

        client = new Faker<Client>()
          .CustomInstantiator(f => new Client())
          .RuleFor(c => c.Gender, f => f.PickRandom<Gender>())
          .RuleFor(c => c.Name, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
          .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
          .RuleFor(c => c.Country, f => f.Address.Country())
          .RuleFor(c => c.City, f => f.Address.City())
          .RuleFor(c => c.StreetAddress, f => f.Address.StreetAddress())
          .FinishWith((f, c) => {

            var cpf = Util.GenerateCpf();
            c.Cpf = cpf;

            var createAccount = new DoCreateAccount();
            createAccount.Run();
            account = createAccount.GetAccount();
            c.ClientAccountId = account.Id;

          });

        if (!ClientRepository.RegisteredClients.Any(c => c.Cpf == client.Cpf))
          IsUniqueCpf = true;

      } while (!IsUniqueCpf);

      client.IsActive = true;

      ClientRepository.Add(client);
      AccountRepository.Add(account);

      Console.WriteLine($"Cliente {client.Name} criado com sucesso!");

      if (ClientRepository.Save() && AccountRepository.Save())
        return true;

      return false;
    }
  }
}
