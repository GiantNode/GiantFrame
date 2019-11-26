﻿using System.Data;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public class MySqlDeleteTable : MySqlNonQueryTask
    {
        private readonly string tableName;

        public MySqlDeleteTable(string tableName)
        {
            this.tableName = tableName;
        }

        public override async Task Run()
        {
            var connection = GetConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = $"DELETE FROM {tableName}";

            await base.Run(command);
        }
    }
}
