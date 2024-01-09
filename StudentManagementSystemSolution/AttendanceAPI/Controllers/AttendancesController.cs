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

        [HttpGet]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> GetAllAttendances()
        {
            var attendanceRecords = await _attendanceService.GetAllAsync();
            var attendanceRecordsResponse = attendanceRecords.Select(a => a.ToAttendanceRecordResponse()).ToList();
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

            return Ok(attendanceRecords);
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

        [HttpGet("alreadyexists")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> AttendanceAlreadyExists()
        {
            var alreadyExists = await _attendanceService.CheckIfAlreadyExists();
            return Ok(alreadyExists);
        }

        [Authorize(Roles ="teacher,student")]
        [HttpGet("get")]
        public async Task<IActionResult> GetAttendance([FromQuery] AttendanceRecordGetDTO attendanceRequests)
        {
            var attendanceRecords = await _attendanceService.GetAttendance(attendanceRequests);
            var attendanceResponses = attendanceRecords.Select(x => x.ToAttendanceRecordResponse()).ToList();
            return Ok(attendanceResponses);
        }

    }
}
