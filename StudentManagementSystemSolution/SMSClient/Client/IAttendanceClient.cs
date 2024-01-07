using SMSClient.Models.ApiModel;

namespace SMSClient.Client
{
    public interface IAttendanceClient
    {
        Task<IEnumerable<AttendanceResponse>> GetAllAttendancesAsync();
        Task<HttpResponseMessage> PostListOfAttendanceAsync(IEnumerable<AttendanceRequest> attendanceRequests); 
    }
}
