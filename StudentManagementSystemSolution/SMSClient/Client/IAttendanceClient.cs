using SMSClient.Model.ApiModel;

namespace SMSClient.Client
{
    public interface IAttendanceClient
    {
        //Task<IEnumerable<AttendanceResponse>> GetAllAttendancesAsync(string accessToken);

        Task<IEnumerable<AttendanceResponse>> GetAttendances(AttendanceGetRequest attendanceRequest, string accessToken);

        //Task<IEnumerable<AttendanceResponse>> GetAttendancesOfAStudent(int studentId, string accessToken);
        Task<HttpResponseMessage> PostListOfAttendanceAsync(IEnumerable<AttendanceRequest> attendanceRequests, string accessToken);
        Task<bool> CheckIfAlreadyExists(string accessToken);
    }
}
