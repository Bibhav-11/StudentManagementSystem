using SMSClient.Model.ApiModel;
using System.Security.Claims;

namespace SMSClient.Service.Attendances
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceList>> AttendanceResponseToList(IEnumerable<AttendanceResponse> attendanceResponses);

        Task<IEnumerable<AttendanceRequest>> GenerateRequestFromPresentStudentList(int[] presentStudentsIds, int teacherId, int? classId);

        Task<int?> GetClassId(ClaimsPrincipal user);
    }
}
