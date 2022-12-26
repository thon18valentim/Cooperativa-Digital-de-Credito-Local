using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Domain.Entities.Maps
{
  public class EmployeeMap : ClassMap<Employee>
  {
    public EmployeeMap()
    {
      Map(m => m.FirstName).Name("name");
      Map(m => m.UserName).Name("login");
      Map(m => m.PasswordHash).Name("passwordHash");
      Map(m => m.PasswordSalt).Name("passwordSalt");
      Map(m => m.LastLogin).Name("lastLogin");
      Map(m => m.IsActive).Name("active");
    }
  }
}
