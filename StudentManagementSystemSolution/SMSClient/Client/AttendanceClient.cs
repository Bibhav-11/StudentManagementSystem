using Serilog;
using SMSClient.Model.ApiModel;

namespace SMSClient.Client
{
    public class AttendanceClient : IAttendanceClient
    {
        private readonly HttpClient _httpClient;

        public AttendanceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            
        }



        public async Task<HttpResponseMessage> PostListOfAttendanceAsync(IEnumerable<AttendanceRequest> attendanceRequests, string accessToken) 
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync("attendances/all", attendanceRequests);
            return responseMessage;
        }

        public async Task<bool> CheckIfAlreadyExists(string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage responseMessage = await _httpClient.GetAsync("attendances/alreadyexists");
                string response = await responseMessage.Content.ReadAsStringAsync();
                return Boolean.Parse(response);
            }
            catch(Exception ex)
            {
                Log.Error($"{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AttendanceResponse>> GetAttendances(AttendanceGetRequest attendanceRequest, string accessToken)
        {
            try
            {

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<AttendanceResponse>>($"attendances/get?StudentId={attendanceRequest.StudentId}&TeacherId={attendanceRequest.TeacherId}&StartDate={attendanceRequest.StartDate}&EndDate={attendanceRequest.EndDate}");
                return result;

            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
