using SMSClient.Model.ApiModel;

namespace SMSClient.Client
{
    public interface IAttendanceClient
    {

        Task<IEnumerable<AttendanceResponse>?> GetAllAttendances(int? classId, string accessToken);

        Task<IEnumerable<AttendanceResponse>> GetAttendances(AttendanceGetRequest attendanceRequest, string accessToken);



        //Task<IEnumerable<AttendanceResponse>> GetAttendancesOfAStudent(int studentId, string accessToken);
        Task<HttpResponseMessage> PostListOfAttendanceAsync(IEnumerable<AttendanceRequest> attendanceRequests, string accessToken);
        Task<bool> CheckIfAlreadyExists(string accessToken);


        //NEW API
        Task<IEnumerable<AttendanceResponse>> GetAttendancesOfAClass(string accessToken);

        Task<IEnumerable<AttendanceResponse>> GetAttendancesForAStudent(string accessToken);

        Task<IEnumerable<AttendanceResponse>> GetAttendancesOfAStudent(int studentId, string accessToken);
    }
}
