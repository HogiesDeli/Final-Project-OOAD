namespace FinalProject;
using System.Data;
using MySql.Data.MySqlClient;
class GuiTier
{
    User user = new User();
    DataTier database = new DataTier();

    public User Login()
    {
        Console.WriteLine("------Welcome to Amarillo Apt Package Management------");
        Console.WriteLine("Please input username: ");
        user.staffID = Console.ReadLine();
        Console.WriteLine("Please input password: ");
        user.staffPassword = Console.ReadLine();
        return user;
    }

    public int Dashboard(User user)
    {
        DateTime localDate = DateTime.Now;
        Console.WriteLine("---------------Dashboard-------------------");
        Console.WriteLine($"Hello: {user.staffID}; Date/Time: {localDate.ToString()}");
        Console.WriteLine("Please select an option to continue:");
        Console.WriteLine("1. Add Known Delivered Package");
        Console.WriteLine("2. Add Unknown Delivered Package");
        Console.WriteLine("3. Remove Package from Pending Area");
        Console.WriteLine("4. Show Package History for Resident");
        Console.WriteLine("5. Log Out");
        int option = Convert.ToInt16(Console.ReadLine());
        return option;
    }

    public void DisplayUnitResidents(DataTable tableResidents)
    {
        int index = 0;
        Console.WriteLine("---------------Unit Residents-------------------");
        foreach (DataRow row in tableResidents.Rows)
        {
            Console.WriteLine($"{index} - Unit Number: {row["unit_number"]} \t Resident Name: {row["full_name"]} \t Email: {row["email"]}");
            index++;
        }
        
    }

    public void DisplayPending(DataTable tablePendingArea)
    {
        int index = 0;
        Console.WriteLine("---------------Pending Packages-------------------");
        foreach (DataRow row in tablePendingArea.Rows)
        {
            Console.WriteLine($"{index} - Unit Number: {row["unit_number"]} \t Resident Name: {row["full_name"]} \t Posting Service: {row["posting_name"]}");
            index++;
        }
    }

    public void DisplayPackageHistory(DataTable tablePackageHistory)
    {
        Console.WriteLine("---------------Package History-------------------");
        foreach (DataRow row in tablePackageHistory.Rows)
        {
            Console.WriteLine($"Unit Number: {row["unit_number"]} \t Resident Name: {row["full_name"]} \t Posting Service: {row["posting_name"]} \t Date: {row["delivery_date"]}");
        }
    }
}