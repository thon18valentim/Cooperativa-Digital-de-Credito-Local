using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoPassVerification : IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters)
    {
      var userName = parameters.FirstOrDefault(x => x.ParameterName == "UserName").ToStringValue();
      var password = parameters.FirstOrDefault(x => x.ParameterName == "OldPassword").ToStringValue();

      var employee = EmployeeRepository.Find(userName);

      if (employee == default)
        return false;

      var hashPassword = HashPassword(password, employee.PasswordSalt);

      if (hashPassword == employee.PasswordHash)
        return true;

      return false;
    }
  }
}
