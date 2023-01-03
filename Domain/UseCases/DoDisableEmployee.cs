using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoDisableEmployee : IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters)
    {
      var userName = parameters.FirstOrDefault(x => x.ParameterName == "UserName").ToStringValue();

      if (EmployeeRepository.CountDisable() == EmployeeRepository.Get().Count - 1)
        return false;

      var employee = EmployeeRepository.Find(userName);

      if (employee == default)
        return false;

      employee.IsActive = false;

      return EmployeeRepository.Save();
    }
  }
}
