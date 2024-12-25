using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HairSalon.API.Models;
using HairSalon.API.Services;
using System.Security.Claims;

namespace HairSalon.API.Controllers;

[ApiController]
[Route("api/appointments")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(IAppointmentService appointmentService, ILogger<AppointmentsController> logger)
    {
        _appointmentService = appointmentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetFreeAppointments()
    {
        try
        {
            var appointments = await _appointmentService.GetFreeAppointmentsAsync();
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching free appointments");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] Appointment appointment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointment, userId);
            return CreatedAtAction(nameof(GetFreeAppointments), new { id = createdAppointment.Id }, createdAppointment);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Unauthorized access while creating appointment: {Message}", ex.Message);
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("book/{id:int}")]
    public async Task<ActionResult<Appointment>> BookAppointment(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var appointment = await _appointmentService.BookAppointmentAsync(id, userId);
            return Ok(appointment);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Appointment not found or already booked");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error booking appointment");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentHistory()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var appointments = await _appointmentService.GetAppointmentHistoryAsync(userId);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching appointment history");
            return StatusCode(500, "Internal server error");
        }
    }
}
