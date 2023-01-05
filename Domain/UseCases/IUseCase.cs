
namespace AdaCredit.Domain.UseCases
{
  public interface IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters);
  }
}
