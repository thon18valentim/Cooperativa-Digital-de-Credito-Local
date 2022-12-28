using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoPassVerification : IUseCase
  {
    public bool Run(string userName, string password)
    {
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
