
namespace AdaCredit.Domain.UseCases
{
  public interface IUseCaseParameter
  {
    object ParameterValue { get; set; }
    string ParameterName { get; set; }
  }
}
