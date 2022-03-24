using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceAPI.Models;
using System.Net.Http;

namespace EcommerceAPI.Contract
{
  public  interface iProductService
    {
        Task<IEnumerable<Productlist>> GetProducts();
        Task<string> AddProduct(ProductModel _ProductModel);
        Task<IEnumerable<Productlist>> EditProduct(string mId);
        Task<string> DeleteProduct(string mId);
        Task<string> ChkDuplicate(int mId, string mCho, string mFieldName, string mval, string mTableName);
    }
}
