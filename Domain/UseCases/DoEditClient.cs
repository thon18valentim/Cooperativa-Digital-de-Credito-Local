﻿using AdaCredit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using AdaCredit.Domain.Entities;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoEditClient : IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters)
    {
      // string name, string cpf, string oldCpf, bool activeConfigSelected

      var name = parameters.FirstOrDefault(x => x.ParameterName == "Name").ToStringValue();
      var cpf = parameters.FirstOrDefault(x => x.ParameterName == "Cpf").ToStringValue();
      var oldCpf = parameters.FirstOrDefault(x => x.ParameterName == "OldCpf").ToStringValue();
      var activeConfigSelected = parameters.FirstOrDefault(x => x.ParameterName == "ActiveConfigSelected").ToBool();

      if (name == null || cpf == null)
        return false;

      cpf = cpf.Trim();
      cpf = cpf.Replace(".", "").Replace("-", "");

      if (!Util.ValidateCpf(cpf))
      {
        Console.WriteLine("Erro, cpf inválido");
        Thread.Sleep(3000);
        return false;
      }

      if (ClientRepository.Find(cpf) != null)
      {
        Console.WriteLine("Erro, cpf já existente");
        Thread.Sleep(3000);
        return false;
      }

      var client = ClientRepository.Find(oldCpf);

      client.Name = name;
      client.Cpf = cpf;
      client.IsActive = activeConfigSelected;

      return ClientRepository.Save();
    }
  }
}
