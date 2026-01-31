using AleBotApi.DbDtos;
using ApeBotApi.DbDtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AleBotApi.DbContexts
{
    public class AbDbContext
         : IdentityDbContext<AbUserDbDto, AbRoleDbDto, Guid, UserClaimDbDto, UserRoleDbDto, UserLoginDbDto, RoleClaimDbDto, UserTokenDbDto>
    {
        public AbDbContext(DbContextOptions<AbDbContext> options) :
            base(options)
        {
            ChangeTracker.StateChanged += OnEntityStateChanged;
            ChangeTracker.Tracked += OnEntityTracked;
        }

        public DbSet<AbAccountDbDto> Accounts { get; set; } = null!;
        public DbSet<AbAccountTransactionDbDto> AccountTransactions { get; set; } = null!;
        public DbSet<AbPaymentSystemDbDto> PaymentSystems { get; set; } = null!;
        public DbSet<AbPaymentNetworkDbDto> PaymentNetworks { get; set; } = null!;
        public DbSet<AbCurrencyDbDto> Currencies { get; set; } = null!;
        public DbSet<AbCourseDbDto> Courses { get; set; } = null!;
        public DbSet<AbLessonDbDto> Lessons { get; set; } = null!;
        public DbSet<AbUserCourseDbDto> UserCourses { get; set; } = null!;
        public DbSet<AbLicenseDbDto> Licenses { get; set; } = null!;
        public DbSet<AbUserLicenseDbDto> UserLicenses { get; set; } = null!;
        public DbSet<AbServerDbDto> Servers { get; set; } = null!;
        public DbSet<AbUserServerDbDto> UserServers { get; set; } = null!;
        public DbSet<FileDbDto> Files { get; set; } = null!;
        public DbSet<AbMerchDbDto> Merches { get; set; } = null!;
        public DbSet<AbMerchCourseDbDto> MerchCourses { get; set; } = null!;
        public DbSet<AbMerchLicenseDbDto> MerchLicenses { get; set; } = null!;
        public DbSet<AbMerchServerDbDto> MerchServers { get; set; } = null!;
        public DbSet<AbOrderDbDto> Orders { get; set; } = null!;
        public DbSet<AbOrderLineDbDto> OrderLines { get; set; } = null!;
        public DbSet<AbServerExtentionDto> ServerExtentions { get; set; } = null!;
        public DbSet<AbMerchServerExtentionsDbDto> MerchServerExtentions { get; set; } = null!;
        public DbSet<AbEventOrSagaDbDto> EventsAndSagas { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AbUserDbDto>()
                .HasMany(x => x.Roles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.Entity<AbUserDbDto>()
                .HasMany(x => x.Claims)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.Entity<AbUserDbDto>()
                .HasMany(x => x.Logins)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.Entity<AbUserDbDto>()
                .HasMany(x => x.Tokens)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.Entity<AbRoleDbDto>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();

            builder.Entity<AbRoleDbDto>()
                .HasMany(x => x.Claims)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();
            builder.Entity<AbUserServerDbDto>()
                .HasMany(x => x.OrderLinesToExtend)
                .WithOne(x => x.ExtendedUserServer)
                .HasForeignKey(x => x.ExtendedUserServerId)
                .IsRequired(false);
        }

        private void OnEntityTracked(object? sender, EntityTrackedEventArgs e)
        {
            if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is DbDtoBase entity)
            {
                var entry = e.Entry;
                entity.Created = DateTime.UtcNow;
                entity.ConcurrencyToken = Guid.NewGuid().ToString("D");
                if (entry.Entity is EntityDbDtoBase bewi && bewi.Id == Guid.Empty)
                    bewi.Id = Guid.NewGuid();
                entry.State = EntityState.Added;
            }
        }

        private void OnEntityStateChanged(object? sender, EntityStateChangedEventArgs e)
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
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
#endif
            base.OnConfiguring(optionsBuilder);

        }
    }
}
