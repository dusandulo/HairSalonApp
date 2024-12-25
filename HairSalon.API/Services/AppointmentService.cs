using HairSalon.API.Data;
using HairSalon.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly DataContext _context;

    public AppointmentService(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Appointment>> GetFreeAppointmentsAsync()
    {
        return await _context.Appointments
            .Include(a => a.Stylist)
            .Where(a => a.Status == "Available")
            .OrderBy(a => a.AppointmentTime)
            .ToListAsync();
    }

    public async Task<Appointment> CreateAppointmentAsync(Appointment appointment, int stylistId)
    {
        var user = await _context.Users.FindAsync(stylistId);
        if (user?.Role != UserRole.Stylist)
        {
            throw new UnauthorizedAccessException("Only stylists can create appointments");
        }

        appointment.StylistId = stylistId;
        appointment.Status = "Available";
        appointment.ClientId = null;
        appointment.CreatedAt = DateTime.UtcNow;
        appointment.UpdatedAt = DateTime.UtcNow;

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<Appointment> BookAppointmentAsync(int appointmentId, int clientId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Stylist)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.Status == "Available");

        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found or already booked");
        }

        var user = await _context.Users.FindAsync(clientId);
        if (user?.Role != UserRole.Client)
        {
            throw new UnauthorizedAccessException("Only clients can book appointments");
        }

        var stylist = await _context.Users.FindAsync(appointment.StylistId);
        if (stylist == null || stylist.Role != UserRole.Stylist)
        {
            throw new UnauthorizedAccessException("Only stylists can be assigned to appointments");
        }

        appointment.ClientId = clientId;
        appointment.Status = "Booked";
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentHistoryAsync(int clientId)
    {
        return await _context.Appointments
            .Include(a => a.Stylist)
            .Where(a => a.ClientId == clientId)
            .OrderByDescending(a => a.AppointmentTime)
            .ToListAsync();
    }
}
