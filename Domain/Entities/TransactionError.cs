using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Domain.Entities
{
  public sealed class TransactionError
  {
    public string Message { get; set; }
    public DateTime Date { get; set; }

    public TransactionError(string message)
    {
      Message = message;
      Date = DateTime.Now;
    }

    public TransactionError() { }
  }
}
