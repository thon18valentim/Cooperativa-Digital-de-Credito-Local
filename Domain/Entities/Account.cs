
namespace AdaCredit.Domain.Entities
{
  public sealed class Account
  {
    public int Id { get; set; }
    public string Number { get; set; }
    public string AgencyNumber { get; set; }
    public decimal Balance { get; set; }

    public Account(string number)
    {
      Number = number;
      AgencyNumber = "0001";
      Balance = 0;
    }

    public Account() { }
  }
}
