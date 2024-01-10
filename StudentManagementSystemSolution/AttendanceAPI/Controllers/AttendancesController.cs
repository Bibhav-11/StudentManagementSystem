using AttendanceAPI.DTO;
using AttendanceAPI.Models;
using AttendanceAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AttendancesController: ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet("all/{classId}")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> GetAllAttendancesOfAClass([FromRoute] int classId)
        {
            var attendanceRecords = await _attendanceService.GetAllAsync(classId);
            var attendanceRecordsResponse = attendanceRecords.Select(a => a.ToAttendanceRecordResponse()).ToList();
            if (attendanceRecordsResponse == null)
            {
                return Ok(new List<AttendanceRecordResponse>());
            }
            return Ok(attendanceRecordsResponse);
        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> MarkAttendance([FromBody] AttendanceRecordRequest attendanceRecordRequest)
        {
            var attendanceRecord = attendanceRecordRequest.ToAttendanceRecord();
            await _attendanceService.CreateAsync(attendanceRecord);

            return CreatedAtAction("Get", new { attendanceId = attendanceRecord.Id }, attendanceRecord);

        }

        [HttpPost("all")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> CreateListOfAttendanceRecords([FromBody] IEnumerable<AttendanceRecordRequest> attendanceRecordRequests)
        {
            var attendanceRecords = attendanceRecordRequests.Select(x => x.ToAttendanceRecord());
            await _attendanceService.CreateListOfAttendancesAsync(attendanceRecords);

            return Ok(new {Success = true});
        }

        [HttpGet("{studentId}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int studentId)
        {
            var attendanceRecord = await _attendanceService.GetAsync(studentId);
            if (attendanceRecord is null) return NotFound();
            var attendanceRecordResponses = attendanceRecord.Select(x => x.ToAttendanceRecordResponse());
            return Ok(attendanceRecordResponses);
        }

        [HttpGet("alreadyexists/{classId}")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> AttendanceAlreadyExists([FromRoute] int classid)
        {
            var alreadyExists = await _attendanceService.CheckIfAlreadyExists(classid);
            return Ok(alreadyExists);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAllAttendanceOfAStudent([FromRoute] int studentId)
        {
            var attendanceRecords = await _attendanceService.GetAsync(studentId);
            await _attendanceService.DeleteAllAttendanceOfAStudent(attendanceRecords);
            return Ok(new { Success = true} );
        }

        [Authorize(Roles ="teacher,student")]
        [HttpGet("get")]
        public async Task<IActionResult> GetAttendance([FromQuery] AttendanceRecordGetDTO attendanceRequests)
        {
            if (User.IsInRole("student"))
            {
                if(!User.HasClaim("student", attendanceRequests.StudentId.ToString()))
                {
                    return Forbid();
                }

            }
            var attendanceRecords = await _attendanceService.GetAttendance(attendanceRequests);
            var attendanceResponses = attendanceRecords.Select(x => x.ToAttendanceRecordResponse()).ToList();
            return Ok(attendanceResponses);
        }

    }
}
