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

    public Employee() { }
  }
}
