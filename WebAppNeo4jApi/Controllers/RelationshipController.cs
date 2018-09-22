using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppNeo4jApi.Helpers;
using WebAppNeo4jApi.Models;

namespace WebAppNeo4jApi.Controllers
{
    public class RelationshipController : ApiController
    {
        string ServerUri = ConfigurationManager.AppSettings["ServerUri"];
        string Username = ConfigurationManager.AppSettings["Username"];
        string Password = ConfigurationManager.AppSettings["Password"];
        Neo4jRelationshipApiService service;

        // GET: api/Relationship
        //Get all the relationship
        public HttpResponseMessage Get()
        {
            service = new Neo4jRelationshipApiService(ServerUri, Username, Password);
            var RelationshipList = service.GetAllRelationships();
            return Request.CreateResponse(HttpStatusCode.OK, RelationshipList);
        }

        // GET: api/Relationship?type=Classmate
        //Get specific relationship
        public HttpResponseMessage Get(string type)
        {
            service = new Neo4jRelationshipApiService(ServerUri, Username, Password);
            var RelationshipList = service.GetRelationships(type);
            return Request.CreateResponse(HttpStatusCode.OK, RelationshipList);
        }

        // POST: api/Relationship
        //BODY: {"From" : "Bob","To" : "Charlie","Type" : "Classmate"}
        //Create a relationship
        public HttpResponseMessage Post([FromBody]CustomRelationship r)
        {
            service = new Neo4jRelationshipApiService(ServerUri, Username, Password);
            var result = service.CreateRelationship(r);

            if (result)
                return Request.CreateResponse(HttpStatusCode.Created, "Created");
            else
                return Request.CreateResponse(HttpStatusCode.OK, "Failed");
        }

        // PUT: api/Relationship?oldtype=Classmate&newtype=Knows
        //Update a specific relationship
        public HttpResponseMessage Put(string oldtype, string newtype)
        {
            service = new Neo4jRelationshipApiService(ServerUri, Username, Password);
            var result = service.UpdateRelationships(oldtype,newtype);

            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated");
            else
                return Request.CreateResponse(HttpStatusCode.OK, "Failed");
        }

        // DELETE: api/Relationship?type=Classmate
        //Delete relationship
        public HttpResponseMessage Delete(string type)
        {
            service = new Neo4jRelationshipApiService(ServerUri, Username, Password);
            var result = service.DeleteRelationships(type);

            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
            else
                return Request.CreateResponse(HttpStatusCode.OK, "Failed");
        }
    }
}
