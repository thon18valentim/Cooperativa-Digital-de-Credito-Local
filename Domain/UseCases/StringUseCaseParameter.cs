
namespace AdaCredit.Domain.UseCases
{
  public class StringUseCaseParameter : IUseCaseParameter
  {
    public object ParameterValue { get; set; }
    public string ParameterName { get; set; } = string.Empty;
  }
}
