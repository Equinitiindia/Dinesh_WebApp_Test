using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class TransactionController : ApiController
    {

        string JSonFile = HttpContext.Current.Server.MapPath("~/result.json");
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            var json = File.ReadAllText(JSonFile);
            var jObject = JObject.Parse(json);
            return Request.CreateResponse(HttpStatusCode.OK, jObject);
        }

        [HttpPost]
        public HttpResponseMessage Add(Transaction t)
        {
            try
            {
                var json = File.ReadAllText(JSonFile);
                var jsonObj = JObject.Parse(json);
                var resArrary = jsonObj.GetValue("res") as JArray;
                int ID = 1;

                if (resArrary.Count > 0)
                    ID = resArrary.Max(obj => obj["Id"].Value<int>()) + 1;
                var newTransMember = "{ 'Id': " + ID + ", 'ApplicationId': " + t.ApplicationId + ",'Type':'" + t.Type + "','Summary':'" + t.Summary + "','Amount':" + t.Amount + ",'PostingDate':'" + t.PostingDate + "','IsCleared':'" + t.IsCleared + "','ClearedDate':'" + t.ClearedDate + "' }";

                var newTrans = JObject.Parse(newTransMember);
                resArrary.Add(newTrans);

                jsonObj["res"] = resArrary;
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                       Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(JSonFile, newJsonResult);
                return Request.CreateResponse(HttpStatusCode.OK, "Created Successfully");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }

        }

        [HttpPut]
        public HttpResponseMessage Update(Transaction t)
        {
            try
            {
                var json = File.ReadAllText(JSonFile);
                var jObject = JObject.Parse(json);
                JArray resArrary = (JArray)jObject["res"];

                if (t.Id > 0)
                {
                    foreach (var tran in resArrary.Where(obj => obj["Id"].Value<int>() == t.Id))
                    {
                        tran["ApplicationId"] = t.ApplicationId;
                        tran["PostingDate"] = !string.IsNullOrEmpty(t.PostingDate) ? t.PostingDate : "";
                        tran["Type"] = !string.IsNullOrEmpty(t.Type) ? t.Type : "";
                        tran["Summary"] = !string.IsNullOrEmpty(t.Summary) ? t.Summary : "";
                        tran["ClearedDate"] = !string.IsNullOrEmpty(t.ClearedDate) ? t.ClearedDate : "";
                        tran["Amount"] = t.Amount;
                        tran["IsCleared"] = t.IsCleared;

                    }

                    jObject["res"] = resArrary;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(JSonFile, output);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpDelete]

        public HttpResponseMessage Delete(Transaction t)
        {
            try
            {
                var json = File.ReadAllText(JSonFile);
                var jObject = JObject.Parse(json);
                JArray resArrary = (JArray)jObject["res"];

                if (t.Id > 0)
                {

                    var tranToDeleted = resArrary.FirstOrDefault(obj => obj["Id"].Value<int>() == t.Id);

                    resArrary.Remove(tranToDeleted);

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(JSonFile, output);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfully");
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }

        }
    }
}
