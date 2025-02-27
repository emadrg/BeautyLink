using API.Attributes;
using BL.Services;
using DTOs.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [APIEndpoint]
    [Route("client")]
    public class ClientController : ControllerBase
    {
        private readonly ClientService ClientService;

        public ClientController(ClientService clientService)
        {
            ClientService = clientService;
        }

        [HttpGet]
        [Route("{clientId}")]
        [Authorize]
        public async Task<ViewClientDetailsDTO> ClientDetails(Guid clientId)
        {
            return await ClientService.ViewClientDetails(clientId);
        }
    }
}
