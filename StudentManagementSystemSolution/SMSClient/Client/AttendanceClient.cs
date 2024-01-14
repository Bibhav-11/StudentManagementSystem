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

        public async Task<IEnumerable<AttendanceResponse>?> GetAllAttendances(int? classId, string accessToken)
        {
            try
            {

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<AttendanceResponse>?>($"attendances/all/{classId}");
                return result;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        //public async Task<HttpResponseMessage> PostListOfAttendanceAsync(IEnumerable<AttendanceRequest> attendanceRequests, string accessToken) 
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        //    HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync("attendances/all", attendanceRequests);
        //    return responseMessage;
        //}

        //public async Task<bool> CheckIfAlreadyExists(int classId, string accessToken)
        //{
        //    try
        //    {
        //        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        //        HttpResponseMessage responseMessage = await _httpClient.GetAsync($"attendances/alreadyexists/{classId}");
        //        string response = await responseMessage.Content.ReadAsStringAsync();
        //        return Boolean.Parse(response);
        //    }
        //    catch(Exception ex)
        //    {
        //        Log.Error($"{ex.Message}");
        //        throw;
        //    }
        //}

        public async Task<IEnumerable<AttendanceResponse>> GetAttendances(AttendanceGetRequest attendanceRequest, string accessToken)
        {
            try
            {

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<AttendanceResponse>>($"attendances/get?StudentId={attendanceRequest.StudentId}&TeacherId={attendanceRequest.TeacherId}&StartDate={attendanceRequest.StartDate}&EndDate={attendanceRequest.EndDate}&ClassId={attendanceRequest.ClassId}");
                return result;

            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        //FROM HERE

        public async Task<IEnumerable<AttendanceResponse>> GetAttendancesOfAClass(string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var attendanceResponses = await _httpClient.GetFromJsonAsync<IEnumerable<AttendanceResponse>>($"attendances/class");
                return attendanceResponses;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AttendanceResponse>> GetAttendancesForAStudent(string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var attendanceResponses = await _httpClient.GetFromJsonAsync<IEnumerable<AttendanceResponse>>($"attendances/myattendance");
                return attendanceResponses;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AttendanceResponse>> GetAttendancesOfAStudent(int studentId, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var attendanceResponses = await _httpClient.GetFromJsonAsync<IEnumerable<AttendanceResponse>>($"attendances/student/{studentId}");
                return attendanceResponses;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostListOfAttendanceAsync(IEnumerable<AttendanceRequest> attendanceRequests, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync($"attendances/all", attendanceRequests);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> DeleteAttendancesOfAStudent(int studentId, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage responseMessage = await _httpClient.DeleteAsync($"attendances/student/{studentId}");
            return responseMessage;
        }


        public async Task<bool> CheckIfAlreadyExists(string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage responseMessage = await _httpClient.GetAsync($"attendances/alreadyexists");
                string response = await responseMessage.Content.ReadAsStringAsync();
                return Boolean.Parse(response);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}");
                throw;
            }
        }

    }
}
