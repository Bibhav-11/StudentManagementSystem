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

        //[HttpPost]
        //[Authorize(Roles = "teacher")]
        //public async Task<IActionResult> MarkAttendance([FromBody] AttendanceRecordRequest attendanceRecordRequest)
        //{
        //    var attendanceRecord = attendanceRecordRequest.ToAttendanceRecord();
        //    await _attendanceService.CreateAsync(attendanceRecord);

        //    return CreatedAtAction("Get", new { attendanceId = attendanceRecord.Id }, attendanceRecord);

        //}

        //[HttpPost("all")]
        //[Authorize(Roles = "teacher")]
        //public async Task<IActionResult> CreateListOfAttendanceRecords([FromBody] IEnumerable<AttendanceRecordRequest> attendanceRecordRequests)
        //{
        //    var attendanceRecords = attendanceRecordRequests.Select(x => x.ToAttendanceRecord());
        //    await _attendanceService.CreateListOfAttendancesAsync(attendanceRecords);

        //    return Ok(new {Success = true});
        //}

        [HttpGet("{studentId}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int studentId)
        {
            var attendanceRecord = await _attendanceService.GetAsync(studentId);
            if (attendanceRecord is null) return NotFound();
            var attendanceRecordResponses = attendanceRecord.Select(x => x.ToAttendanceRecordResponse());
            return Ok(attendanceRecordResponses);
        }

        //[HttpGet("alreadyexists/{classId}")]
        //[Authorize(Roles = "teacher")]
        //public async Task<IActionResult> AttendanceAlreadyExists([FromRoute] int classid)
        //{
        //    var alreadyExists = await _attendanceService.CheckIfAlreadyExists(classid);
        //    return Ok(alreadyExists);
        //}

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


        //From HERE
        [Authorize(Roles ="student")]
        [HttpGet("myattendance")]
        public async Task<IActionResult> GetAttendancesForStudent()
        {
            var studentId = User.Claims.Where(c => c.Type == "student").FirstOrDefault()?.Value;
            if (studentId == null) { return NotFound();  }

            var attendanceRecords = await _attendanceService.GetAttendancesForAStudent(Int32.Parse(studentId));
            var attendanceResponses = attendanceRecords.Select(x => x.ToAttendanceRecordResponse());

            return Ok(attendanceResponses);
        }

        [Authorize(Roles ="teacher")]
        [HttpGet("class")]
        public async Task<IActionResult> GetAttendancesOfAClass()
        {
            var classId = User.Claims.Where(c => c.Type == "class").FirstOrDefault()?.Value;
            var teacherId = User.Claims.Where(c => c.Type == "teacher").FirstOrDefault()?.Value;
            if (classId == null || teacherId == null) { return NotFound(); }

            var attendanceRequest = new AttendanceRecordGetDTO
            {
                ClassId = Int32.Parse(classId),
                TeacherId = Int32.Parse(teacherId),
            };

            var attendanceRecords = await _attendanceService.GetAttendance(attendanceRequest);
            var attendanceResponses = attendanceRecords.Select(x => x.ToAttendanceRecordResponse());

            return Ok(attendanceResponses);
        }

        [Authorize(Roles ="teacher")]
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetAllAttendancesOfAStudent(int studentId)
        {
            var attendanceRecords = await _attendanceService.GetAttendancesForAStudent(studentId);
            var attendanceRecordResponse = attendanceRecords.Select(x => x.ToAttendanceRecordResponse());
            return Ok(attendanceRecordResponse);
        }

        [Authorize(Roles ="teacher")]
        [HttpPost("all")]
        public async Task<IActionResult> MarkAttendanceRecords([FromBody] IEnumerable<AttendanceRecordRequest> attendanceRecords)
        {
            var classId = User.Claims.Where(c => c.Type == "class").FirstOrDefault()?.Value;
            var teacherId = User.Claims.Where(c => c.Type == "teacher").FirstOrDefault()?.Value;
            if (classId == null || teacherId == null) { return BadRequest(); }

            var attendanceRecordList = attendanceRecords.Select(attendance => attendance.ToAttendanceRecord(Int32.Parse(teacherId), Int32.Parse(classId)));
            await _attendanceService.CreateListOfAttendancesAsync(attendanceRecordList);

            return new JsonResult(new { Success = true, Message = "Successfully taken attendance" });
        }

        [Authorize(Roles ="admin")]
        [HttpDelete("student/{studentId}")]
        public async Task<IActionResult> DeleteAttendance(int studentId)
        {
            var attendanceRecords = await _attendanceService.GetAttendancesForAStudent(studentId);
            await _attendanceService.DeleteAllAttendanceOfAStudent(attendanceRecords);
            return new JsonResult(new { Success = true, Message = "Successfully deleted" });
        }

        [HttpGet("alreadyexists")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> AttendanceAlreadyExists()
        {
            var classId = User.Claims.Where(c => c.Type == "class").FirstOrDefault()?.Value;
            if (classId is null) return NotFound();
            var alreadyExists = await _attendanceService.CheckIfAlreadyExists(Int32.Parse(classId));
            return Ok(alreadyExists);
        }

    }
}
