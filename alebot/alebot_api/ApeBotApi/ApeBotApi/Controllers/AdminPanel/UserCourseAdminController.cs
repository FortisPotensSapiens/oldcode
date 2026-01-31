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
    /// Курсы пользователя
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/user-course-admin")]
    public class UserCourseAdminController : ControllerBase
    {
        private readonly ILogger<UserCourseAdminController> _logger;
        private readonly AbDbContext _db;

        public UserCourseAdminController(ILogger<UserCourseAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список курсов пользователя
        /// </summary>
        [HttpGet("user-courses")]
        [ProducesResponseType(200, Type = typeof(List<AbUserCourseDbDto>))]
        public async Task<IActionResult> GetUserCourses(Guid? userId, Guid? courseId)
        {
            var query = _db.UserCourses.AsNoTracking();

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId);

            if (courseId.HasValue)
                query = query.Where(x => x.CourseId == courseId);

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получить курс пользователя
        /// </summary>
        [HttpGet("user-courses/{userCourseId}")]
        [ProducesResponseType(200, Type = typeof(AbUserCourseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetUserCourse(Guid userCourseId)
        {
            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.Id == userCourseId);
            if (userCourse == null)
                return NotFound($"Не найден курс пользователя {userCourse}");

            return Ok(userCourse);
        }

        /// <summary>
        /// Получить курс пользователя по ID пользователя и круса
        /// </summary>
        [HttpGet("user-courses/by-user-and-course")]
        [ProducesResponseType(200, Type = typeof(AbUserCourseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetUserCourseByUserAndCourse([Required] Guid userId, [Required] Guid courseId)
        {
            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);
            if (userCourse == null)
                return NotFound($"Не найден курс {courseId} пользователя {userId}");

            return Ok(userCourse);
        }

        /// <summary>
        /// Создать курс пользователя
        /// </summary>
        [HttpPost("user-courses")]
        [ProducesResponseType(200, Type = typeof(AbUserCourseDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateUserCourse(CreateUserCourseBinding binding)
        {
            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.UserId == binding.UserId && x.CourseId == binding.CourseId);
            if (userCourse != null)
                return ValidationProblem($"Уже есть курс {binding.CourseId} пользователя {binding.UserId}");

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == binding.UserId);
            if (user == null)
                return NotFound($"Пользовател {binding.UserId} не найден");

            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == binding.CourseId);
            if (course == null)
                return NotFound($"Курс {binding.UserId} не найден");

            userCourse = new AbUserCourseDbDto
            {
                UserId = binding.UserId,
                CourseId = binding.CourseId,
                LessonsLearned = []
            };

            _db.UserCourses.Add(userCourse);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUserCourse Exc");
                return UnprocessableEntity("Не удалось создать курс пользователя");
            }

            return await GetUserCourse(userCourse.Id);
        }

        /// <summary>
        /// Удалить курс пользователя
        /// </summary>
        [HttpDelete("user-courses/{userCourseId}")]
        [ProducesResponseType(200, Type = typeof(AbUserCourseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteUserCourse(Guid userCourseId)
        {
            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.Id == userCourseId);
            if (userCourse == null)
                return NotFound($"Не найден урок {userCourseId}");

            _db.UserCourses.Remove(userCourse);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteUserCourse Exc");
                return UnprocessableEntity("Не удалось создать курс пользователя");
            }

            return Ok();
        }

        /// <summary>
        /// Добавляем всем пользователям бесплатные курсы если их у них нет на данный момент.
        /// </summary>
        /// <returns></returns>
        [HttpPut("add-free-cource-to-all-users")]
        [ProducesResponseType(200)]
        public async Task AddFreeCourseToAllUsers()
        {
            var courses = await _db.Courses.Where(x => x.Free)
             .AsNoTracking()
             .Select(x => x.Id)
             .ToListAsync();
            var users = await _db.Users
                .AsNoTracking()
                .Select(x => x.Id)
                .ToListAsync();
            foreach (var userId in users)
                foreach (var courseId in courses)
                {
                    var userCourse = new AbUserCourseDbDto
                    {
                        UserId = userId,
                        CourseId = courseId,
                        LessonsLearned = []
                    };
                    if (!await _db.UserCourses.AnyAsync(uc => uc.UserId == userId && uc.CourseId == courseId))
                        _db.UserCourses.Add(userCourse);
                }
            await _db.SaveChangesAsync();
        }

    }
}
