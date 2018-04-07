using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;

public partial class Customer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ddlSeller.Visible)
        {
            PrepareForm();
        }
    }

    private void PrepareForm()
    {
        SqlConnection dbConn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Repositorios\TesteStefanini\Database.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand dbComm;
        SqlDataReader dbReader;

        string dbQuery;

        dbConn.Open();

        //Prepare the Gender DropDown
        dbQuery = "select Id, Name from Gender";
        dbComm = new SqlCommand(dbQuery, dbConn);
        dbComm.CommandType = System.Data.CommandType.Text;
        using (dbReader = dbComm.ExecuteReader())
        {
            ddlGender.DataSource = dbReader;
            ddlGender.DataTextField = "Name";
            ddlGender.DataValueField = "Id";
            ddlGender.DataBind();
        }
        ddlGender.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        ddlGender.SelectedIndex = 0;

        //Prepare the City DropDown
        dbQuery = "select Id, Name from City";
        dbComm = new SqlCommand(dbQuery, dbConn);
        dbComm.CommandType = System.Data.CommandType.Text;
        using (dbReader = dbComm.ExecuteReader())
        {
            ddlCity.DataSource = dbReader;
            ddlCity.DataTextField = "Name";
            ddlCity.DataValueField = "Id";
            ddlCity.DataBind();
        }
        ddlCity.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        ddlCity.SelectedIndex = 0;

        //Prepare the Region DropDown
        dbQuery = "select Id, Name from Region";
        dbComm = new SqlCommand(dbQuery, dbConn);
        dbComm.CommandType = System.Data.CommandType.Text;
        using (dbReader = dbComm.ExecuteReader())
        {
            ddlRegion.DataSource = dbReader;
            ddlRegion.DataTextField = "Name";
            ddlRegion.DataValueField = "Id";
            ddlRegion.DataBind();
        }
        ddlRegion.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        ddlRegion.SelectedIndex = 0;

        //Prepare the Classification DropDown
        dbQuery = "select Id, Name from Classification";
        dbComm = new SqlCommand(dbQuery, dbConn);
        dbComm.CommandType = System.Data.CommandType.Text;
        using (dbReader = dbComm.ExecuteReader())
        {
            ddlClass.DataSource = dbReader;
            ddlClass.DataTextField = "Name";
            ddlClass.DataValueField = "Id";
            ddlClass.DataBind();
        }
        ddlClass.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        ddlClass.SelectedIndex = 0;

        if (ddlSeller.Visible)
        {
            //Prepare the User DropDown
            dbQuery = "select Id, Login from UserSys US where UserRoleId in (select Id from UserRole where Name = 'Seller')";
            dbComm = new SqlCommand(dbQuery, dbConn);
            dbComm.CommandType = System.Data.CommandType.Text;
            using (dbReader = dbComm.ExecuteReader())
            {
                ddlSeller.DataSource = dbReader;
                ddlSeller.DataTextField = "Login";
                ddlSeller.DataValueField = "Id";
                ddlSeller.DataBind();
            }
            ddlSeller.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            ddlSeller.SelectedIndex = 0;
        }

        dbConn.Close();
    }

    [System.Web.Services.WebMethod]
    public static string Search(string pAdmin,
                                string pID,
                                string pName,
                                string pGender,
                                string pCity,
                                string pRegion,
                                string pLast,
                                string pUntil,
                                string pClass,
                                string pSeller)
    {
        string Result = "";

        SqlConnection dbConn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Repositorios\TesteStefanini\Database.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand dbComm;
        SqlDataReader dbReader;
        DataSet dbSet;
        DataSet dsCustomers;

        string dbQuery = "";

        dbQuery += "select CLA.Name, ";
        dbQuery += "CUS.Name, ";
        dbQuery += "CUS.Phone, ";
        dbQuery += "GEN.Name, ";
        dbQuery += "CIT.Name, ";
        dbQuery += "REG.Name, ";
        dbQuery += "CUS.LastPurchase, ";
        dbQuery += "USR.Login from Customer CUS ";
        dbQuery += "left join Classification CLA on(CLA.Id = CUS.ClassificationId) ";
        dbQuery += "left join Gender GEN on(GEN.id = CUS.GenderId) ";
        dbQuery += "left join City CIT on(CIT.Id = CUS.CityId) ";
        dbQuery += "left join Region REG on(REG.Id = CUS.RegionId) ";
        dbQuery += "left join UserSys USR on(USR.Id = CUS.UserId) ";

        if(Encoding.UTF8.GetString(Convert.FromBase64String(pAdmin)).ToUpper() == "TRUE")
        {
            dbQuery += "where 1 = 1 ";
        }
        else
        {
            dbQuery += "where CUS.UserId = " + Encoding.UTF8.GetString(Convert.FromBase64String(pID));
        }

        if(Encoding.UTF8.GetString(Convert.FromBase64String(pName)) != "")
        {
            dbQuery += "and CUS.Name like '%" + Encoding.UTF8.GetString(Convert.FromBase64String(pName)) + "%'";
        }

        if (Encoding.UTF8.GetString(Convert.FromBase64String(pGender)) != "")
        {
            dbQuery += "and GEN.Name = '" + Encoding.UTF8.GetString(Convert.FromBase64String(pGender)) + "'";
        }

        if (Encoding.UTF8.GetString(Convert.FromBase64String(pCity)) != "")
        {
            dbQuery += "and CIT.Name = '" + Encoding.UTF8.GetString(Convert.FromBase64String(pCity)) + "'";
        }

        if (Encoding.UTF8.GetString(Convert.FromBase64String(pRegion)) != "")
        {
            dbQuery += "and REG.Name = '" + Encoding.UTF8.GetString(Convert.FromBase64String(pRegion)) + "'";
        }

        if (Encoding.UTF8.GetString(Convert.FromBase64String(pLast)) != "")
        {
            dbQuery += "and CUS.LastPurchase between '" + Encoding.UTF8.GetString(Convert.FromBase64String(pLast)) + "' and '" + Encoding.UTF8.GetString(Convert.FromBase64String(pUntil)) + "'";
        }

        if (Encoding.UTF8.GetString(Convert.FromBase64String(pClass)) != "")
        {
            dbQuery += "and CLA.Name = '" + Encoding.UTF8.GetString(Convert.FromBase64String(pClass)) + "'";
        }

        if (Encoding.UTF8.GetString(Convert.FromBase64String(pSeller)) != "")
        {
            dbQuery += "and CUS.UserId = " + Encoding.UTF8.GetString(Convert.FromBase64String(pSeller));
        }

        dbConn.Open();
        dbComm = new SqlCommand(dbQuery, dbConn);
        dbComm.CommandType = System.Data.CommandType.Text;

        if(Encoding.UTF8.GetString(Convert.FromBase64String(pAdmin)).ToUpper() == "TRUE")
        {
            Result += "<thead class='tablehead'>";
            Result += "<tr>";
            Result += "<td>" + "Classification" + "</td>";
            Result += "<td>" + "Name" + "</td>";
            Result += "<td>" + "Phone" + "</td>";
            Result += "<td>" + "Gender" + "</td>";
            Result += "<td>" + "City" + "</td>";
            Result += "<td>" + "Region" + "</td>";
            Result += "<td>" + "Last Purchase" + "</td>";
            Result += "<td>" + "Seller" + "</td>";
            Result += "</tr>";
            Result += "</thead>";
        }
        else
        {
            Result += "<thead class='tablehead'>";
            Result += "<tr>";
            Result += "<td>" + "Classification" + "</td>";
            Result += "<td>" + "Name" + "</td>";
            Result += "<td>" + "Phone" + "</td>";
            Result += "<td>" + "Gender" + "</td>";
            Result += "<td>" + "City" + "</td>";
            Result += "<td>" + "Region" + "</td>";
            Result += "<td>" + "Last Purchase" + "</td>";
            Result += "</tr>";
            Result += "</thead>";
        }

        Result += "<tbody>";
        using (dbReader = dbComm.ExecuteReader())
        {
            if (dbReader.HasRows)
            {
                while (dbReader.Read())
                {
                    Result += "<tr>";
                    Result += "<td class='cellborder'>" + dbReader.GetString(0) + "</td>";
                    Result += "<td class='cellborder'>" + dbReader.GetString(1) + "</td>";
                    Result += "<td class='cellborder'>" + dbReader.GetString(2) + "</td>";
                    Result += "<td class='cellborder'>" + dbReader.GetString(3) + "</td>";
                    Result += "<td class='cellborder'>" + dbReader.GetString(4) + "</td>";
                    Result += "<td class='cellborder'>" + dbReader.GetString(5) + "</td>";
                    Result += "<td class='cellborder'>" + dbReader.GetDateTime(6).ToShortDateString() + "</td>";

                    if (Encoding.UTF8.GetString(Convert.FromBase64String(pAdmin)).ToUpper() == "TRUE")
                    {
                        Result += "<td class='cellborder'>" + dbReader.GetString(7) + "</td>";
                    }

                    Result += "</tr>";
                }
            }
        }
        Result += "</tbody>";

        dbConn.Close();

        return Result;
    }

    [System.Web.Services.WebMethod]
    public static string SetRegion(string pCity)
    {
        SqlConnection dbConn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Repositorios\TesteStefanini\Database.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand dbComm;
        SqlDataReader dbReader;

        string dbQuery;

        dbConn.Open();

        string Result = "";

        if (pCity != "")
        {
            //Prepare the Region DropDown
            dbQuery = "select REG.Id, REG.Name from City CIT left join Region REG on (REG.Id = CIT.RegionId) where CIT.Name = '" + Encoding.UTF8.GetString(Convert.FromBase64String(pCity)) + "'";
            dbComm = new SqlCommand(dbQuery, dbConn);
            dbComm.CommandType = System.Data.CommandType.Text;

            Result += "" + ":";
            Result += "" + ";";

            using (dbReader = dbComm.ExecuteReader())
            {
                if (dbReader.HasRows)
                {
                    while (dbReader.Read())
                    {
                        Result += dbReader.GetInt32(0).ToString() + ":";
                        Result += dbReader.GetString(1).ToString() + ";";
                    }
                }
            }
        }
        else
        {
            //Prepare the Region DropDown
            dbQuery = "select Id, Name from Region";
            dbComm = new SqlCommand(dbQuery, dbConn);
            dbComm.CommandType = System.Data.CommandType.Text;

            Result += "" + ":";
            Result += "" + ";";

            using (dbReader = dbComm.ExecuteReader())
            {
                if (dbReader.HasRows)
                {
                    while (dbReader.Read())
                    {
                        Result += dbReader.GetInt32(0).ToString() + ":";
                        Result += dbReader.GetString(1).ToString() + ";";
                    }
                }
            }
        }

        dbConn.Close();

        return Result;
    }
}