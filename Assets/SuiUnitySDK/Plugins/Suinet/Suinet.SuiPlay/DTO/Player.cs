using System;
using System.Collections.Generic;

namespace Suinet.SuiPlay.DTO
{
    public class Player
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public object LastLogin { get; set; }
        public string UserId { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public Dictionary<string, Wallet> Wallets { get; set; }
    }
}
