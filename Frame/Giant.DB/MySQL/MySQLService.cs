﻿using Giant.DataTask;
using MySql.Data.MySqlClient;

namespace Giant.DB
{
    public class MySQLService : IDBService
    {
        private readonly string connStr;
        private MySqlConnection connection;

        public DataBaseService DBService { get; private set; }

        public MySQLService(string host, string dbName, string account, string passWorld)
        {
            this.connStr = $"Server={host};Database={dbName};Uid={account};Pwd = {passWorld}; ";
        }

        public void Start()
        {
        }

        public MySqlCommand GetCommand()
        {
            return GetConnection().CreateCommand();
        }

        public MySqlConnection GetConnection()
        {
            connection = new MySqlConnection(this.connStr);
            return connection;
        }
    }
}
