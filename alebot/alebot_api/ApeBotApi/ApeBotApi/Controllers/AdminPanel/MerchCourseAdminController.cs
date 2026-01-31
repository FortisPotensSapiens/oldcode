using AleBotApi.Bindings.Merch;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Курс продукта
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/merch-course-admin")]
    public class MerchCourseAdminController : ControllerBase
    {
        private readonly ILogger<MerchCourseAdminController> _logger;
        private readonly AbDbContext _db;

        public MerchCourseAdminController(ILogger<MerchCourseAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список курсов продукта
        /// </summary>
        [HttpGet("merch-courses")]
        [ProducesResponseType(200, Type = typeof(List<AbMerchCourseDbDto>))]
        public async Task<IActionResult> GetMerchCourses(Guid? merchId, Guid? courseId)
        {
            var query = _db.MerchCourses.AsNoTracking();

            if (merchId.HasValue)
                query = query.Where(x => x.MerchId == merchId);

            if (courseId.HasValue)
                query = query.Where(x => x.CourseId == courseId);

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получить курс продукта
        /// </summary>
        [HttpGet("merch-courses/{merchCourseId}")]
        [ProducesResponseType(200, Type = typeof(AbMerchCourseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetMerchCourse(Guid merchCourseId)
        {
            var merchCourse = await _db.MerchCourses.FirstOrDefaultAsync(x => x.Id == merchCourseId);
            if (merchCourse == null)
                return NotFound($"Не найден курс продукта {merchCourseId}");

            return Ok(merchCourse);
        }

        /// <summary>
        /// Создать курс продукта
        /// </summary>
        [HttpPost("merch-courses")]
        [ProducesResponseType(200, Type = typeof(AbMerchCourseDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateMerchCourse(CreateMerchCourseBinding binding)
        {
            var merchCourse = await _db.MerchCourses.FirstOrDefaultAsync(x => x.MerchId == binding.MerchId
                && x.CourseId == binding.CourseId);
            if (merchCourse != null)
                return ValidationProblem($"Уже есть курс {binding.CourseId} продукта {binding.MerchId}");

            var merch = await _db.Merches.FirstOrDefaultAsync(x => x.Id == binding.MerchId);
            if (merch == null)
                return NotFound($"Продукт {binding.MerchId} не найден");

            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == binding.CourseId);
            if (course == null)
                return NotFound($"Курс {binding.CourseId} не найдена");

            merchCourse = new AbMerchCourseDbDto
            {
                MerchId = binding.MerchId,
                CourseId = binding.CourseId
            };

            _db.MerchCourses.Add(merchCourse);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateMerchCourse Exc");
                return UnprocessableEntity("Не удалось создать курс пользователя");
            }

            return await GetMerchCourse(merchCourse.Id);
        }

        /// <summary>
        /// Удалить курс продукта
        /// </summary>
        [HttpDelete("merch-courses/{merchCourseId}")]
        [ProducesResponseType(200, Type = typeof(AbMerchCourseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteMerchCourse(Guid merchCourseId)
        {
            var merchCourse = await _db.MerchCourses.FirstOrDefaultAsync(x => x.Id == merchCourseId);
            if (merchCourse == null)
                return NotFound($"Не найдена курс продукта {merchCourseId}");

            _db.MerchCourses.Remove(merchCourse);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteMerchCourse Exc");
                return UnprocessableEntity("Не удалось удалить курс продукта");
            }

            return Ok();
        }
    }
}
