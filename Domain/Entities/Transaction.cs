using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities.Enums;

namespace AdaCredit.Domain.Entities
{
  public sealed class Transaction
  {
    public string HomeBankCode { get; set; }
    public string HomeBankAgency { get; set; }
    public string HomeBankAccount { get; set; }
    public string DestinationBankCode { get; set; }
    public string DestinationBankAgency { get; set; }
    public string DestinationBankAccount { get; set; }
    public TransactionType Type { get; set; }
    public int Entry { get; set; }
    public decimal Value { get; set; }

    public Transaction() { }
  }
}
