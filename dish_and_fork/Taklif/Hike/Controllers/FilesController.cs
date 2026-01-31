using System.Data;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Models;
using Hike.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Path = System.IO.Path;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Hike.Controllers
{
    [ApiController]
    [Route("api/v1/files")]
    public sealed class FilesController : ControllerBase
    {
        private readonly HikeDbContext _db;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FilesController> _logger;

        public FilesController(HikeDbContext db, IWebHostEnvironment environment, ILogger<FilesController> logger)
        {
            _db = db;
            _environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// Загружает файл на сервер. На загруженное изображение будет наложена ватермарка во имя темных богов.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(Guid))]
        [RequestSizeLimit(600_000)]
        public async Task<Guid> Upload(IFormFile file)
        {
            Request.EnableBuffering();
            using var stream = file.OpenReadStream();
            stream.Seek(0, SeekOrigin.Begin);
            using var md5 = MD5.Create();
            var hash = await md5.ComputeHashAsync(stream);
            var oldFile = await _db.Files.FirstOrDefaultAsync(x => x.Hash == hash);
            if (oldFile != null)
                return oldFile.Id;
            var path = GetPath(hash);
            if (!Directory.Exists(path.dir))
                Directory.CreateDirectory(path.dir);
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension))
                extension = ".file";
            stream.Seek(0, SeekOrigin.Begin);
            await ClearMetadata(stream, path.path + extension);
            var newFile = new FileDto
            {
                Id = Guid.NewGuid(),
                Hash = hash,
                Size = file.Length,
                ContentType = file.ContentType,
                Extention = extension
            };
            _db.Files.Add(newFile);
            var uf = new UserFileDto
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Title = file.FileName,
                NormalizedTitle = file.FileName?.GetNormalizedName(),
                FileId = newFile.Id
            };
            _db.UserFiles.Add(uf);
            await _db.SaveChangesAsync();
            return newFile.Id;
        }

        private async Task ClearMetadata(Stream input, string path)
        {
            if (System.IO.File.Exists(path))
                return;
            using var image = await Image.LoadAsync(input);
            using var watermark = await Image.LoadAsync(Path.Combine(_environment.WebRootPath, "img", "watermark.png"));
            image.Metadata.ExifProfile = null;
            image.Metadata.IptcProfile = null;
            image.Metadata.XmpProfile = null;
            image.Mutate(x => x.DrawImage(watermark,
                new Point(image.Width - watermark.Width, image.Height - watermark.Height),
                1f
             ));
            await image.SaveAsync(path);
        }


        ///// <summary>
        /////Скачать файл с сервера. Так же можно скачать его по пути указанному в поле path информации о файле и лучше его использовать для этого.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //[ProducesResponseType(200, Type = typeof(byte[]))]
        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60 * 60 * 24 * 365)]
        //public async Task<IActionResult> Download(Guid id)
        //{
        //    var file = await _db.Files.FirstOrDefaultAsync(x => x.Id == id);
        //    if (file == null)
        //        throw new ApplicationException("Файл не найден")
        //        {
        //            Data = { ["id"] = id }
        //        };
        //    var path = GetPath(file.Hash);
        //    var stream = new FileStream(path.path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4_000_000);
        //    return File(stream, file.ContentType, file.Title, true);
        //  }

        /// <summary>
        ///  Получить список информаций о файлах на сервере. Скачать файл можно по пути укзананному в свойтве Path
        /// </summary>
        /// <param name="pageNumber">Номер старницы, начинаеться с 1</param>
        /// <param name="pageSize">Размер страницы (максимум 1000)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PageResultModel<FileReadModel>))]
        public async Task<PageResultModel<FileReadModel>> GetAll([FromQuery] PaginationModel model)
        {
            var count = await _db.Files.CountAsync();
            var files = await _db.Files.OrderByDescending(x => x.Created).Skip((int)model.Skip()).Take((int)model.PageSize).ToListAsync();
            return new PageResultModel<FileReadModel>
            {
                TotalCount = count,
                Items = files.Select(x => FileReadModel.From(x, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}")).ToList()
            };
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(FileReadModel))]
        public async Task<FileReadModel> Get([FromRoute, Required] Guid id)
        {
            var file = await _db.Files.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return FileReadModel.From(file, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
        }

        private (string dir, string path, string fileName) GetPath(byte[] hash)
        {
            var fileName = BitConverter.ToString(hash).Replace("-", "");
            var dir = Path.Combine(_environment.WebRootPath, "files");
            var path = Path.Combine(dir, fileName);
            return (dir, path, fileName);
        }
    }
}
