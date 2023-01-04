using AdaCredit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using AdaCredit.Domain.Entities;
using Spectre.Console;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoEditClient : IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters)
    {
      var name = parameters.FirstOrDefault(x => x.ParameterName == "Name").ToStringValue();
      var cpf = parameters.FirstOrDefault(x => x.ParameterName == "Cpf").ToStringValue();
      var oldCpf = parameters.FirstOrDefault(x => x.ParameterName == "OldCpf").ToStringValue();
      var phone = parameters.FirstOrDefault(x => x.ParameterName == "Phone").ToStringValue();
      var country = parameters.FirstOrDefault(x => x.ParameterName == "Country").ToStringValue();
      var city = parameters.FirstOrDefault(x => x.ParameterName == "City").ToStringValue();
      var streetAddress = parameters.FirstOrDefault(x => x.ParameterName == "StreetAddress").ToStringValue();
      var activeConfigSelected = parameters.FirstOrDefault(x => x.ParameterName == "ActiveConfigSelected").ToBool();

      if (name == null || cpf == null)
        return false;

      cpf = cpf.Trim();
      cpf = cpf.Replace(".", "").Replace("-", "");

      if (!Util.ValidateCpf(cpf))
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup("[bold red]Erro, cpf inválido[/]"));
        Thread.Sleep(3000);
        return false;
      }

      if (ClientRepository.Find(cpf) != null)
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup("[bold red]Erro, cpf já existente[/]"));
        Thread.Sleep(3000);
        return false;
      }

      var client = ClientRepository.Find(oldCpf);

      client.Name = name;
      client.Cpf = cpf;
      client.Phone = phone.Trim();
      client.Country = country;
      client.City = city;
      client.StreetAddress = streetAddress;
      client.IsActive = activeConfigSelected;

      return ClientRepository.Save();
    }
  }
}
