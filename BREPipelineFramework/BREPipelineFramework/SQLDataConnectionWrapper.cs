using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.RuleEngine;

namespace BREPipelineFramework
{
    internal class SQLDataConnectionWrapper
    {
        #region Private properties

        private SqlConnection sqlConnection;
        private DataConnection dataConnection;

        #endregion

        #region Internal properties

        internal SqlConnection SqlConnection
        {
            get { return sqlConnection; }
            set { sqlConnection = value; }
        }

        internal DataConnection DataConnection
        {
            get { return dataConnection; }
            set { dataConnection = value; }
        }

        #endregion

        #region Constructors

        internal SQLDataConnectionWrapper(string ConnectionString, string DBName, string DBTable)
        {
            sqlConnection = new SqlConnection(ConnectionString);
            dataConnection = new DataConnection(DBName, DBTable, sqlConnection);
        }

        #endregion
    }
}
