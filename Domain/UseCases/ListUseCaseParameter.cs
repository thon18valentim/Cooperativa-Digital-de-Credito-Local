
namespace AdaCredit.Domain.UseCases
{
  public class ListUseCaseParameter<T> : IUseCaseParameter
  {
    public object ParameterValue { get; set; }
    public string ParameterName { get; set; } = string.Empty;
  }
}
