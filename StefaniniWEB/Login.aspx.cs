using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Web.SessionState;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [System.Web.Services.WebMethod]
    public static string LogOn(string pEmail, string pPassword)
    {
        SqlConnection dbConn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Repositorios\TesteStefanini\Database.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand dbComm;
        SqlDataReader dbReader;
        String Result = "OK";

        string decEmail = Encoding.UTF8.GetString(Convert.FromBase64String(pEmail));
        string decPassword = Encoding.UTF8.GetString(Convert.FromBase64String(pPassword));
        string dbQuery = "select US.Id, UR.IsAdmin from UserSys US left join UserRole UR on(US.UserRoleId = UR.Id) where Email = '" + decEmail + "' and Password = '" + decPassword + "'";

        dbConn.Open();
        dbComm = new SqlCommand(dbQuery, dbConn);
        dbComm.CommandType = System.Data.CommandType.Text;
        dbReader = dbComm.ExecuteReader();

        if (!dbReader.HasRows)
        {
            Result = @"The email and/or password entered is invalid. Please try again.";
        }
        else
        {
            dbReader.Read();
            Result = dbReader.GetInt32(0).ToString() + ":" + dbReader.GetBoolean(1).ToString();
        }

        dbConn.Close();

        return Result;
    }
}