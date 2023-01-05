using AdaCredit.Domain.Entities.Enums;
using CsvHelper.Configuration.Attributes;

namespace AdaCredit.Domain.Entities
{
  public sealed class Transaction
  {
    public string OriginBankCode { get; set; }
    public string OriginBankAgency { get; set; }
    public string OriginBankAccount { get; set; }
    public string DestinationBankCode { get; set; }
    public string DestinationBankAgency { get; set; }
    public string DestinationBankAccount { get; set; }
    public TransactionType Type { get; set; }
    public int Entry { get; set; }
    public decimal Value { get; set; }

    [Ignore]
    public string BankName { get; set; }

    [Ignore]
    public DateTime Date { get; set; }

    [Ignore]
    public string ErrorMessage { get; set; }

    [Ignore]
    public DateTime ErrorDate { get; set; }

    public Transaction() { }
  }
}
