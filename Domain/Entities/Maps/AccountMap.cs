using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
