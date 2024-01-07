using SMSClient.Models.ApiModel;

namespace SMSClient.Client
{
    public class AttendanceClient: IAttendanceClient
    {
        private readonly HttpClient _httpClient;

        public AttendanceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AttendanceResponse>> GetAllAttendancesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<AttendanceResponse>>($"attendance");
        }

        public async Task<HttpResponseMessage> PostListOfAttendanceAsync(IEnumerable<AttendanceRequest> attendanceRequests)
        {
            HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync("attendances/all", attendanceRequests);
            return responseMessage;
        }
    }
}
