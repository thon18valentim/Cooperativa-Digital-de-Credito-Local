using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace AdaCredit.Domain.Entities.Maps
{
  public class TransactionMap : ClassMap<Transaction>
  {
    public TransactionMap()
    {
      Map(m => m.HomeBankCode).Name("homeBankCode");
      Map(m => m.HomeBankAgency).Name("homeBankAgency");
      Map(m => m.HomeBankAccount).Name("homeBankNumber");
      Map(m => m.DestinationBankCode).Name("destinationBankCode");
      Map(m => m.DestinationBankAgency).Name("destinationBankAgency");
      Map(m => m.DestinationBankAccount).Name("destinationBankNumber");
      Map(m => m.Type).Name("type");
      Map(m => m.Entry).Name("entry");
      Map(m => m.Value).Name("value");
    }
  }
}
