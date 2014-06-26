using System.Collections.Generic;
using System.Linq;
using Microsoft.RuleEngine;
using BREPipelineFramework.Helpers;

namespace BREPipelineFramework
{
    public class SQLDataConnectionCollection
    {
        #region Private properties

        private List<SQLDataConnectionWrapper> collection;

        #endregion

        #region Public properties

        /// <summary>
        /// Count of SQL Connections that should be asserted to the ExecutionPolicy
        /// </summary>
        public int SQLConnectionCount
        {
            get { return collection.Count(); }
        }

        #endregion

        #region Constructors

        public SQLDataConnectionCollection()
        {
            this.collection = new List<SQLDataConnectionWrapper>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add a SQLDataConnection by passing in the connection string, DBName, and DBTable
        /// </summary>
        public void AddSQLDataConnection(string SQLConnectionString, string DBName, string DBTable)
        {
            collection.Add(new SQLDataConnectionWrapper(SQLConnectionString, DBName, DBTable));
        }

        /// <summary>
        /// Add a SQLDataConnection by passing in an SSO Store and Key from which to fetch a connection string, a DBName, and DBTable
        /// </summary>
        public void AddSQLDataConnection(string SSOStore, string SSOKey, string DBName, string DBTable)
        {
            string _SQLConnectionString = StaticHelpers.ReadFromSSO(SSOStore, SSOKey);
            AddSQLDataConnection(_SQLConnectionString, DBName, DBTable);
        }

        /// <summary>
        /// Close all SQL Connection
        /// </summary>
        public void CloseSQLConnections()
        {
            foreach (SQLDataConnectionWrapper wrapper in collection)
            {
                wrapper.SqlConnection.Close();
            }
        }

        /// <summary>
        /// Get a SQLDataConnection from the collection by index
        /// </summary>
        public DataConnection GetDataConnectionByIndex(int index)
        {
            return collection.ElementAt(index).DataConnection;
        }

        /// <summary>
        /// Get details of a SQLDataConnection from the collection by index
        /// </summary>
        public void GetDataConnectionDetailsByIndex(int index, out string DBName, out string DBTable)
        {
            DBName = collection.ElementAt(index).DbName;
            DBTable = collection.ElementAt(index).DbTable;
        }

        #endregion
    }
}
