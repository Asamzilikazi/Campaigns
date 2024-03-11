using Campaigns.DataAccessLibrary.Repositories;
using Campaigns.DTOs;
using Campaigns.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CampaignsController : ControllerBase
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CampaignsController(ICampaignRepository campaignRepository, IHttpContextAccessor httpContextAccessor)
    {
        _campaignRepository = campaignRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    // GET: api/Campaigns
    [HttpGet("GetAllCampaigns")]
    [SwaggerOperation(Summary = "Get all campaigns", Description = "Retrieves a list of all campaigns.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        try
        {
            var campaigns = await _campaignRepository.GetAllAsync();
            return Ok(campaigns);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching campaigns");
        }
    }

    // GET: api/Campaigns/5
    [HttpGet("GetCampaignById/{id}")]
    [SwaggerOperation(Summary = "Get a campaign by ID", Description = "Retrieves a specific campaign by its ID.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }
            return Ok(campaign);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            return StatusCode(500, "An error occurred while fetching the campaign");
        }
    }

    //[Authorize]  //commented out this line to allow access without authentication
    [AllowAnonymous] //Remove or comment out this line to apply Authentication
    [HttpPost("CreateACampaign")]
    [SwaggerOperation(Summary = "Create a new campaign", Description = "Creates a new campaign.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] CampaignDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Mapping DTO 
        var campaign = new Campaign
        {
            CampaignId = dto.CampaignId,
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            ClientId = dto.ClientId,
            UserId = dto.UserId,
            //Client = dto.Client
        };

        try
        {
            await _campaignRepository.AddAsync(campaign);

            return Ok("Campaign created successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the campaign");
        }
    }

    //[Authorize]  //commented out this line to allow access without authentication
    [AllowAnonymous] //Remove or comment out this line to apply Authentication
    [HttpPut("UpdateACampaign/{id}")]
    [SwaggerOperation(Summary = "Update a campaign by ID", Description = "Updates an existing campaign by its ID.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, [FromBody] CampaignDto dto)
    {
        // Validate the input parameters
        if (id != dto.CampaignId)
        {
            return BadRequest("ID mismatch between URL and request body");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Retrieve the campaign from the repository by its ID
        var campaign = await _campaignRepository.GetByIdAsync(id);
        if (campaign == null)
        {
            return NotFound($"Campaign with ID {id} not found");
        }

        // Update the campaign properties with the values provided in the request body
        campaign.Name = dto.Name;
        campaign.Description = dto.Description;
        campaign.StartDate = dto.StartDate;
        campaign.EndDate = dto.EndDate;
        campaign.ClientId = dto.ClientId;
        campaign.UserId = dto.UserId;

        try
        {
            await _campaignRepository.UpdateAsync(campaign);

            return Ok($"Campaign with ID {id} updated successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the campaign: {ex.Message}");
        }
    }

    //[Authorize]  //commented out this line to allow access without authentication
    [AllowAnonymous] //Remove or comment out this line to apply Authentication
    [HttpDelete("DeleteACampaign/{id}")]
    [SwaggerOperation(Summary = "Delete a campaign by ID", Description = "Deletes a campaign by its ID.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        // Validate the input parameter
        if (id <= 0)
        {
            return BadRequest("Invalid campaign ID");
        }

        // Retrieve the campaign from the repository by its ID
        var campaign = await _campaignRepository.GetByIdAsync(id);
        if (campaign == null)
        {
            return NotFound($"Campaign with ID {id} not found");
        }

        try
        {
            await _campaignRepository.DeleteAsync(campaign);

            return Ok($"Campaign with ID {id} deleted successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the campaign: {ex.Message}");
        }
    }
    [HttpGet("SetSessionData")]
    public IActionResult SetSessionData()
    {
        // Set session data
        _httpContextAccessor.HttpContext.Session.SetString("SessionKey", "SessionValue");

        return Ok("Session data set successfully");
    }

    [HttpGet("GetSessionData")]
    public IActionResult GetSessionData()
    {
        // Retrieve session data
        var sessionValue = _httpContextAccessor.HttpContext.Session.GetString("SessionKey");

        if (sessionValue != null)
        {
            return Ok($"Session data: {sessionValue}");
        }
        else
        {
            return Ok("Session data not found");
        }
    }

}
