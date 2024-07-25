using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VTTCommonParameters.Dal.Entities.AppEntities;
using VTTCommonParameters.Repository.Repositories;

namespace VTTCommonParamaters.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class CommonParameterController : ControllerBase
    {
        [HttpGet("GetPageValuesById/{pageId}/{skip}/{take}")]
        public string GetPageValuesById(int pageId,int skip, int take)
        {
            GeneralRepository repository = new GeneralRepository();
            var response = JsonConvert.SerializeObject(repository.GetPageValuesById(pageId, skip, take));
            return response;
        }

        [HttpGet("GetAllPages")/*, Authorize(Roles ="Admin")*/]
        public string GetAllPages()
        {
            GeneralRepository repository = new GeneralRepository();
            var dataTable = repository.GetAllPages();
            var response = JsonConvert.SerializeObject(dataTable);
            return response;
        }
        [HttpGet("GetAllParamIDs/{pageId}")]
        public string GetParameterIDs(int pageId)
        {
            GeneralRepository repository = new GeneralRepository();
            var response = JsonConvert.SerializeObject(repository.GetParameterIDs(pageId));
            return response;
        }

        [HttpDelete("RemoveData/{rowId}/{pageId}")]
        public IActionResult RemoveData(int rowId, int pageId)
        {
            GeneralRepository repository = new GeneralRepository();
            repository.RemoveData(rowId,pageId);
            return Ok(new { message = "Data removed successfully" });
        }

        [HttpGet("GetTotalCount/{pageId}")]
        public int GetTotalCount(int pageId)
        {
            GeneralRepository repository = new GeneralRepository();
            var response=repository.GetTotalCount(pageId);
            return response;
        }

        [HttpPost("AddData")]
        public IActionResult AddData([FromBody]List<ParameterValue> parameterValues)
        {
            if (parameterValues == null || !parameterValues.Any())
            {
                return BadRequest(new { message = "No data to add" });
            }

            GeneralRepository repository = new GeneralRepository();
            repository.AddData(parameterValues);
            return Ok(new { message = "Data added successfully" });
        }


        [HttpPut("UpdateData/{rowId}/{pageId}")]
        public IActionResult UpdateData(int rowId, int pageId, [FromBody] List<ParameterValue> parameterValues)
        {
            GeneralRepository repository = new GeneralRepository();
            repository.UpdateData(rowId, pageId, parameterValues);
            return Ok(new { message = "Data updated successfully" });
        }
    }
}
