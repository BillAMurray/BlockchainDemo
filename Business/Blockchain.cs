using BlockchainDemo.Models;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainDemo.Business
{
    public static class Blockchain
    {
        public static Block Mine(Block block)
        {
            int seq = 1;
            bool validou = false;
            TimeSpan elapsedTime;

            var timer = new Stopwatch();
            timer.Start();
            while (validou == false)
            {
                block.Nonce = seq;
                var hash = GenerateHash(block);
                if (hash.Substring(0, 4) == "0000")
                {
                    timer.Stop();
                    elapsedTime = timer.Elapsed;
                    block.TimeSpent = elapsedTime.ToString(@"m\:ss\.fff");
                    block.Hash = hash;
                    validou = true;
                }
                else
                {
                    seq++;
                }
            }
            return block;
        }


        public static string GenerateHash(Block block)
        {
            try
            {
                var text = $"{block.BlockIndex.ToString()}{block.Nonce.ToString()}{block.Data}";
                var result = GenerateHashCode(text);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string GenerateHashCode(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) text = "";
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }




    }
}

