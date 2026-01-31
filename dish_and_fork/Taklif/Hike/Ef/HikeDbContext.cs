using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Options;
using Hike.Entities;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace Hike.Ef
{
    /// <summary>
    /// Контекст данных
    /// </summary>
    public sealed class HikeDbContext : ApiAuthorizationDbContext<HikeUser>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HikeDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IHttpContextAccessor contextAccessor
            ) : base(options, operationalStoreOptions)
        {
            _contextAccessor = contextAccessor;
            ChangeTracker.StateChanged += OnEntityStateChanged;
            ChangeTracker.Tracked += OnEntityTracked;
        }

        public DbSet<FileDto> Files { get; set; }
        public DbSet<MerchandiseDto> Merchandises { get; set; }
        public DbSet<MerchandiseImageDto> MerchandiseImages { get; set; }
        public DbSet<CategoryDto> Categories { get; set; }
        public DbSet<MerchandiseCategoryDto> MerchandiseCategories { get; set; }
        public DbSet<OrderDto> Orders { get; set; }
        public DbSet<OrderItemDto> OrderItems { get; set; }
        public DbSet<PartnerDto> Partners { get; set; }
        public DbSet<PartnerUser> PartnerUsers { get; set; }
        public DbSet<ApplicationDto> Applications { get; set; }
        public DbSet<OfferToApplicationDto> ApplicationOffers { get; set; }
        public DbSet<ChatMessageDto> Comments { get; set; }
        public DbSet<RatingDto> Ratings { get; set; }
        public DbSet<DeviceDto> Devices { get; set; }
        public DbSet<UserFileDto> UserFiles { get; set; }
        public DbSet<ShopDto> Shops { get; set; }
        public DbSet<CollectionDto> Collections { get; set; }
        public DbSet<CollectionCategoryDto> CollectionCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(HikeDbContext).Assembly);
            builder
                .Entity<HikeUser>()
                .HasIndex(x => x.NormalizedEmail)
                .IsUnique();
            builder.Entity<FileDto>()
                .HasIndex(x => x.Hash)
                .IsUnique();
            builder
                .Model
                .GetEntityTypes()
                .Where(e => string.IsNullOrWhiteSpace(e.GetViewName()))
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))
                .ToList()
                .ForEach(p =>
                {
                    p.SetPrecision(28);
                    p.SetScale(18);
                });
            builder.Entity<MerchandiseDto>()
                .HasIndex(x => x.NormalizedTitle)
                .IsUnique(false);
            builder.Entity<MerchandiseImageDto>()
                .HasKey(x => new { x.MerchandiseId, x.FileId });
            builder.Entity<CategoryDto>()
                .HasIndex(x => new { x.NormalizedTitle })
                .IsUnique();
            builder.Entity<MerchandiseCategoryDto>()
                .HasKey(x => new { x.MerchandiseId, x.CategoryId });
            builder.Entity<OrderItemDto>()
                .HasKey(x => new { x.OrderId, x.Id });
            builder.Entity<OrderDto>()
                .HasOne(x => x.Buyer)
                .WithMany(x => x.OrdersAsBuyer);
            builder.Entity<OrderDto>().Property(x => x.Number).ValueGeneratedOnAdd();
            builder.Entity<OrderDto>().HasIndex(x => x.Number).IsUnique();
            builder.Entity<PartnerUser>()
                .HasKey(x => new { x.PartnerId, x.UserId });
            builder.Entity<PartnerUser>().HasIndex(x => x.UserId).IsUnique();
            builder.Entity<PartnerDto>()
                .HasIndex(x => x.Inn)
                .IsUnique();
            builder.Entity<PartnerDto>()
                .HasIndex(x => x.ExternalId)
                .HasFilter(@$"TRIM(coalesce(""{nameof(PartnerDto.ExternalId)}"", '')) <> ''")
                .IsUnique();
            builder.Entity<ApplicationDto>()
                .HasIndex(x => x.NormalizedTitle)
                .IsUnique(false);
            builder.Entity<OfferToApplicationDto>().Property(x => x.Number).ValueGeneratedOnAdd();
            builder.Entity<OfferToApplicationDto>().HasIndex(x => x.Number).IsUnique();
            builder.Entity<ApplicationDto>().Property(x => x.Number).ValueGeneratedOnAdd();
            builder.Entity<ApplicationDto>().HasIndex(x => x.Number).IsUnique();
            builder.Entity<ChatMessageDto>()
                .HasOne(x => x.Parent)
                .WithMany(x => x.Descendants)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<ChatMessageDto>()
              .HasOne(x => x.Offer)
              .WithMany(x => x.Comments)
              .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<OfferToApplicationDto>()
                .HasOne(x => x.Application)
                .WithMany(x => x.Offers)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<OrderItemDto>()
                .HasOne(x => x.Offer)
                .WithMany(x => x.OrderItems)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<OrderItemDto>()
            .HasOne(x => x.Item)
            .WithMany(x => x.OrderItems)
            .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<CollectionCategoryDto>()
             .HasKey(x => new { x.CategoryId, x.CollectionId });
            builder.Entity<OfferToApplicationImageDto>()
           .HasKey(x => new { x.FileId, x.OfferToApplicationId });
            builder.Entity<MerchandiseCompositionRequester>()
                .HasKey(x => new { x.MerchandiseId, x.UserId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            base.OnConfiguring(optionsBuilder);
        }

        private void OnEntityTracked(object sender, EntityTrackedEventArgs e)
        {
            if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is DbDtoBase entity)
            {
                var entry = e.Entry;
                entity.CreatedById = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                entity.Created = DateTime.UtcNow;
                entity.ConcurrencyToken = Guid.NewGuid().ToString("D");
                if (entry.Entity is EntityDtoBase bewi && bewi.Id == Guid.Empty)
                    bewi.Id = Guid.NewGuid();
                entry.State = EntityState.Added;
            }
        }

        private void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            if (e.Entry.Entity is DbDtoBase entity && e.Entry.State == EntityState.Modified)
            {
                var entry = e.Entry;
                var oldToken = entity.ConcurrencyToken;
                var newToken = Guid.NewGuid().ToString("D");
                entity.ConcurrencyToken = newToken;
                entry.CurrentValues[nameof(DbDtoBase.ConcurrencyToken)] = newToken;
                entry.OriginalValues[nameof(DbDtoBase.ConcurrencyToken)] = oldToken;
                entity.Updated = DateTime.UtcNow;
                entity.UpdatedById = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

        public async Task<List<(decimal Proportion, string UserId)>> GetSpendMoneyProportionToIndividaulOrders(Guid partnerId)
        {
            var sum = await this.Orders
                     .Where(o => o.SellerInfo.Id == partnerId)
                     .Where(x => x.Type == OrderType.Individual)
                     .SumAsync(o => o.Amount.Value);
            var dbos = await this.Orders
                 .AsNoTracking()
                 .Where(x => x.SellerInfo.Id == partnerId)
                  .Where(x => x.Type == OrderType.Individual)
                 .GroupBy(x => x.BuyerId)
                 .Select(x => new
                 {
                     UserId = x.Key,
                     Proportion = x.Sum(y => y.Amount.Value) / sum
                 })
                 .ToListAsync();
            return dbos.Select(x => (x.Proportion, x.UserId))
            .ToList();
        }

        public async Task<List<(decimal Proportion, string UserId)>> GetSpendMoneyProportionToStandartOrders(Guid merchId)
        {
            var sum = await this.Orders
                      .Where(x => x.Items.Any(i => i.ItemId == merchId))
                     .SumAsync(o => o.Amount.Value);
            var dbos = await this.Orders
            .AsNoTracking()
                 .Where(x => x.Items.Any(i => i.ItemId == merchId))
                 .GroupBy(x => x.BuyerId)
                 .Select(x => new
                 {
                     UserId = x.Key,
                     Proportion = x.Sum(y => y.Amount.Value) / sum
                 })
                 .ToListAsync();
            return dbos.Select(x => (x.Proportion, x.UserId))
        .ToList();
        }

        public async Task<List<(double Stars, string UserId)>> GetMerhStars(Guid merchId)
        {
            var dtos = await this.Ratings
              .Where(x => x.MerchandiseId == merchId)
              .Select(x => new { x.Rating, x.EvaluatorId })
              .ToListAsync();
            return dtos.Select(x => (x.Rating, x.EvaluatorId)).ToList();
        }

        public async Task<List<(double Stars, string UserId)>> GetPartnerStars(Guid partnerId)
        {
            var dtos = await this.Ratings
              .Where(x => x.ShopId == partnerId)
              .Select(x => new { x.Rating, x.EvaluatorId })
              .ToListAsync();
            return dtos.Select(x => (x.Rating, x.EvaluatorId)).ToList();
        }

        public async Task<double> GetMerchRating(Guid merchId)
        {
            var stars = await GetMerhStars(merchId);
            var proportion = await GetSpendMoneyProportionToStandartOrders(merchId);
            return CalculateRating(stars, proportion);
        }

        public async Task<double> GetPartnerRating(Guid partnerId)
        {
            var stars = await GetPartnerStars(partnerId);
            var proportion = await GetSpendMoneyProportionToIndividaulOrders(partnerId);
            return CalculateRating(stars, proportion);
        }

        public double CalculateRating(List<(double Stars, string UserId)> stars,
            List<(decimal Proportion, string UserId)> proportions)
        {
            var d = proportions.ToDictionary(x => x.UserId, y => y.Proportion);

            var sum = stars.Select(x =>
            {
                if (d.ContainsKey(x.UserId))
                    return x.Stars * (double)d[x.UserId];
                return 0;
            }).Sum();
            return sum;
        }

    }
}
