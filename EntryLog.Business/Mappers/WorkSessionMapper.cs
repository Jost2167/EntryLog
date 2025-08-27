using EntryLog.Business.DTOs;
using EntryLog.Entities.Entities;

namespace EntryLog.Business.Mappers
{
    internal static class WorkSessionMapper
    {
        public static GetWorkSessionDTO MapToGetWorkSessionDTO(WorkSession workSession)
        {
            return new GetWorkSessionDTO(
                workSession.Id.ToString(),
                workSession.EmployeeId,
                new GetCheckDTO(
                    workSession.CheckIn.Method,
                    workSession.CheckIn.DeviceName,
                    workSession.CheckIn.Date,
                    new GetLocationDTO(
                        workSession.CheckIn.Location.Latitude,
                        workSession.CheckIn.Location.Longitude,
                        workSession.CheckIn.Location.IpAddress
                    ),
                    workSession.CheckIn.PhotoUrl,
                    workSession.CheckIn.Note),
                workSession.CheckOut != null ? new GetCheckDTO(
                    workSession.CheckOut.Method,
                    workSession.CheckOut.DeviceName,
                    workSession.CheckOut.Date,
                    new GetLocationDTO(
                        workSession.CheckOut.Location.Latitude,
                        workSession.CheckOut.Location.Longitude,
                        workSession.CheckOut.Location.IpAddress
                    ),
                    workSession.CheckOut.PhotoUrl,
                    workSession.CheckOut.Note
                ) : null,
                workSession.TotalWorked,
                workSession.Status.ToString()
            );
        }
    }
}