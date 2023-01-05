using CsvHelper.Configuration;

namespace AdaCredit.Domain.Entities.Maps
{
  public class AccountMap : ClassMap<Account>
  {
    public AccountMap()
    {
      Map(m => m.Id).Name("id");
      Map(m => m.Number).Name("number");
      Map(m => m.AgencyNumber).Name("agencyNumber");
      Map(m => m.Balance).Name("balance");
    }
  }
}
