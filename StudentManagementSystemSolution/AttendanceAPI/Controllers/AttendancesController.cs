using AttendanceAPI.DTO;
using AttendanceAPI.Models;
using AttendanceAPI.Service;
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
        public async Task<IActionResult> GetAllAttendances()
        {
            var attendanceRecords = await _attendanceService.GetAllAsync();
            var attendanceRecordsResponse = attendanceRecords.Select(a => a.ToAttendanceRecordResponse()).ToList();
            return Ok(attendanceRecordsResponse);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAttendance([FromBody] AttendanceRecordRequest attendanceRecordRequest)
        {
            var attendanceRecord = attendanceRecordRequest.ToAttendanceRecord();
            await _attendanceService.CreateAsync(attendanceRecord);

            return CreatedAtAction("Get", new { attendanceId = attendanceRecord.Id }, attendanceRecord);

        }

        [HttpPost("all")]
        public async Task<IActionResult> CreateListOfAttendanceRecords([FromBody] IEnumerable<AttendanceRecordRequest> attendanceRecordRequests)
        {
            var attendanceRecords = attendanceRecordRequests.Select(x => x.ToAttendanceRecord());
            await _attendanceService.CreateListOfAttendancesAsync(attendanceRecords);

            return Ok(attendanceRecords);
        }

        [HttpGet("{attendanceId}")]
        public async Task<IActionResult> Get([FromRoute] int attendanceId)
        {
            var attendanceRecord = await _attendanceService.GetAsync(attendanceId);
            if (attendanceRecord is null) return NotFound();
            var attendanceRecordResponse = attendanceRecord.ToAttendanceRecordResponse();
            return Ok(attendanceRecordResponse);
        }

    }
}
