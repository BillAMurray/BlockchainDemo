using BlockchainDemo.Business;
using BlockchainDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace BlockchainDemo.Controllers
{
    public class HomeController : Controller
    {
        static List<Block> listBlockchain = new List<Block>();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Hash()
        {
            return View();
        }


        public IActionResult Block()
        {
            Block block = new Block
            {
                BlockIndex = 1
            };
            block = Blockchain.Mine(block);
            return View(block);
        }


        public IActionResult GenerateBlockchain()
        {
            listBlockchain = new List<Block>();
            Block block = new Block
            {
                BlockIndex = 1,
                PreviousHash = new string('0', 64)
            };
            block = Blockchain.Mine(block);
            listBlockchain.Add(block);
            return View();
        }


        public JsonResult InvalidateBlockchain(int idList, Block block)
        {
            listBlockchain[idList].Nonce = block.Nonce;
            if (string.IsNullOrEmpty(block.Data)) block.Data = "";
            listBlockchain[idList].Data = block.Data;
            listBlockchain[idList].BlockIndex = block.BlockIndex;
            string hash = "";
            string text = "";
            while (idList <= listBlockchain.Count - 1)
            {
                listBlockchain[idList].IsValid = false;
                text = listBlockchain[idList].BlockIndex.ToString() + listBlockchain[idList].Data;
                hash = Blockchain.GenerateHashCode(text);
                listBlockchain[idList].Hash = hash;
                if (idList > 0)
                {
                    listBlockchain[idList].PreviousHash = listBlockchain[idList - 1].Hash;
                }
                idList = idList + 1;
            }
            return Json(listBlockchain);
        }

        public JsonResult RemovePreviousBlockChain()
        {
            int ind = listBlockchain.Count;
            if ((ind -1) > 0)
                listBlockchain.RemoveAt(ind - 1);
            return Json(listBlockchain);
        }


        public JsonResult MineBlockchain(int idList, Block block)
        {
            listBlockchain[idList].Nonce = block.Nonce;
            if (string.IsNullOrEmpty(block.Data)) block.Data = "";
            listBlockchain[idList].Data = block.Data;
            listBlockchain[idList].BlockIndex = block.BlockIndex;
            var miningblock = Blockchain.Mine(listBlockchain[idList]);
            miningblock.IsValid = true;
            listBlockchain[idList] = miningblock;
            for (int i = 1; i < listBlockchain.Count; i++)
            {
                listBlockchain[i].PreviousHash = listBlockchain[i - 1].Hash;
            }
            return Json(listBlockchain);
        }


        public JsonResult GetBlockchain()
        {
            return Json(listBlockchain);
        }


        public JsonResult AddBlock()
        {
            var previousHash = listBlockchain[listBlockchain.Count - 1].Hash;
            int index = listBlockchain.Count + 1;
            Block block = new Block
            {
                BlockIndex = index,
                PreviousHash = previousHash
            };
            block = Blockchain.Mine(block);
            listBlockchain.Add(block);
            return Json(listBlockchain);
        }



        public IActionResult Mine(Block block)
        {
            block = Blockchain.Mine(block);
            return Json(block);
        }


        public IActionResult GetHash(string text)
        {
            if (string.IsNullOrEmpty(text)) text = "";
            return Json(Blockchain.GenerateHashCode(text));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
