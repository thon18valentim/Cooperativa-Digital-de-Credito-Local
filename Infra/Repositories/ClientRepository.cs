using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;

namespace AdaCredit.Infra.Repositories
{
  public sealed class ClientRepository
  {
    private List<Client> clients;

    public IEnumerable<Client> Clients { get; set; }

    static ClientRepository()
    {
      LoadRegisteredClients();
    }

    private static void LoadRegisteredClients()
    {
      /**
       * Carregar clients cadastrados previamente
       */
    }
  }
}
