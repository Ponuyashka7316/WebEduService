using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using KostenVoranSchlagConsoleParser.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        string entityName = null;
        public ActionResult Index(Guid? Id)
        {
            if (Id.HasValue)
            {
                Guid entityGuid = Id.Value;
                OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
                var service = (IOrganizationService)serviceProxy;
                try
                {
                    entityName = "invoice";
                    Entity mainEntity = service.Retrieve(entityName, entityGuid, new ColumnSet(true));
                    ViewBag.name = mainEntity.GetAttributeValue<string>("name");
                    ViewBag.Decimal = (mainEntity.GetAttributeValue<Money>("totalamount").Value).ToString();
                }
                catch (Exception e)
                {
                    ViewBag.Message = "There is no MainEntity with ID:" + Id;
                }
            }
            else
            {
                ViewBag.Message = "Enter MainEntity guid as parameter";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Attachment()
        {
            entityName = "myprefix_gu_main";
            OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
            var service = (IOrganizationService)serviceProxy;
            GuMainsRepository rep = new GuMainsRepository();
            var records = rep.GetRecords(entityName);
            List<entityModel> model = new List<entityModel>();
            entityModel temp = new entityModel { Id = new Guid(), name = "name", IsChecked = false };
            {
                foreach (var item in records)
                {
                    temp = new entityModel { Id = item.Id, name = item.Attributes["myprefix_name"].ToString(), IsChecked = false };
                    if (temp != null)
                        model.Add(temp);
                }
            }
            return View(model);
        }

        [HttpPost]
        public void Attachment(IEnumerable<string> checkboxList, HttpPostedFileBase upload)
        {
            OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
            var service = (IOrganizationService)serviceProxy;

            System.IO.Stream str = upload.InputStream;
            Int32 strLen, strRead;
            str = upload.InputStream;
            strLen = Convert.ToInt32(str.Length);
            byte[] strArr = new byte[strLen];
            strRead = str.Read(strArr, 0, strLen);
            string encodedData = Convert.ToBase64String(strArr);

            if (checkboxList != null && upload != null)
            {
                foreach (var item in checkboxList)
                {
                    var guid = Guid.Parse(item);
                    Entity annotation = new Entity();
                    annotation.LogicalName = "annotation"; //logical name of target entity 
                    annotation["filename"] = upload.FileName.ToString();
                    annotation["subject"] = "TEST";
                    annotation["objectid"] = new EntityReference("myprefix_gu_main", guid);
                    annotation["documentbody"] = encodedData;
                    annotation["isdocument"] = true;
                    annotation["mimetype"] = "text/plain";
                    service.Create(annotation);
                }
                RedirectToAction("About");
            }
            else
            {
                ViewData["Message"] = "You didn't select anything.";
            }

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}