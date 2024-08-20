using Microsoft.AspNetCore.Mvc;
using PubEventManager.Application.Dtos;
using PubEventManager.Application.IServices;
using PubEventManager.Application.Services;

namespace PubEventManager.Api.Controllers;

[ApiController]
[Route("api/tables")]
public class TableController : ControllerBase
{
    private readonly ITableService _tableService;
    public TableController(ITableService tableService)
    {
        _tableService = tableService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] int capacity)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _tableService.CreateTable(capacity);
        if (!result)
        {
            return BadRequest("Adding failed.");
        }

        return Ok(new { message = "Table created successfully." });
    }
}
