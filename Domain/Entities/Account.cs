using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Domain.Entities
{
  public sealed class Account
  {
    public string Number { get; set; }
    public long AgencyNumber { get; set; }

    public Account(string number)
    {
      Number = number;
      AgencyNumber = 0001;
    }
  }
}
