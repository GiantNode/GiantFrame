﻿using Giant.DB;
using Giant.DB.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Test
{
    class Test_Mongo
    {
        private Random random = new Random();

        public void Init()
        {
            DataBaseService.Instance.Init(DataBaseType.MongoDB, "127.0.0.1:27017", "Giant", "dbOwner", "dbOwner");
        }

        public async void TestMongo()
        {
            //await TestInsertBatch();
            await TestFindBatch(10);
            await TestDelete();
            await TestFindBatch(10);
        }

        public async Task TestFindBatch(int num)
        {
            List<PlayerInfo> players = await FindPlayerBySort(num);

            Console.WriteLine($"player count {players.Count}");
        }

        public async Task<List<PlayerInfo>> FindPlayerBySort(int limit = 99999)
        {
            BsonDocument bsons = new BsonDocument()
            {
                new BsonElement("Level", 1),
            };

            FindOptions<PlayerInfo> options = new FindOptions<PlayerInfo>
            {
                Limit = limit,
                Sort = bsons
            };

            var query = new MongoDBQueryBatch<PlayerInfo>("Player", x => x.Uid > 1, options);
            var player = await query.Task();

            return player;
        }


        private Task<DeleteResult> TestDelete()
        {
            var task = new MongoDBDeleteBatch<PlayerInfo>("Player", x => x.Uid > 0);
            return task.Task();
        }

        private async void TestInsert()
        {
            long uid = 10000 * 10;
            PlayerInfo player;
            for (int i = 0; i < 10; i++)
            {
                player = new PlayerInfo
                {
                    Uid = ++uid,
                    Account = $"Account&{uid}"
                };

                var task = new MongoDBInsert<PlayerInfo>("Player", player);
                await task.Task();
            }
        }

        private async Task TestInsertBatch()
        {
            long uid = 20000 * 10;
            List<PlayerInfo> player = new List<PlayerInfo>();
            for (int i = 0; i < 20; i++)
            {
                player.Add(new PlayerInfo
                {
                    Uid = ++uid,
                    Account = $"Account&{uid}",
                    Level = random.Next(10, 100)
                });
            }

            var task = new MongoDBInsertBatch<PlayerInfo>("Player", player);
            await task.Task();
        }
    }
}
