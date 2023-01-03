using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoDisableClient : IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters)
    {
      var cpf = parameters.FirstOrDefault(x => x.ParameterName == "Cpf").ToStringValue();

      var client = ClientRepository.Find(cpf);

      if (client == default)
        return false;

      client.IsActive = false;

      return ClientRepository.Save();
    }
  }
}
