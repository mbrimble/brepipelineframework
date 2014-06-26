using System.Data.SqlClient;
using Microsoft.RuleEngine;

namespace BREPipelineFramework
{
    internal class SQLDataConnectionWrapper
    {
        #region Private properties

        private SqlConnection sqlConnection;
        private DataConnection dataConnection;
        private string dbName;
        private string dbTable;

        #endregion

        #region Internal properties

        /// <summary>
        /// The SQLConnection from which the DataConnection is built
        /// </summary>
        internal SqlConnection SqlConnection
        {
            get { return sqlConnection; }
        }

        internal string DbName
        {
            get { return dbName; }
        }

        internal string DbTable
        {
            get { return dbTable; }
        }

        /// <summary>
        /// The DataConnection that will be asserted to the ExecutionPolicy
        /// </summary>
        internal DataConnection DataConnection
        {
            get { return dataConnection; }
        }

        #endregion

        #region Constructors

        internal SQLDataConnectionWrapper(string ConnectionString, string DBName, string DBTable)
        {
            sqlConnection = new SqlConnection(ConnectionString);
            dataConnection = new DataConnection(DBName, DBTable, sqlConnection);
            this.dbName = DBName;
            this.dbTable = DBTable;
        }

        #endregion
    }
}
