namespace FinalProject;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
class DataTier
{
    public string connStr = "server=20.172.0.16;user=hskarr1;database=hskarr1;port=8080;password=hskarr1";


    public bool LoginCheck(User user)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "LoginCount";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputUserID", user.staffID);
            cmd.Parameters.AddWithValue("@inputUserPassword", user.staffPassword);
            cmd.Parameters.Add("@userCount", MySqlDbType.Int32).Direction = ParameterDirection.Output;
            MySqlDataReader rdr = cmd.ExecuteReader();

            int returnCount = (int)cmd.Parameters["@userCount"].Value;
            rdr.Close();
            conn.Close();

            if (returnCount == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return false;
        }
    }

    // Lists all Residents in database, pulls name and unit number
    public DataTable ListResidents(User user)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "ListResidents";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tableResidents = new DataTable();
            tableResidents.Load(rdr);
            rdr.Close();
            conn.Close();
            return tableResidents;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }
    }

    // Lists the unit specified by user and shows all residents in said unit
    public DataTable ListUnit(User user, int unit_number)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "ListUnit";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputunit_number", unit_number);
            cmd.Parameters["@inputunit_number"].Direction = ParameterDirection.Input;
            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tableResidents = new DataTable();
            tableResidents.Load(rdr);
            rdr.Close();
            conn.Close();
            return tableResidents;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }
    }

    //Adds any new packages for known residents to Pending area 
    public void AddPending(int unit_number, string full_name, string posting_name)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "AddPending";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputunit_number", unit_number);
            cmd.Parameters["@inputunit_number"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputfull_name", full_name);
            cmd.Parameters["@inputfull_name"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputposting_name", posting_name);
            cmd.Parameters["@inputposting_name"].Direction = ParameterDirection.Input;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
        }
    }

    // Lists all pending packages for residents to pick up
    public DataTable ListPending(User user)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "ListPending";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tablePendingArea = new DataTable();
            tablePendingArea.Load(rdr);
            rdr.Close();
            conn.Close();
            return tablePendingArea;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }
    }

    // Removes picked up package from pending area
    public void RemovePending(int unit_number, string full_name, string posting_name)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "RemovePending";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputunit_number", unit_number);
            cmd.Parameters["@inputunit_number"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputfull_name", full_name);
            cmd.Parameters["@inputfull_name"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputposting_name", posting_name);
            cmd.Parameters["@inputposting_name"].Direction = ParameterDirection.Input;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
        }
    }

    // Allows for staff to add an unknown package to unknown table
    public void AddUnknown(string unknown_name, string unknown_address, string unknown_posting, string unknown_date)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "AddUnknown";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputunknown_name", unknown_name);
            cmd.Parameters["@inputunknown_name"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputunknown_address", unknown_address);
            cmd.Parameters["@inputunknown_address"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputunknown_posting", unknown_posting);
            cmd.Parameters["@inputunknown_posting"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputunknown_date", unknown_date);
            cmd.Parameters["@inputunknown_date"].Direction = ParameterDirection.Input;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
        }
    }

        // Adds a known resident's package to their package history
        public void AddPackageHistory(int unit_number, string full_name, string posting_name, string delivery_date)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "AddPackageHistory";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputunit_number", unit_number);
            cmd.Parameters["@inputunit_number"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputfull_name", full_name);
            cmd.Parameters["@inputfull_name"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputposting_name", posting_name);
            cmd.Parameters["@inputposting_name"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputdelivery_date", delivery_date);
            cmd.Parameters["@inputdelivery_date"].Direction = ParameterDirection.Input;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
        }
    }

    // Allows staff to show a list of packages delivered to a specific residentLe
    public DataTable ListPackageHistory(User user, int unit_number, string full_name)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
            string procedure = "ListPackageHistory";
            MySqlCommand cmd = new MySqlCommand(procedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inputunit_number", unit_number);
            cmd.Parameters["@inputunit_number"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@inputfull_name", full_name);
            cmd.Parameters["@inputfull_name"].Direction = ParameterDirection.Input;
            MySqlDataReader rdr = cmd.ExecuteReader();

            DataTable tableResidents = new DataTable();
            tableResidents.Load(rdr);
            rdr.Close();
            conn.Close();
            return tableResidents;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            conn.Close();
            return null;
        }
    }
}