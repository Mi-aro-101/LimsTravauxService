using LimsTravauxService.Models;
using LimsTravauxService.Services;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;

namespace LimsTravauxService.Controllers;

[ApiController]
[Route("/api/avancee/travail")]
public class AvanceeTravailController : Controller
{
    private readonly IAvanceeTravailService avanceeTravailService;
    public AvanceeTravailController(IAvanceeTravailService avanceeTravailService)
    {
        this.avanceeTravailService = avanceeTravailService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAvanceeTravaux()
    {
        List<AvanceeTravail> avanceesTravaux = await avanceeTravailService.GetAvanceesTravaux();
        return Ok(new ApiResponse
        {
            Data = avanceesTravaux,
            IsSuccess = true,
            Message = "Liste des avancements de travaux récupérés avec succès.",
            StatusCode = 200
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse>> GetAvanceeTravail(int id)
    {
        try
        {
            AvanceeTravail avanceeTravail = await avanceeTravailService.GetAvanceeTravail(id);
            return Ok(new ApiResponse
            {
                Data = avanceeTravail,
                IsSuccess = true,
                Message = "Avancement de travail récupéré avec succès.",
                StatusCode = 200
            });
        }catch(Exception e)
        {
            return BadRequest(new ApiResponse{
                IsSuccess = false,
                Message = e.Message,
                StatusCode = 400
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateAvanceeTravail([FromBody] AvanceeTravail avanceeTravailDto)
    {
        AvanceeTravail avanceeTravail = await avanceeTravailService.CreateAvanceeTravail(avanceeTravailDto);
        return Ok(new ApiResponse
        {
            Data = avanceeTravail,
            IsSuccess = true,
            Message = "Avancement de travail créé avec succès.",
            StatusCode = 201
        });
    }
}