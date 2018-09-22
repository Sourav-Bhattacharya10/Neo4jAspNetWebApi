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
    public class StudentController : ApiController
    {
        string ServerUri = ConfigurationManager.AppSettings["ServerUri"];
        string Username = ConfigurationManager.AppSettings["Username"];
        string Password = ConfigurationManager.AppSettings["Password"];
        Neo4jStudentApiService service;

        // GET: api/Student
        //Get all the nodes
        public HttpResponseMessage Get()
        {
            service = new Neo4jStudentApiService(ServerUri, Username, Password);
            var StudentList = service.GetAllStudents();
            return Request.CreateResponse(HttpStatusCode.OK, StudentList);
        }

        // GET: api/Student?name=Alice
        //Get specific node
        public HttpResponseMessage Get(string name)
        {
            service = new Neo4jStudentApiService(ServerUri, Username, Password);
            var Student = service.GetStudent(name);
            return Request.CreateResponse(HttpStatusCode.OK, Student);
        }

        // POST: api/Student
        //BODY: {"name" : "Danny","marks" : 75,"favoritesubject" : "Science"}
        //Create a node
        public HttpResponseMessage Post([FromBody]Student std)
        {
            service = new Neo4jStudentApiService(ServerUri, Username, Password);
            var result = service.CreateStudent(std);
        
            if(result)
                return Request.CreateResponse(HttpStatusCode.Created, "Inserted");
            else
                return Request.CreateResponse(HttpStatusCode.OK, "Failed");
        }

        // PUT: api/Student
        //BODY: {"name" : "Danny","marks" : 40,"favoritesubject" : "Geography"}
        //Update a specific node
        public HttpResponseMessage Put([FromBody]Student std)
        {
            service = new Neo4jStudentApiService(ServerUri, Username, Password);
            var result = service.UpdateStudent(std);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE: api/Student?name=Danny
        //Detach all relationship and then delete the node
        public HttpResponseMessage Delete(string name)
        {
            service = new Neo4jStudentApiService(ServerUri, Username, Password);
            var result = service.DeleteStudent(name);

            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
            else
                return Request.CreateResponse(HttpStatusCode.OK, "Failed");
        }
    }
}
