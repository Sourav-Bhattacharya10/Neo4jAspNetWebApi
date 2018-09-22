using Neo4j.Driver.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppNeo4jApi.Models;

namespace WebAppNeo4jApi.Helpers
{
    public class Neo4jRelationshipApiService : IDisposable
    {
        IDriver driver;

        public Neo4jRelationshipApiService(string serveruri, string username, string password)
        {
            driver = GraphDatabase.Driver(serveruri, AuthTokens.Basic(username, password));
        }

        //Create a relationship
        public bool CreateRelationship(CustomRelationship r)
        {
            bool result = false;

            try
            {
                using (var session = driver.Session())
                {
                    var relation = session.WriteTransaction(tx =>
                    {
                        var res = tx.Run($"MATCH (n:Student {{name: \"{r.From}\"}}),(m:Student {{name : \"{r.To}\"}}) WITH n,m CREATE (n)-[:RELATIONSHIP {{ type : \"{r.Type}\"}}]->(m) RETURN n,m");
                        return res;
                    });

                    if (relation.Summary.Counters.RelationshipsCreated == 1)
                        result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        //Get all the relationship
        public List<CustomRelationship> GetAllRelationships()
        {
            List<CustomRelationship> RelationshipList = null;

            try
            {
                using (var session = driver.Session())
                {
                    RelationshipList = new List<CustomRelationship>();

                    var relations = session.ReadTransaction(tx =>
                    {
                        var res = tx.Run($"MATCH p=()-[r:RELATIONSHIP]->() RETURN p");
                        return res;
                    });

                    foreach (var record in relations)
                    {
                        var relationProps = JsonConvert.SerializeObject(record[0].As<IPath>().Relationships[0].Properties);
                        var rl = JsonConvert.DeserializeObject<Relationship>(relationProps);

                        var startnodeProps = JsonConvert.SerializeObject(record[0].As<IPath>().Start.Properties);
                        var startnode = JsonConvert.DeserializeObject<Student>(startnodeProps);

                        var endnodeProps = JsonConvert.SerializeObject(record[0].As<IPath>().End.Properties);
                        var endnode = JsonConvert.DeserializeObject<Student>(endnodeProps);

                        CustomRelationship cr = new CustomRelationship();
                        cr.Type = rl.Type;
                        cr.From = startnode.name;
                        cr.To = endnode.name;

                        RelationshipList.Add(cr);
                    }
                }
            }
            catch (Exception ex)
            {
                RelationshipList = null;
            }

            return RelationshipList;
        }

        //Get specific relationship
        public List<CustomRelationship> GetRelationships(string type)
        {
            List<CustomRelationship> RelationshipList = null;

            try
            {
                using (var session = driver.Session())
                {
                    RelationshipList = new List<CustomRelationship>();

                    var relations = session.ReadTransaction(tx =>
                    {
                        var res = tx.Run($"MATCH p=()-[r:RELATIONSHIP]->() WHERE r.type = \"{type}\" RETURN p");
                        return res;
                    });

                    foreach (var record in relations)
                    {
                        var relationProps = JsonConvert.SerializeObject(record[0].As<IPath>().Relationships[0].Properties);
                        var rl = JsonConvert.DeserializeObject<Relationship>(relationProps);

                        var startnodeProps = JsonConvert.SerializeObject(record[0].As<IPath>().Start.Properties);
                        var startnode = JsonConvert.DeserializeObject<Student>(startnodeProps);

                        var endnodeProps = JsonConvert.SerializeObject(record[0].As<IPath>().End.Properties);
                        var endnode = JsonConvert.DeserializeObject<Student>(endnodeProps);

                        CustomRelationship cr = new CustomRelationship();
                        cr.Type = rl.Type;
                        cr.From = startnode.name;
                        cr.To = endnode.name;

                        RelationshipList.Add(cr);
                    }
                }
            }
            catch (Exception ex)
            {
                RelationshipList = null;
            }

            return RelationshipList;
        }

        //Update a specific relationship
        public bool UpdateRelationships(string oldtype, string newtype)
        {
            bool result = false;

            try
            {
                using (var session = driver.Session())
                {
                    var relation = session.WriteTransaction(tx =>
                    {
                        var res = tx.Run($"MATCH p=()-[r:RELATIONSHIP]->() WHERE r.type = \"{oldtype}\" SET r.type = \"{newtype}\" RETURN p");
                        return res;
                    });

                    if (relation.Summary.Counters.PropertiesSet > 0)
                        result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        //Delete relationship
        public bool DeleteRelationships(string type)
        {
            bool result = false;

            try
            {
                using (var session = driver.Session())
                {
                    var relation = session.WriteTransaction(tx =>
                    {
                        var res = tx.Run($"MATCH p=()-[r:RELATIONSHIP]->() WHERE r.type = \"{type}\" DELETE r");
                        return res;
                    });

                    if (relation.Summary.Counters.RelationshipsDeleted > 0)
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
        // ~Neo4jRelationshipApiService() {
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