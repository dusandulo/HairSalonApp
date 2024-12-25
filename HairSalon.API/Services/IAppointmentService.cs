using HairSalon.API.Models;

namespace HairSalon.API.Services;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> GetFreeAppointmentsAsync();
    Task<Appointment> CreateAppointmentAsync(Appointment appointment, int stylistId);
    Task<Appointment> BookAppointmentAsync(int appointmentId, int clientId);
    Task<IEnumerable<Appointment>> GetAppointmentHistoryAsync(int clientId);
}
