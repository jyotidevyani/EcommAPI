using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EcommerceAPI.App_Start;
using EcommerceAPI.Contract;
using EcommerceAPI.Models;
using EcommerceAPI.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EcommerceAPI.Controllers
{
    [RoutePrefix("api/Product")]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]

    public class ProductController : ApiController
    {
        private readonly iProductService _iProductService;

        public ProductController(iProductService _ProductService)
        {
            _iProductService = _ProductService;
        }
        
        [Route("getproducts")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProducts()
        {
            {
                try
                {
                    IEnumerable<Productlist> prlist = await _iProductService.GetProducts();
                    if (prlist.Count() > 0)
                    {
                        return Ok(prlist);
                    }
                    else
                    {
                        return Ok("Data Not Found");
                    }
                }
                catch (Exception ex)
                {
                    return Ok("Error while processing");
                }
            }
        }
       
        [Route("editproduct")]
        [HttpPost]
        public async Task<IHttpActionResult> EditProduct([FromBody] JObject jsonStr)
        {
            {
                try
                {
                    dynamic jsonData = JsonConvert.DeserializeObject(jsonStr.ToString());
                    string mId = jsonData.mId;

                    IEnumerable<Productlist> prlist = await _iProductService.EditProduct(mId);

                    if (prlist.Count() > 0)
                    {
                        return Ok(prlist);
                    }
                    else
                    {
                        return Ok("Data Not Found");
                    }
                }
                catch (Exception ex)
                {
                    return Ok("Data Not Found");
                }
            }
        }

        [Route("deleteproduct")]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteProduct([FromBody] JObject jsonStr)
        {
            {
                try
                {
                    dynamic jsonData = JsonConvert.DeserializeObject(jsonStr.ToString());
                    string mId = jsonData.mId;

                    string moutputresult = await _iProductService.DeleteProduct(mId);
                    if (moutputresult == "SUCCESS")
                    {
                        return Ok(moutputresult);
                    }
                    else
                    {
                        return Ok("Data not found");
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(new Exception("Error while processing"));
                }
            }
        }

        [Route("addproduct")]
        [HttpPost]
        public async Task<IHttpActionResult> AddProduct([FromBody] JObject jsonStr)
        {
                // This api will get called while saving new or edited product
                try
                {
                    ProductModel _ProductModel = new ProductModel(); 
                    dynamic jsonData = JsonConvert.DeserializeObject(jsonStr.ToString());
                    _ProductModel.mId = jsonData.mId;
                    _ProductModel.mProdName = jsonData.mProdName;
                    _ProductModel.mDescription = jsonData.mDescription;
                    _ProductModel.mPrice = jsonData.mPrice;
                    _ProductModel.mUpdate_Mode = jsonData.mUpdate_Mode;


                // Checking for duplicate product name

                string mresponse = await _iProductService.ChkDuplicate(_ProductModel.mId, "1", "ProdName", _ProductModel.mProdName, "Product");
                if (mresponse == "Notduplicate")
                {

                    // Checking for duplicate product name

                    // If productname not duplicate will add new product

                    string moutputresult = await _iProductService.AddProduct(_ProductModel);
                    if (moutputresult == "SUCCESS")
                    {
                        return Ok(moutputresult);
                    }
                    else
                    {
                        //UPDATEID = _clsactivityLog.UpdateAPIresponseLog(requestid, "SUCCESS", "Data Not Found");
                        return Ok("Data not found");
                    }
                }
                else
                {
                    return Ok("Duplicate Product");
                }
                }
                catch (Exception ex)
                {
                    return Ok("Error while processing");
                }
        }
    }

    internal class EnableCorsAttribute : Attribute
    {
        private string origins;
        private string headers;
        private string methods;
        private string exposedHeaders;

        public EnableCorsAttribute(string origins, string headers, string methods, string exposedHeaders)
        {
            this.origins = origins;
            this.headers = headers;
            this.methods = methods;
            this.exposedHeaders = exposedHeaders;
        }
    }
}