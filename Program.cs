
using AdaCredit.Views;
using AdaCredit.Domain.Entities;
using AdaCredit.Utils;
using AdaCredit.Infra.Repositories;

namespace AdaCredit
{
  public static class Program
  {
    static void Main()
    {
      Login.Show();
      Menu.Show();

      //var employee = new Employee("Othon", "othonvv", "21412791@", "124");
      //var employee2 = new Employee("Ana", "ana123", "5125112@", "765");
      //Util.WriteFileInDesktop("adaCredit_employee_database.csv", new List<Employee> { employee, employee2 });

      //Console.WriteLine("| Original |");
      //foreach (Employee e in EmployeeRepository.RegisteredEmployees)
      //  Console.WriteLine(e.FirstName);
      //Console.ReadKey();
      //Console.WriteLine("----");

      //EmployeeRepository.Add();

      //Console.WriteLine("| Nova |");
      //foreach (Employee e in EmployeeRepository.RegisteredEmployees)
      //  Console.WriteLine(e.FirstName);
    }
  }
}
