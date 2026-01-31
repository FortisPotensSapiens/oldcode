using System.Reflection.PortableExecutable;
using AleBotApi.Bindings;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Урок
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/lesson-admin")]
    public class LessonAdminController : ControllerBase
    {
        private readonly ILogger<LessonAdminController> _logger;
        private readonly AbDbContext _db;

        public LessonAdminController(ILogger<LessonAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список уроков
        /// </summary>
        [HttpGet("lessons")]
        [ProducesResponseType(200, Type = typeof(List<AbLessonDbDto>))]
        public async Task<IActionResult> GetLessons(Guid? courseId)
        {
            var query = _db.Lessons.AsNoTracking();

            if (courseId.HasValue)
                query = query.Where(x => x.CourseId == courseId);

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получить урок
        /// </summary>
        [HttpGet("lessons/{lessonId}")]
        [ProducesResponseType(200, Type = typeof(AbLessonDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetLesson(Guid lessonId)
        {
            var lesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson == null)
                return NotFound($"Не найден урок {lesson}");

            return Ok(lesson);
        }

        /// <summary>
        /// Создать урок
        /// </summary>
        [HttpPost("lessons")]
        [ProducesResponseType(200, Type = typeof(AbLessonDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateLesson(CreateLessonBinding binding)
        {
            var hasLessonWithThisNumber = await _db.Lessons.AnyAsync(x => x.CourseId == binding.CourseId && x.Number == binding.Number);
            if (hasLessonWithThisNumber)
                return ValidationProblem("Урок с таким номером уже существует");
            if (!await _db.Courses.AnyAsync(x => x.Id == binding.CourseId))
                return BadRequest("Нельзя создать курс для несушествующего урока");
            var lesson = new AbLessonDbDto
            {
                CourseId = binding.CourseId,
                Name = binding.Name,
                Number = binding.Number,
                Body = binding.Body
            };

            _db.Lessons.Add(lesson);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateLesson Exc");
                return UnprocessableEntity("Не удалось создать урок");
            }

            return await GetLesson(lesson.Id);
        }

        /// <summary>
        /// Изменить урок
        /// </summary>
        [HttpPatch("lessons")]
        [ProducesResponseType(200, Type = typeof(AbLessonDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> ChangeLesson(ChangeLessonBinding binding)
        {
            var lesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == binding.LessonId);
            if (lesson == null)
                return NotFound($"Не найден урок {binding.LessonId}");

            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == binding.CourseId);
            if (course == null)
                return NotFound($"Не найден курс {binding.CourseId}");

            if (lesson.CourseId == binding.CourseId
                && lesson.Name == binding.Name
                && lesson.Number == binding.Number
                && lesson.Body == binding.Body)
                return Ok(lesson);

            lesson.CourseId = binding.CourseId;
            lesson.Name = binding.Name;
            lesson.Number = binding.Number;
            lesson.Body = binding.Body;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeLesson Exc");
                return UnprocessableEntity("Не удалось изменить урок");
            }

            return await GetLesson(lesson.Id);
        }

        /// <summary>
        /// Удалить урок
        /// </summary>
        [HttpDelete("lessons/{lessonId}")]
        [ProducesResponseType(200, Type = typeof(AbLessonDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteLesson(Guid lessonId)
        {
            var lesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson == null)
                return NotFound($"Не найден урок {lessonId}");

            _db.Lessons.Remove(lesson);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteLesson Exc");
                return UnprocessableEntity("Не удалось создать урок");
            }

            return Ok();
        }
    }
}
