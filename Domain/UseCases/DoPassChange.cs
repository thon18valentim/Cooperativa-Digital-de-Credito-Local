using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoPassChange : IUseCase
  {
    public bool Run(string userName, string newPassword)
    {
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
