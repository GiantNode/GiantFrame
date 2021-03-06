﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.DB.MySQL
{
    public class MySqlInsertPlayer : MySqlNonQueryTask
    {
        private readonly PlayerInfo player;

        public MySqlInsertPlayer(PlayerInfo player)
        {
            this.player = player;
        }

        public override async Task Run()
        {
            var connection = GetConnection();
            try
            {
                connection.Open();
                string sql = "INSERT INTO Player (Account,Uid,Level) VALUES(@account,@uid,@level)";

                // 方法1
                //var command = connection.CreateCommand();
                //command.CommandType = CommandType.Text;
                //command.CommandText = sql;
                //command.Parameters.AddWithValue("@account", this.player.Account);
                //command.Parameters.AddWithValue("@uid", this.player.Uid);
                //command.Parameters.AddWithValue("@level", this.player.Level);
                //await base.Run(command);

                //方法2
                await connection.QueryAsync(sql, player);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
            finally
            {
                connection.Dispose();
            }
        }
    }

    public class MySqlInsertPlayerBatch : MySqlNonQueryTask
    {
        private readonly List<PlayerInfo> players;

        public MySqlInsertPlayerBatch(List<PlayerInfo> players)
        {
            this.players = players;
        }

        public override async Task Run()
        {
            var connection = GetConnection();

            //方法1
            //connection.Open();
            //var command = connection.CreateCommand();
            //command.CommandType = CommandType.Text;

            //List<string> playerStrList = players.ConvertAll<string>(player => $"('{player.Account}','{player.Uid}','{player.Level}')");
            //string valueStr = string.Join(",", playerStrList);

            //command.CommandText = $"INSERT INTO player (Account,Uid,Level) VALUES {valueStr};";
            //await base.Run(command);

            //方法2
            string sql = "INSERT INTO player (account,uid,level, name,id) VALUES (@Account,@Uid,@Level,@Account, @Uid);";
            int result = await connection.ExecuteAsync(sql, players);
        }
    }
}
