using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Domain.Entities
{
  public sealed class Client : User
  {
    public string Cpf { get; set; }
    public Account ClientAccount { get; private set; }
    public bool IsActive { get; set; }

    public Client(string name, string cpf, string accountNumber)
    {
      base.Name = name;
      Cpf = cpf;
      ClientAccount = new Account(accountNumber);
    }
  }
}
