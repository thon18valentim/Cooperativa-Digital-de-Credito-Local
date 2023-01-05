using CsvHelper.Configuration;

namespace AdaCredit.Domain.Entities.Maps
{
  public class TransactionErrorMap : ClassMap<TransactionError>
  {
    public TransactionErrorMap()
    {
      Map(m => m.Message).Name("message");
      Map(m => m.Date).Name("date");
    }
  }
}
