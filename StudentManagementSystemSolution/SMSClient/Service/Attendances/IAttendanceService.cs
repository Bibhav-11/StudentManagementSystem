using SMSClient.Model.ApiModel;

namespace SMSClient.Service.Attendances
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceList>> AttendanceResponseToList(IEnumerable<AttendanceResponse> attendanceResponses);

        Task<IEnumerable<AttendanceRequest>> GenerateRequestFromPresentStudentList(int[] presentStudentsIds, int teacherId);
    }
}
