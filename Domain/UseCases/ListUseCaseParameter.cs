using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Domain.UseCases
{
  public class ListUseCaseParameter<T> : IUseCaseParameter
  {
    public object ParameterValue { get => (List<T>)ParameterValue; set => ParameterValue = value; }
    public string ParameterName { get; set; } = string.Empty;

    public static implicit operator ListUseCaseParameter<T>(List<T> list) => new() { ParameterValue = list };
    public static implicit operator ListUseCaseParameter<T>((string name, List<T> value) tuple) => new() { ParameterValue = tuple.value, ParameterName = tuple.name };
  }
}
