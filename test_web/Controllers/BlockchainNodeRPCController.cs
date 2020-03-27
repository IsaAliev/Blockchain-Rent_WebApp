using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace Rent_WebApp.Controllers
{
    public class RPCRequest
    {
        public const string jsonrpc = "2.0";
        [JsonPropertyName("method")]
        public string Method { get; set; }
        [JsonPropertyName("params")]
        public object[] Parameters { get; set; }
        [JsonPropertyName("id")]
        public string ID { get; set; }
    }

    [ApiController]
    [Route("NodeRPC")]
    public class BlockchainNodeRPCController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public BlockchainNodeRPCController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("GetBlock/{blockHeight}")]
        public async Task<string> GetBlock(int blockHeight)
        { 
            var rpcReq = new RPCRequest();
            object[] parameters = new object[2];

            parameters[0] = blockHeight;
            parameters[1] = 1;
            rpcReq.ID = "1";
            rpcReq.Method = "getblock";
            rpcReq.Parameters = parameters;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(rpcReq))
            };
            request.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var client = _clientFactory.CreateClient("nodeRPC");
            string result = await client.SendAsync(request).Result.Content.ReadAsStringAsync();

            return result;
        }
    }
}
