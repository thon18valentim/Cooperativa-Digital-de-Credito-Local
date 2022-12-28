using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoLogin : IUseCase
  {
    public bool Run(string userName, string password)
    {
      var employee = EmployeeRepository.Find(userName);

      if (employee == default)
        return false;

      if (!employee.IsActive)
        return false;

      var hashPassword = HashPassword(password, employee.PasswordSalt);

      if (hashPassword == employee.PasswordHash)
      {
        employee.LastLogin = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        EmployeeRepository.Save();
        return true;
      }

      return false;
    }
  }
}
