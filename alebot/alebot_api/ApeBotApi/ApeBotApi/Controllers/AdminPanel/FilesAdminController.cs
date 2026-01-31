using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Файлы
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/files-admin")]
    public class FilesAdminController : ControllerBase
    {
        private readonly ILogger<FilesAdminController> _logger;
        private readonly AbDbContext _db;
        private readonly IWebHostEnvironment _environment;
        private readonly IHostApplicationLifetime _lifetime;

        public FilesAdminController(
            ILogger<FilesAdminController> logger,
            AbDbContext db,
            IWebHostEnvironment environment,
            IHostApplicationLifetime lifetime
            )
        {
            _logger = logger;
            _db = db;
            _environment = environment;
            _lifetime = lifetime;
        }

        /// <summary>
        /// Загрузить файл на сервер
        /// </summary>
        [HttpPost("upload")]
        [ProducesResponseType(200, Type = typeof(FileReadModel))]
        public async Task<FileReadModel?> Upload(IFormFile file)
        {
            Request.EnableBuffering();
            using var stream = file.OpenReadStream();
            stream.Position = 0;
            using var md5 = MD5.Create();
            var hash = await md5.ComputeHashAsync(stream);
            var oldFile = await _db.Files.FirstOrDefaultAsync(x => x.Hash == hash);
            if (oldFile != null)
                return FileReadModel.From(oldFile, GetPath(oldFile));
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension))
                extension = ".file";
            var dir = Path.Combine(_environment.WebRootPath, "files", FileReadModel.GetStr(hash));
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, $"file{extension}");
            var size = await SaveFile(stream, path);
            var newFile = new FileDbDto
            {
                Id = new Guid(hash),
                Name = file.FileName,
                Hash = hash,
                Size = size,
                ContentType = file.ContentType,
                Extention = extension
            };
            _db.Files.Add(newFile);
            await _db.SaveChangesAsync(_lifetime.ApplicationStopping);
            return FileReadModel.From(newFile, GetPath(newFile));
        }

        /// <summary>
        /// Получить список файлов
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PageResultModel<FileReadModel>))]
        public async Task<PageResultModel<FileReadModel>> GetAll([FromQuery] PaginationModel model)
        {
            var count = await _db.Files.CountAsync();
            var files = await _db.Files.OrderByDescending(x => x.Created).Skip((int)model.Skip()).Take((int)model.PageSize).ToListAsync();
            return new PageResultModel<FileReadModel>
            {
                TotalCount = count,
                Items = files.Select(x => FileReadModel.From(x, GetPath(x))).ToList()
            };
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(FileReadModel))]
        public async Task<FileReadModel> Get([FromRoute, Required] Guid id)
        {
            var file = await _db.Files.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return FileReadModel.From(file, GetPath(file));
        }

        private async Task<long> SaveFile(Stream stream, string path)
        {
            if (!System.IO.File.Exists(path))
            {
                using var fileStream = System.IO.File.Create(path, 80_000_000);
                stream.Position = 0;
                await stream.CopyToAsync(fileStream, _lifetime.ApplicationStopping);
                await fileStream.FlushAsync(_lifetime.ApplicationStopping);
                return fileStream.Length;
            }
            else
            {
                return stream.Length;
            }
        }



        private string GetPath(FileDbDto? dto) => dto == null ? null :
            $"{GetBasePath()?.Trim().TrimEnd('/')}/files/{FileReadModel.GetStr(dto.Hash)}/{FileReadModel.GetName(dto)}";

        private string GetBasePath() => $"{(Request.Host.Port == 5240 ? "http" : Request.Scheme)}://{Request.Host}{Request.PathBase}";
    }

    public class PaginationModel
    {
        private const uint MAX_ITEMS_PER_PAGE = 1000;

        /// <summary>
        /// Номер страницы (начиная с 1)
        /// </summary>
        [Range(1, uint.MaxValue)]
        public uint PageNumber { get; set; } = 1;

        /// <summary>
        /// Размер страницы (максимум 1000)
        /// </summary>
        [Range(1, MAX_ITEMS_PER_PAGE)]
        public uint PageSize { get; set; } = MAX_ITEMS_PER_PAGE;

        public uint Skip() => (PageNumber - 1) * PageSize;
    }

    public class PageResultModel<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }

    public class FileReadModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Path { get; set; }
        public string Extention { get; set; }

        public static FileReadModel From(FileDbDto? dto, string path)
        {
            if (dto == null)
                return null;
            return new FileReadModel
            {
                Id = dto.Id,
                Name = dto.Name,
                ContentType = dto.ContentType,
                Created = dto.Created,
                Hash = GetStr(dto.Hash),
                Size = dto.Size,
                Updated = dto.Updated,
                Path = path,
                Extention = dto.Extention,
            };
        }

        public static string GetName(FileDbDto? dto)
        {
            var fileName = "file";
            var extension = dto?.Extention;
            if (string.IsNullOrWhiteSpace(extension))
                extension = ".file";
            return fileName + extension;
        }

        public static string GetStr(byte[] hash) => new Guid(hash).ToString("N");
    }
}
