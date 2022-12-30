using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
