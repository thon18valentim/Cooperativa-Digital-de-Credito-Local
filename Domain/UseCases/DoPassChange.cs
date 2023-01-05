using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoPassChange : IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters)
    {
      var userName = parameters.FirstOrDefault(x => x.ParameterName == "UserName").ToStringValue();
      var newPassword = parameters.FirstOrDefault(x => x.ParameterName == "NewPassword").ToStringValue();

      var employee = EmployeeRepository.Find(userName);

      if (employee == default)
        return false;

      employee.PasswordSalt = GenerateSalt();
      var hashedPassword = HashPassword(newPassword, employee.PasswordSalt);
      employee.PasswordHash = hashedPassword;

      return EmployeeRepository.Save();
    }
  }
}
