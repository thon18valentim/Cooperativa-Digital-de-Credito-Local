using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Domain.UseCases
{
  public class StringUseCaseParameter : IUseCaseParameter
  {
    public object ParameterValue { get => (string)ParameterValue; set => ParameterValue = value; }
    public string ParameterName { get; set; } = string.Empty;

    public static implicit operator StringUseCaseParameter(string s) => new StringUseCaseParameter() { ParameterValue = s };

    public static implicit operator StringUseCaseParameter((string name, string value) tuple) => new() { ParameterValue = tuple.value, ParameterName = tuple.name };
  }
}
