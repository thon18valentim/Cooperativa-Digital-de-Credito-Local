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
    public bool Run(string param1, string param2)
    {
      throw new NotImplementedException();
    }

    public bool Run()
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
          .FinishWith((f, c) => {

            var cpf = Util.GenerateCpf();
            c.Cpf = cpf;

            account = new DoCreateAccount().Run();
            c.ClientAccountId = account.Id;

          });

        // conferindo se cliente é unico
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
