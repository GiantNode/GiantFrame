﻿using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Giant.DB.MongoDB;

namespace Server.Account
{
    [BsonIgnoreExtraElements]
    public class AccountInfo : MongoUpdateTask
    {
        [BsonId]
        public string Account { get; set; }
        public string Password { get; set; }
        public string RegistTime { get; set; }
        public string LastLoginTime { get; set; }

        [BsonIgnoreIfNull]
        public List<int> Servers { get; set; }

        public override Task UpdateTask()
        {
            return new MongoDBUpdate<AccountInfo>("Account", x => x.Account == this.Account, this).Task();
        }
    }
}
