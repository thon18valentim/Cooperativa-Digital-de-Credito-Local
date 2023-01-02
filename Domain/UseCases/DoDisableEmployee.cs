using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoDisableEmployee : IUseCase
  {
    public bool Run(string param1, string param2)
    {
      throw new NotImplementedException();
    }

    public bool Run(string userName)
    {
      if (EmployeeRepository.CountDisable() == EmployeeRepository.RegisteredEmployees.Count - 1)
        return false;

      var employee = EmployeeRepository.Find(userName);

      if (employee == default)
        return false;

      employee.IsActive = false;

      return EmployeeRepository.Save();
    }
  }
}
