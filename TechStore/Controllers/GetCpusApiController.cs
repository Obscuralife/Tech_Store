using Microsoft.AspNetCore.Mvc;
using TechStore.Controllers.BaseApiModel;
using TechStore.Models.FilteredProducts;
using TechStore.Repository;
using TS.DataAccessLayer.Models;

namespace TechStore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GetCpusApiController : BaseProductApiController<Cpu>
    {
        public GetCpusApiController(CpuRepository repository) : base(repository) { }
    }
}