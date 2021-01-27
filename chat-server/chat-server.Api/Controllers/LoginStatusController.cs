using Microsoft.AspNetCore.Mvc;
using chat_server.Entity;
using chat_server.Entity.interfaces;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace chat_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    //[Authorize]
    public class LoginStatusController : GenericController<tblLogedinStatus>
    {
        private IGenericService<tblLogedinStatus> _genericService;
        public LoginStatusController(IGenericService<tblLogedinStatus> genericService) : base(genericService)
        {
            _genericService = genericService;
        }
    }
}
