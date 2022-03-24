using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using EcommerceAPI.Models;

namespace EcommerceAPI.Models
{
    public class ProductModel
    {
        [Required]
        public int mId { get; set; }

        [Required]
        public string mProdName { get; set; }
        public string mDescription { get; set; }

        [Required]
        [Range(0, 10000)]
        public Decimal mPrice { get; set; }
        public string mUpdate_Mode { get; set; }
        public DataSet GetDataSet(String spname, SqlParameter[] p)
        {
            string cs = ConfigurationManager.ConnectionStrings["EcommAPIConn"].ToString();

            SqlConnection con = new SqlConnection(cs);

            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                cmd.CommandTimeout = 600;
                cmd.Connection = con;
                cmd.CommandText = spname;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(p);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataTable Get_Data_Table(string spname, SqlParameter[] p)
        {
            string cs = ConfigurationManager.ConnectionStrings["EcommAPIConn"].ToString();

            SqlConnection con = new SqlConnection(cs);

            DataTable tblCity = new DataTable();
            SqlCommand cmd = new SqlCommand(spname, con);
            try
            {
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(p);
                con.Open();
                IDataReader rdr = cmd.ExecuteReader();
                tblCity.Load(rdr);
                con.Close();

            }
            catch (Exception ex)
            {
                con.Close();

            }
            return tblCity;
        }
    }

    public class async
    {
    }

    public class Productlist
    {
        public string mId { get; set; }

        public string mProdName { get; set; }
        public string mDescription { get; set; }

        public string mPrice { get; set; }

        List<ProductModel> lstProductModel = new List<ProductModel>();
    }

}