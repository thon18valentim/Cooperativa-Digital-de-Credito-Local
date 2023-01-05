using CsvHelper.Configuration;

namespace AdaCredit.Domain.Entities.Maps
{
  public class ClientMap : ClassMap<Client>
  {
    public ClientMap()
    {
      Map(m => m.Name).Name("name");
      Map(m => m.Gender).Name("gender");
      Map(m => m.Cpf).Name("cpf");
      Map(m => m.Phone).Name("phone");
      Map(m => m.Country).Name("country");
      Map(m => m.City).Name("city");
      Map(m => m.StreetAddress).Name("streetAdress");
      Map(m => m.ClientAccountId).Name("accountId");
      Map(m => m.IsActive).Name("active");
    }
  }
}
