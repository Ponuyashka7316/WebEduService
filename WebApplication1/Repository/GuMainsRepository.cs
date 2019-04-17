using KostenVoranSchlagConsoleParser.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Repository
{
    public class GuMainsRepository
    {
        OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
        public GuMainsRepository()
        {
            serviceProxy = ConnectHelper.CrmService;
        }
        public IEnumerable<Entity> GetRecords(string entity)
        {
            var service = (IOrganizationService)serviceProxy;

            var query = new QueryExpression(entity)
            {
                Distinct = true,
                ColumnSet = new ColumnSet(true),

            };
            var res = service.RetrieveMultiple(query).Entities;

            return res;
        }
    }
}