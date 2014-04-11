using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
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

        public void AddSQLDataConnection(string SQLConnectionString, string DBName, string DBTable)
        {
            collection.Add(new SQLDataConnectionWrapper(SQLConnectionString, DBName, DBTable));
        }

        public void AddSQLDataConnection(string SSOStore, string SSOKey, string DBName, string DBTable)
        {
            string _SQLConnectionString = StaticHelpers.ReadFromSSO(SSOStore, SSOKey);
            AddSQLDataConnection(_SQLConnectionString, DBName, DBTable);
        }

        public void CloseSQLConnections()
        {
            foreach (SQLDataConnectionWrapper wrapper in collection)
            {
                wrapper.SqlConnection.Close();
            }
        }

        public DataConnection GetDataConnectionByIndex(int index)
        {
            return collection.ElementAt(index).DataConnection;
        }

        #endregion
    }
}
