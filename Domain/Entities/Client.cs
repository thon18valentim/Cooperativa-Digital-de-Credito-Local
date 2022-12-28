using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities.Enums;

namespace AdaCredit.Domain.Entities
{
  public sealed class Client
  {
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public string Cpf { get; set; }
    public int ClientAccountId { get; set; }
    public bool IsActive { get; set; }

    public Client(string name, string lastName, Gender gender, string cpf, string accountNumber)
    {
      Name = name;
      Gender = gender;
      Cpf = cpf;
    }

    public Client() { }
  }
}
