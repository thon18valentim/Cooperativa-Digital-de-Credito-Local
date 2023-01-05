using AdaCredit.Domain.Entities.Enums;

namespace AdaCredit.Domain.Entities
{
  public sealed class Client
  {
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public string Cpf { get; set; }
    public string Phone { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string StreetAddress { get; set; }
    public int ClientAccountId { get; set; }
    public bool IsActive { get; set; }

    public Client() { }
  }
}
