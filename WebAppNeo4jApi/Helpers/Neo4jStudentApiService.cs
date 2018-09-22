using Neo4j.Driver.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppNeo4jApi.Models;

namespace WebAppNeo4jApi.Helpers
{
    public class Neo4jStudentApiService : IDisposable
    {
        IDriver driver;

        public Neo4jStudentApiService(string serveruri, string username, string password)
        {
            driver = GraphDatabase.Driver(serveruri, AuthTokens.Basic(username, password));
        }

        //Create a node
        public bool CreateStudent(Student std)
        {
            bool result = false;

            try
            {
                using (var session = driver.Session())
                {
                    var student = session.WriteTransaction(tx =>
                    {
                        var res = tx.Run($"CREATE (a:Student {{ name : \"{std.name}\", marks : {std.marks}, favoritesubject : \"{std.favoritesubject}\" }}) RETURN a");
                        return res;
                    });

                    if (student.Summary.Counters.NodesCreated == 1)
                        result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        //Get all the nodes
        public List<Student> GetAllStudents()
        {
            List<Student> StudentList = null;

            try
            {
                using (var session = driver.Session())
                {
                    StudentList = new List<Student>();

                    var students = session.ReadTransaction(tx =>
                    {
                        var res = tx.Run($"MATCH (n:Student) RETURN n");
                        return res;
                    });

                    foreach (var record in students)
                    {
                        var nodeProps = JsonConvert.SerializeObject(record[0].As<INode>().Properties);
                        var std = JsonConvert.DeserializeObject<Student>(nodeProps);
                        StudentList.Add(std);
                    }
                }
            }
            catch (Exception ex)
            {
                StudentList = null;
            }

            return StudentList;
        }

        //Get specific node
        public Student GetStudent(string name)
        {
            Student result = null;

            try
            {
                using (var session = driver.Session())
                {
                    result = new Student();

                    var student = session.ReadTransaction(tx =>
                    {
                        var res = tx.Run($"MATCH (n:Student) WHERE n.name = \"{name}\" RETURN n");
                        return res;
                    });

                    
                    var nodeProps = JsonConvert.SerializeObject(student.Single()[0].As<INode>().Properties);
                    result = JsonConvert.DeserializeObject<Student>(nodeProps);
                    
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        //Update a specific node
        public Student UpdateStudent(Student std)
        {
            Student result = null;

            try
            {
                using (var session = driver.Session())
                {
                    result = new Student();

                    var student = session.WriteTransaction(tx =>
                    {
                        var res = tx.Run($"MATCH (n:Student) WHERE n.name = \"{std.name}\" SET n.marks = {std.marks}, n.favoritesubject = \"{std.favoritesubject}\" RETURN n");
                        return res;
                    });


                    var nodeProps = JsonConvert.SerializeObject(student.Single()[0].As<INode>().Properties);
                    result = JsonConvert.DeserializeObject<Student>(nodeProps);

                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        //Detach all relationship and then delete the node
        public bool DeleteStudent(string name)
        {
            bool result = false;

            try
            {
                using (var session = driver.Session())
                {
                    var student = session.WriteTransaction(tx =>
                    {
                        var res = tx.Run($"MATCH (n:Student) WHERE n.name = \"{name}\" DETACH DELETE n");
                        return res;
                    });

                    if (student.Summary.Counters.NodesDeleted == 1)
                        result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    driver?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Neo4jApiService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}