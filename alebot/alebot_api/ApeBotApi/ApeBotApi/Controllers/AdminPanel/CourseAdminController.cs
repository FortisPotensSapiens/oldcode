using System.ComponentModel.DataAnnotations;
using AleBotApi.Bindings;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Курсы
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/course-admin")]
    public class CourseAdminController : ControllerBase
    {
        private readonly ILogger<CourseAdminController> _logger;
        private readonly AbDbContext _db;

        public CourseAdminController(ILogger<CourseAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список курсов
        /// </summary>
        [HttpGet("courses")]
        [ProducesResponseType(200, Type = typeof(List<AbCourseDbDto>))]
        public async Task<IActionResult> GetCourses()
        {
            return Ok(await _db.Courses
                .AsNoTracking()
                .ToListAsync());
        }

        /// <summary>
        /// Получить курс
        /// </summary>
        [HttpGet("courses/{courseId}")]
        [ProducesResponseType(200, Type = typeof(AbCourseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetCourse(Guid courseId)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
                return NotFound($"Не найден курс {courseId}");

            return Ok(course);
        }

        /// <summary>
        /// Создать курс
        /// </summary>
        [HttpPost("courses")]
        [ProducesResponseType(200, Type = typeof(AbCourseDbDto))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateCourse([FromBody][Required] CreateCourseBinding binding)
        {
            var course = new AbCourseDbDto
            {
                Name = binding.Name,
                Description = binding.Description,
                Photo = binding.Photo,
                Free = binding.Free
            };

            _db.Courses.Add(course);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateCourse Exc");
                return UnprocessableEntity("Не удалось создать курс");
            }

            return await GetCourse(course.Id);
        }

        /// <summary>
        /// Изменить курс
        /// </summary>
        [HttpPatch("courses")]
        [ProducesResponseType(200, Type = typeof(AbCourseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> ChangeCourse([FromBody][Required] ChangeCourseBinding binding)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == binding.CourseId);
            if (course == null)
                return NotFound($"Не найден курс {binding.CourseId}");

            if (course.Name == binding.Name
                && course.Description == binding.Description
                && course.Photo.SequenceEqual(binding.Photo)
                && course.Free == binding.Free)
                return Ok(course);

            course.Name = binding.Name;
            course.Description = binding.Description;
            course.Photo = binding.Photo;
            course.Free = binding.Free;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeCourse Exc");
                return UnprocessableEntity("Не удалось изменить курс");
            }

            return await GetCourse(course.Id);
        }

        /// <summary>
        /// Удалить курс
        /// </summary>
        [HttpDelete("courses/{courseId}")]
        [ProducesResponseType(200, Type = typeof(AbCourseDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
                return NotFound($"Не найден курс {courseId}");

            var lessons = await _db.Lessons.Where(x => x.CourseId == courseId).ToListAsync();
            if (lessons.Count > 0)
                return ValidationProblem($"Не удалось удалить курс, так как у него есть уроки");

            _db.Courses.Remove(course);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteCourse Exc");
                return UnprocessableEntity("Не удалось удалить курс");
            }

            return Ok();
        }
    }
}
