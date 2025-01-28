using Microsoft.AspNetCore.Mvc;
using LimsTravauxService.Models;
using LimsTravauxService.Services;
using LimsTravauxService.Utils;

namespace LimsTravauxService.Controllers;

[ApiController]
[Route("api/type/travaux")]
public class TypeTravauxController : ControllerBase
{
    private readonly ITypeTravauxService _typeTravauxService;
    public TypeTravauxController(ITypeTravauxService typeTravauxService)
    {
        _typeTravauxService = typeTravauxService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetTypeTravauxFrom(int position, int pageSize)
    {
        if (position == 0) position = 1;
        if (pageSize == 0) pageSize = 2;
        Dictionary<string, object> response = new Dictionary<string, object>();
        response["nbrPerPage"] = pageSize;
        int totalTypeTravauxRows = _typeTravauxService.CountTypeTravaux();
        response["TotalCount"] = totalTypeTravauxRows;
        response["nbrLinks"] = Math.Ceiling((double)totalTypeTravauxRows / pageSize);

            response["position"] = position;
            int skiped = (position-1) * pageSize;
            List<TypeTravaux> typeTravaux = await _typeTravauxService.GetTypeTravauxFrom(skiped, pageSize);
            return Ok(new ApiResponse
            {
                Data = typeTravaux,
                ViewBag = response,
                IsSuccess = true,
                Message = "Datas retrieved successfully.",
                StatusCode = 200
            });
    }
}