using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Domain.UseCases
{
  public interface IUseCaseParameter
  {
    object ParameterValue { get; set; }
    string ParameterName { get; set; }
  }
}
