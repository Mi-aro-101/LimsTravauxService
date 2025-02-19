using Microsoft.AspNetCore.Mvc;
using LimsTravauxService.Models;
using LimsTravauxService.Services;
using LimsTravauxService.Dto;
using LimsUtils.Api;
using LimsFrontEnd.Utils;

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

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateTypeTravaux([FromBody] TypeTravauxDto typeTravauxDto)
    {
        TypeTravaux typeTravaux = await _typeTravauxService.CreateTypeTravaux(typeTravauxDto);
        return Ok(new ApiResponse
        {
            Data = typeTravaux,
            IsSuccess = true,
            Message = "Type de travail créé avec succès.",
            StatusCode = 201
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetTypeTravaux(int id)
    {
        TypeTravaux typeTravaux = await _typeTravauxService.GetTypeTravaux(id);
        if (typeTravaux == null)
        {
            return NotFound(new ApiResponse
            {
                IsSuccess = false,
                Message = $"Type de travail avec l'id {id} n'existe pas.",
                StatusCode = 404
            });
        }
        return Ok(new ApiResponse
        {
            Data = typeTravaux,
            IsSuccess = true,
            Message = "Type de travail récupéré avec succès.",
            StatusCode = 200
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse>> UpdateTypeTravaux(int id, [FromBody] TypeTravauxDto typeTravauxDto)
    {
        TypeTravaux typeTravaux = await _typeTravauxService.UpdateTypeTravaux(id, typeTravauxDto);
        if (typeTravaux == null)
        {
            return NotFound();
        }

        return Ok(new ApiResponse
        {
            Data = typeTravaux,
            IsSuccess = true,
            Message = "Type de travail mis à jour avec succès.",
            StatusCode = 200
        });
    }

    [HttpPost("tarifier/{id}")]
    public async Task<ActionResult> Tarifier(int id, FormuleDto formuleDto)
    {
        Formule formule = new Formule();
        formule = formule.Evaluer(formuleDto);
        return Ok(new ApiResponse
        {
            Data = formule,
            IsSuccess = true,
            Message = "Formule calculée.",
            StatusCode = 200
        });
    }
}