using System;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EcommerceAPI.Contract;
using EcommerceAPI.Models;
using System.Net.Security;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace EcommerceAPI.Repository
{
    public class clsProduct : iProductService
    {
        string ECommProdURL = ConfigurationManager.AppSettings["ECommProdURL"].ToString();
        string EcommAPIConn = ConfigurationManager.ConnectionStrings["EcommAPIConn"].ToString();

        public async Task<string> AddProduct(ProductModel _ProductModel)
        {
            string mOutputResult = "";
            int mcol = 0;
            SqlParameter[] p = new SqlParameter[5];

            if (_ProductModel.mUpdate_Mode == "A")
            {
                p[mcol] = new SqlParameter("@eId", 1);
            }
            else
            {
                p[mcol] = new SqlParameter("@eId", _ProductModel.mId);
            }

            mcol = mcol + 1;
            p[mcol] = new SqlParameter("@eProdName", _ProductModel.mProdName);
            mcol = mcol + 1;
            p[mcol] = new SqlParameter("@eDescription", _ProductModel.mDescription);
            mcol = mcol + 1;
            p[mcol] = new SqlParameter("@ePrice", Convert.ToDecimal(_ProductModel.mPrice));
            mcol = mcol + 1;
            p[mcol] = new SqlParameter("@eUpdate_Mode", _ProductModel.mUpdate_Mode);

            DataSet dsOutputResult = _ProductModel.GetDataSet("SP_InsertUpdateProduct", p);
            mOutputResult = dsOutputResult.Tables[0].Rows[0][0].ToString();

            return await Task.FromResult(mOutputResult);

        }

        public async Task<IEnumerable<Productlist>> GetProducts()
        {
            DataTable dt = new DataTable();

            ProductModel prd = new ProductModel();
            List<Productlist> prdlist = new List<Productlist>();

            SqlParameter[] p = new System.Data.SqlClient.SqlParameter[1];
            p[0] = new SqlParameter("@eCh", "ALL");
            dt =  prd.Get_Data_Table("SP_GetProducts", p);
            IEnumerable<Productlist> lcomm;
            lcomm =  (from DataRow dr in dt.Rows
                     select new Productlist()
                     {
                         mId = dr["mId"].ToString(),
                         mProdName = dr["mProdName"].ToString(),
                         mDescription = dr["mDescription"].ToString(),
                         mPrice = dr["mPrice"].ToString(),
                     }).ToList();
            return await Task.FromResult(lcomm);

        }

        public async Task<IEnumerable<Productlist>> EditProduct(string mId)
        {
            DataTable dt = new DataTable();
            ProductModel prd = new ProductModel();
            SqlParameter[] p = new System.Data.SqlClient.SqlParameter[1];
            p[0] = new SqlParameter("@eId", mId);
            dt = prd.Get_Data_Table("SP_EditProduct", p);
            IEnumerable<Productlist> lcomm;
            lcomm = (from DataRow dr in dt.Rows
                     select new Productlist()
                     {
                         mId = dr["mId"].ToString(),
                         mProdName = dr["mProdName"].ToString(),
                         mDescription = dr["mDescription"].ToString(),
                         mPrice = dr["mPrice"].ToString(),
                     }).ToList();
            return await Task.FromResult(lcomm);

        }
        public async Task<string> DeleteProduct(string mId)
        {
            DataTable dt = new DataTable();
            ProductModel prd = new ProductModel();

            string mOutputResult = "";
            SqlParameter[] p = new SqlParameter[1];
            p[0] = new SqlParameter("@eId", mId);
            DataSet dsOutputResult = prd.GetDataSet("SP_DeleteProduct", p);
            mOutputResult = dsOutputResult.Tables[0].Rows[0][0].ToString();
            return await Task.FromResult(mOutputResult);
        }

        public async Task<string> ChkDuplicate(int mId, string mCho, string mFieldName, string mval, string mTableName)
        {

            string mOutputResult = "";
            string response = "";
            int mcol = 0;
            SqlParameter[] p = new SqlParameter[5];

            p[mcol] = new SqlParameter("@eId", Convert.ToInt32(mId));
            mcol = mcol + 1;
            p[mcol] = new SqlParameter("@eCho", mCho);
            mcol = mcol + 1;
            p[mcol] = new SqlParameter("@eFieldName", mFieldName);
            mcol = mcol + 1;
            p[mcol] = new SqlParameter("@eval", mval);
            mcol = mcol + 1;
            p[mcol] = new SqlParameter("@eTableName", mTableName);

            ProductModel prd = new ProductModel();
            DataSet dsOutputResult = prd.GetDataSet("SP_ChkDuplicate", p);
            mOutputResult = dsOutputResult.Tables[0].Rows[0][0].ToString();

            if (Convert.ToInt32(mOutputResult) > 0)
                response = "Duplicate";
            else
                response = "Notduplicate";

            return await Task.FromResult(response);

        }
        private RemoteCertificateValidationCallback RemoveCertificateValidationCallback(Func<bool> p)
        {
            throw new NotImplementedException();
        }


    }
}