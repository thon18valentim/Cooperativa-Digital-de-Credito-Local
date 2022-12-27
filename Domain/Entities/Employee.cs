using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities.Enums;

namespace AdaCredit.Domain.Entities
{
  public sealed class Employee
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string LastLogin { get; set; }
    public bool IsActive { get; set; }

    public Employee(string firstName, string lastName, Gender gender, string userName, string passwordHash, string passwordSalt)
    {
      FirstName = firstName;
      LastName = lastName;
      Gender = gender;
      UserName = userName;
      PasswordHash = passwordHash;
      PasswordSalt = passwordSalt;
      LastLogin = DateTime.Now.ToString("dd/MM/yyyy/HH:mm");
      IsActive = true;
    }

    public Employee() { }
  }
}
