using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoDisableClient : IUseCase
  {
    public bool Run(string param1, string param2)
    {
      throw new NotImplementedException();
    }

    public bool Run(string cpf)
    {
      var client = ClientRepository.Find(cpf);

      if (client == default)
        return false;

      client.IsActive = false;

      return ClientRepository.Save();
    }
  }
}
