using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using AleBotApi.Bindings.License;
using AleBotApi.Clients;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.Models.RDtos;
using AleBotApi.Services;
using ApeBotApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers
{
    /// <summary>
    /// Сервера VDS которые купил пользователь
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1")]
    public class PurchasesController : ControllerBase
    {
        private readonly ILogger<PurchasesController> _logger;
        private readonly AbDbContext _db;
        private readonly IEmailService _emailService;
        private readonly IDarkbandRuClient _darkbandRu;

        public PurchasesController(
            ILogger<PurchasesController> logger,
            AbDbContext db,
            IEmailService emailService,
            IDarkbandRuClient darkbandRu
            )
        {
            _logger = logger;
            _db = db;
            _emailService = emailService;
            _darkbandRu = darkbandRu;
        }

        /// <summary>
        /// Получить курсы текущего пользователя
        /// </summary>
        /// <param name="onlyPaid">Только платные</param>
        [HttpGet("courses")]
        [ProducesResponseType(200, Type = typeof(List<UserCourseRDto>))]
        public async Task<IActionResult> GetUserCourses(bool onlyPaid = default)
        {
            var query = _db.UserCourses
                .Join(_db.Courses, uc => uc.CourseId, c => c.Id, (uc, c) => new { UserCourse = uc, Course = c })
                .OrderByDescending(x => x.UserCourse.Created)
                .AsNoTracking()
                .Where(x => x.UserCourse.UserId == CurrentUserId);

            if (onlyPaid)
                query = query.Where(x => x.Course.Free == false);

            var resultQuery = query.Select(x => new UserCourseRDto
            {
                CourseId = x.Course.Id,
                CourseName = x.Course.Name,
                CoursePhoto = x.Course.Photo,
                CourseFree = x.Course.Free,
                LastLessonId = x.UserCourse.LastLessonId,
                LessonsLearned = (uint)x.UserCourse.LessonsLearned.Count(),
                Description = x.Course.Description,
                TotalLessonsCount = _db.Lessons.Count(l => l.CourseId == x.Course.Id)
            });

            return Ok(await resultQuery.ToListAsync());
        }

        /// <summary>
        /// Получить уроки курса
        /// </summary>
        [HttpGet("courses/{courseId}/lessons")]
        [ProducesResponseType(200, Type = typeof(List<UserCourseLessonBriefRDto>))]
        public async Task<IActionResult> GetUserCourses(Guid courseId)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
                return NotFound($"Не найден курс {courseId}");

            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.UserId == CurrentUserId && x.CourseId == courseId);
            if (userCourse == null)
                return ValidationProblem($"Курс не доступен");

            var query = _db.Lessons
                .AsNoTracking()
                .Where(x => x.CourseId == courseId)
                .OrderBy(x => x.Number)
                .Select(x => new UserCourseLessonBriefRDto
                {
                    LessonId = x.Id,
                    LessonName = x.Name,
                    LessonNumber = x.Number
                });

            var result = await query.ToListAsync();

            foreach (var lesson in result)
                lesson.LessonLearned = userCourse.LessonsLearned.Contains(lesson.LessonId);

            return Ok(result);
        }

        /// <summary>
        /// Получить первый непройденный урок курса
        /// </summary>
        [HttpGet("courses/{courseId}/first-notlearned-lesson")]
        [ProducesResponseType(200, Type = typeof(UserCourseLessonRDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> GetFirstNotLearnedLesson(Guid courseId)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
                return NotFound($"Не найден курс {courseId}");

            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.UserId == CurrentUserId && x.CourseId == courseId);
            if (userCourse == null)
                return ValidationProblem($"Курс не доступен");

            var result = new UserCourseLessonRDto
            {
                CourseName = course.Name,
                CourseDescription = course.Description,
                CoursePhoto = course.Photo,
                CourseFree = course.Free,

                LastLessonId = userCourse.LastLessonId,
                LessonsLearned = (uint)userCourse.LessonsLearned.Count(),
            };

            AbLessonDbDto? nextLesson = null;
            {
                var nextLessonQuery = _db.Lessons
                    .Where(x => x.CourseId == course.Id)
                    .Where(x => !userCourse.LessonsLearned.Contains(x.Id))
                    .AsNoTracking();

                var lastLessonOfCourse = await nextLessonQuery
                    .OrderByDescending(x => x.Number)
                    .FirstOrDefaultAsync();

                if (userCourse.LastLessonId.HasValue && lastLessonOfCourse?.Id != userCourse.LastLessonId)
                {
                    var lastLearnedLessonNumbmer = await _db.Lessons
                        .Where(x => x.Id == userCourse.LastLessonId)
                        .Select(x => x.Number)
                        .FirstOrDefaultAsync();

                    nextLesson = await nextLessonQuery.Where(x => x.Number > lastLearnedLessonNumbmer).OrderBy(x => x.Number).FirstOrDefaultAsync();
                }

                if (nextLesson == null)
                    nextLesson = await nextLessonQuery.OrderBy(x => x.Number).FirstOrDefaultAsync();
                if (nextLesson == null)
                    nextLesson = await _db.Lessons.OrderBy(x => x.Number).FirstOrDefaultAsync(x => x.CourseId == course.Id);
            }
            Debug.Assert(nextLesson != null);

            result.LessonId = nextLesson.Id;
            result.LessonName = nextLesson.Name;
            result.LessonNumber = nextLesson.Number;
            result.LessonBody = nextLesson.Body;

            await SetLessonAsLearnedAsync(nextLesson.Id);

            return Ok(result);
        }

        /// <summary>
        /// Получить урок
        /// </summary>
        [HttpGet("courses/lesson/{lessonId}")]
        [ProducesResponseType(200, Type = typeof(UserCourseLessonRDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetLesson(Guid lessonId)
        {
            var lesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson == null)
                return ValidationProblem($"Не найден урок {lessonId}");

            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == lesson.CourseId);
            if (course == null)
                return ValidationProblem($"Не найден курс для урока {lessonId}");

            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.CourseId == lesson.CourseId && x.UserId == CurrentUserId);
            if (userCourse == null)
                return ValidationProblem($"Не найден урок для курс {lesson.CourseId} пользователя {CurrentUserId}");

            var result = new UserCourseLessonRDto
            {
                CourseName = course.Name,
                CourseDescription = course.Description,
                CoursePhoto = course.Photo,
                CourseFree = course.Free,

                LastLessonId = userCourse.LastLessonId,
                LessonsLearned = (uint)userCourse.LessonsLearned.Count(),

                LessonId = lesson.Id,
                LessonName = lesson.Name,
                LessonNumber = lesson.Number,
                LessonBody = lesson.Body
            };

            await SetLessonAsLearnedAsync(lesson.Id);

            return Ok(result);
        }

        /// <summary>
        /// Получить идентификатор слудующего урока курса
        /// </summary>
        [HttpGet("courses/next-lesson/{currentLessonId}")]
        [ProducesResponseType(200, Type = typeof(UserCourseLessonRDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetNextLesson(Guid currentLessonId)
        {
            var currentLesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == currentLessonId);
            if (currentLesson == null)
                return ValidationProblem($"Не найден урок {currentLessonId}");

            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == currentLesson.CourseId);
            if (course == null)
                return ValidationProblem($"Не найден курс для урока {currentLessonId}");

            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.CourseId == currentLesson.CourseId && x.UserId == CurrentUserId);
            if (userCourse == null)
                return ValidationProblem($"Не найден урок для курс {currentLesson.CourseId} пользователя {CurrentUserId}");

            var nextLesson = await _db.Lessons
                .Where(x => x.CourseId == course.Id && x.Number > currentLesson.Number)
                .OrderBy(x => x.Number)
                .FirstOrDefaultAsync();

            if (nextLesson == null)
                nextLesson = currentLesson;

            var result = new UserCourseLessonRDto
            {
                CourseName = course.Name,
                CourseDescription = course.Description,
                CoursePhoto = course.Photo,
                CourseFree = course.Free,

                LastLessonId = userCourse.LastLessonId,
                LessonsLearned = (uint)userCourse.LessonsLearned.Count(),

                LessonId = nextLesson.Id,
                LessonName = nextLesson.Name,
                LessonNumber = nextLesson.Number,
                LessonBody = nextLesson.Body
            };

            await SetLessonAsLearnedAsync(nextLesson.Id);

            return Ok(result);
        }

        /// <summary>
        /// Получить идентификатор предыдущего урока курса
        /// </summary>
        [HttpGet("courses/prev-lesson/{currentLessonId}")]
        [ProducesResponseType(200, Type = typeof(UserCourseLessonRDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetPrevLesson(Guid currentLessonId)
        {
            var currentLesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == currentLessonId);
            if (currentLesson == null)
                return ValidationProblem($"Не найден урок {currentLessonId}");

            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == currentLesson.CourseId);
            if (course == null)
                return ValidationProblem($"Не найден курс для урока {currentLessonId}");

            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.CourseId == currentLesson.CourseId && x.UserId == CurrentUserId);
            if (userCourse == null)
                return ValidationProblem($"Не найден урок для курс {currentLesson.CourseId} пользователя {CurrentUserId}");

            var prevLesson = await _db.Lessons
                .Where(x => x.CourseId == course.Id && x.Number < currentLesson.Number)
                .OrderByDescending(x => x.Number)
                .FirstOrDefaultAsync();

            if (prevLesson == null)
                prevLesson = currentLesson;

            var result = new UserCourseLessonRDto
            {
                CourseName = course.Name,
                CourseDescription = course.Description,
                CoursePhoto = course.Photo,
                CourseFree = course.Free,

                LastLessonId = userCourse.LastLessonId,
                LessonsLearned = (uint)userCourse.LessonsLearned.Count(),

                LessonId = prevLesson.Id,
                LessonName = prevLesson.Name,
                LessonNumber = prevLesson.Number,
                LessonBody = prevLesson.Body
            };

            await SetLessonAsLearnedAsync(prevLesson.Id);

            return Ok(result);
        }

        /// <summary>
        /// Пометить урок как пройденный
        /// </summary>
        private async Task<IActionResult> SetLessonAsLearnedAsync(Guid lessonId)
        {
            var lesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson == null)
                return ValidationProblem($"Не найден урок {lessonId}");

            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == lesson.CourseId);
            if (course == null)
                return ValidationProblem($"Не найден для урока {lessonId}");

            var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.UserId == CurrentUserId && x.CourseId == course.Id);
            if (userCourse == null)
                return ValidationProblem($"Не найден курс пользователя {CurrentUserId} для урока {lessonId}");

            if (userCourse.LessonsLearned.Contains(lessonId))
                return Ok();

            userCourse.LastLessonId = lesson.Id;
            userCourse.LessonsLearned = [.. userCourse.LessonsLearned, .. new[] { lesson.Id }];

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SetLessonAsLearned Exc");
                return ValidationProblem("Не удалось пометить урок как пройденный");
            }

            return Ok();
        }

        /// <summary>
        /// Получить список всех купленных Лицензий
        /// </summary>
        /// <returns></returns>
        [HttpGet("licenses")]
        [ProducesResponseType(200, Type = typeof(List<UserLicenseRDto>))]
        public async Task<IActionResult> GetAllLicenses()
        {
            var query = _db.UserLicenses
                .Join(_db.Licenses, ul => ul.LicenseId, l => l.Id, (ul, l) => new { UserLicense = ul, License = l })
                .OrderByDescending(x => x.UserLicense.Created)
                .AsNoTracking()
                .Where(x => x.UserLicense.UserId == CurrentUserId)
                .Select(x => new UserLicenseRDto
                {
                    Id = x.UserLicense.Id,
                    Name = x.License.Name,
                    ActivationKey = x.UserLicense.ActivationKey,
                    TradingAccount = x.UserLicense.TradingAccount
                });

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получить купленную лицензию
        /// </summary>
        /// <returns></returns>
        [HttpGet("licenses/{userLicenseId}")]
        [ProducesResponseType(200, Type = typeof(UserLicenseRDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetUserLicense(Guid userLicenseId)
        {
            var query = _db.UserLicenses
                .Join(_db.Licenses, ul => ul.LicenseId, l => l.Id, (ul, l) => new { UserLicense = ul, License = l })
                .OrderByDescending(x => x.UserLicense.Created)
                .AsNoTracking()
                .Where(x => x.UserLicense.UserId == CurrentUserId && x.UserLicense.Id == userLicenseId)
                .Select(x => new UserLicenseRDto
                {
                    Id = x.UserLicense.Id,
                    Name = x.License.Name,
                    ActivationKey = x.UserLicense.ActivationKey,
                    TradingAccount = x.UserLicense.TradingAccount
                });

            var userLicense = await query.FirstOrDefaultAsync();
            if (userLicense == null)
                return NotFound($"Не найдена лицензия пользователя {userLicenseId}");

            return Ok(userLicense);
        }

        /// <summary>
        /// Указать номер торгового счета для лицензии - через какой счет бот торгует на ней.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("licenses/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> Update([FromRoute, Required] Guid id, [FromBody] ChangeUserLicenseBinding model)
        {
            var userLicense = _db.UserLicenses.FirstOrDefault(x => x.Id == id);
            if (userLicense == null)
                return NotFound($"Не найдена лицензия пользователя {id}");

            if (userLicense.UserId != CurrentUserId)
                return ValidationProblem($"Лицензия не принадлежит текущему пользователю");

            if (userLicense.TradingAccount == model.TradingAccount)
                return Ok();

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userLicense.UserId);
            if (user == null)
                return NotFound($"Не найден пользователь {userLicense.UserId}");

            var license = await _db.Licenses.FirstOrDefaultAsync(x => x.Id == userLicense.LicenseId);
            if (license == null)
                return NotFound($"Не найдена лицензия {userLicense.LicenseId}");

            userLicense.TradingAccount = model.TradingAccount;

            try
            {
                await _darkbandRu.UpdateAccountNumber(int.Parse(userLicense.ActivationKey), userLicense.TradingAccount);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Exc");
                return ValidationProblem("Не удалось указать номер торгового счета для лицензии");
            }

            await _emailService.SendUserLicenseChanged(user.Id, user.Email, license.Id, license.Name,
                userLicense.ActivationKey, userLicense.TradingAccount);

            return Ok();
        }


        /// <summary>
        /// Получить список всех купленных VDS серверов
        /// </summary>
        /// <returns></returns>
        [HttpGet("vds-servers")]
        [ProducesResponseType(200, Type = typeof(List<UserServerRDto>))]
        public async Task<IActionResult> GetAllServers()
        {
            var query = _db.UserServers
                .Join(_db.Servers, us => us.ServerId, s => s.Id, (us, s) => new { UserServer = us, Server = s })
                .OrderByDescending(x => x.UserServer.Created)
                .AsNoTracking()
                .Where(x => x.UserServer.UserId == CurrentUserId)
                .Select(x => new UserServerRDto
                {
                    Id = x.UserServer.Id,
                    Name = x.Server.Name,
                    Login = x.UserServer.Login,
                    Password = x.UserServer.Password,
                    Address = x.UserServer.Address
                });

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получить купленный VDS сервер
        /// </summary>
        /// <returns></returns>
        [HttpGet("vds-servers/{userServerId}")]
        [ProducesResponseType(200, Type = typeof(UserServerRDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetUserServer(Guid userServerId)
        {
            var query = _db.UserServers
                .Join(_db.Servers, us => us.ServerId, s => s.Id, (us, s) => new { UserServer = us, Server = s })
                .OrderByDescending(x => x.UserServer.Created)
                .AsNoTracking()
                .Where(x => x.UserServer.UserId == CurrentUserId && x.UserServer.Id == userServerId)
                .Select(x => new UserServerRDto
                {
                    Id = x.UserServer.Id,
                    Name = x.Server.Name,
                    Login = x.UserServer.Login,
                    Password = x.UserServer.Password,
                    Address = x.UserServer.Address
                });

            var userServer = await query.FirstOrDefaultAsync();
            if (userServer == null)
                return NotFound($"Не найден VDS сервер пользователя {userServerId}");

            return Ok(userServer);
        }

        [HttpPut("vds-server/reboot/{userServerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> RebootVdsServer(Guid userServerId)
        {
            var us = await _db.UserServers.FirstOrDefaultAsync(x => x.Id == userServerId);
            if (us == null)
                new { userServerId }.ThrowApplicationException("Сервер не найден!");
            if (!int.TryParse(us.ExternalId, out var vpsId))
                new { us, userServerId }.ThrowApplicationException("У сервера не указана vps_id! Обратитесь к администратору с просьбой установить vps_id"); ;
            await _darkbandRu.RebootVps(vpsId);
            return Ok();
        }


        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Current user is not defined"));
    }
}
