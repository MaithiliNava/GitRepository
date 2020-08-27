using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MemoryGameAPI.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MemoryGameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IWebHostEnvironment WebHostEnvironment;

        public PlayerController(IWebHostEnvironment WebHostEnv)
        {
            WebHostEnvironment = WebHostEnv;
        }

        [HttpGet]
        public ActionResult<Player> ReadJsonFile(string fName)
        {
            if (fName != null)
            {
                string fileDir = Path.Combine(WebHostEnvironment.WebRootPath, "game-boards");
                string file = Path.Combine(fileDir + fName);
                string playerText = System.IO.File.ReadAllText(file);
                object jsonObject = JsonConvert.DeserializeObject(playerText);
                Player player = (Player)jsonObject;
                return player;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IActionResult CreateJsonFile(Player player)
        {
            JObject jObj = (JObject)JToken.FromObject(player);
            string fileDir = Path.Combine(WebHostEnvironment.WebRootPath, "game-boards");
            string fName = Guid.NewGuid().ToString() + ".json";
            string fPath = Path.Combine(fileDir + fName);
            System.IO.File.WriteAllText(fPath, jObj.ToString());

            return CreatedAtRoute("Get", fName); 
        }

        [HttpPatch]
        public IActionResult PatchJsonFile([FromBody]JsonPatchDocument<Player> playerPatch, string fName)
        {
            if (playerPatch == null)
            {
                return BadRequest();
            }

            string fileDir = Path.Combine(WebHostEnvironment.WebRootPath, "game-boards");
            string file = Path.Combine(fileDir + fName);
            string playerText = System.IO.File.ReadAllText(file);
            object jsonObject = JsonConvert.DeserializeObject(playerText);
            Player orgPlayerDoc = (Player)jsonObject;

            playerPatch.ApplyTo(orgPlayerDoc);
            return Ok(orgPlayerDoc);

        }

        
    }
}