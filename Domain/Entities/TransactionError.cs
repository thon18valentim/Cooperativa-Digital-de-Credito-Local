
namespace AdaCredit.Domain.Entities
{
  public sealed class TransactionError
  {
    public string Message { get; set; }
    public DateTime Date { get; set; }

    public TransactionError() { }
  }
}
