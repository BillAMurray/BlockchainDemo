using System;

namespace BlockchainDemo.Models
{
    public class Block
    {
        public int BlockIndex { get; set; } = 0;
        public string Data { get; set; } = string.Empty;

        public string TimeSpent { get; set; } = string.Empty;
        public int Nonce { get; set; } = 0;
        public string PreviousHash { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public bool IsValid { get; set; } = true;
    }
}

