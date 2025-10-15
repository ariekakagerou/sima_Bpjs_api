using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Models;

namespace sima_bpjs_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Admin> Admin { get; set; }
        public DbSet<KtpKk> KtpKk { get; set; }
        public DbSet<Bpjs> Bpjs { get; set; }
        public DbSet<PembayaranBpjs> PembayaranBpjs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table: admin
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(255);
                entity.Property(e => e.Password).HasColumnName("password");
            });

            // Table: ktp_kk
            modelBuilder.Entity<KtpKk>(entity =>
            {
                entity.ToTable("ktp_kk");
                entity.HasKey(e => e.Nik);

                entity.Property(e => e.Nik).HasColumnName("nik").HasMaxLength(16);
                entity.Property(e => e.NoKk).HasColumnName("no_kk").HasMaxLength(16);
                entity.Property(e => e.NamaLengkap).HasColumnName("nama_lengkap").HasMaxLength(100);
                entity.Property(e => e.TempatLahir).HasColumnName("tempat_lahir").HasMaxLength(50);
                entity.Property(e => e.TanggalLahir).HasColumnName("tanggal_lahir");
                entity.Property(e => e.JenisKelamin).HasColumnName("jenis_kelamin").HasMaxLength(1);
                entity.Property(e => e.Alamat).HasColumnName("alamat");
                entity.Property(e => e.Agama).HasColumnName("agama").HasMaxLength(50);
                entity.Property(e => e.StatusPerkawinan).HasColumnName("status_perkawinan").HasMaxLength(50);
                entity.Property(e => e.Pekerjaan).HasColumnName("pekerjaan").HasMaxLength(100);
                entity.Property(e => e.Kewarganegaraan).HasColumnName("kewarganegaraan").HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            // Table: bpjs
            modelBuilder.Entity<Bpjs>(entity =>
            {
                entity.ToTable("bpjs");
                entity.HasKey(e => e.IdBpjs);

                entity.Property(e => e.IdBpjs).HasColumnName("id_bpjs");
                entity.Property(e => e.Nik).HasColumnName("nik").HasMaxLength(16);
                entity.Property(e => e.NoBpjs).HasColumnName("no_bpjs").HasMaxLength(20);
                entity.Property(e => e.FaskesUtama).HasColumnName("faskes_utama").HasMaxLength(100);
                entity.Property(e => e.KelasPerawatan).HasColumnName("kelas_perawatan");
                entity.Property(e => e.StatusPeserta).HasColumnName("status_peserta");
                entity.Property(e => e.TanggalDaftar).HasColumnName("tanggal_daftar");

                entity
                    .HasOne(e => e.KtpKk)
                    .WithMany(k => k.Bpjs)
                    .HasForeignKey(e => e.Nik)
                    .HasPrincipalKey(k => k.Nik);
            });

            // Table: pembayaran_bpjs
            modelBuilder.Entity<PembayaranBpjs>(entity =>
            {
                entity.ToTable("pembayaran_bpjs");
                entity.HasKey(e => e.IdPembayaran);

                entity.Property(e => e.IdPembayaran).HasColumnName("id_pembayaran");
                entity.Property(e => e.IdBpjs).HasColumnName("id_bpjs");
                entity.Property(e => e.Bulan).HasColumnName("bulan").HasMaxLength(20);
                entity.Property(e => e.Tahun).HasColumnName("tahun");
                entity.Property(e => e.Jumlah).HasColumnName("jumlah");
                entity.Property(e => e.StatusPembayaran).HasColumnName("status_pembayaran");
                entity.Property(e => e.TanggalBayar).HasColumnName("tanggal_bayar");

                entity
                    .HasOne(e => e.Bpjs)
                    .WithMany(b => b.Pembayaran)
                    .HasForeignKey(e => e.IdBpjs)
                    .HasPrincipalKey(b => b.IdBpjs);
            });

            // Table: users
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(100);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(255);
                entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(20);
                entity.Property(e => e.Nik).HasColumnName("nik").HasMaxLength(16);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Nik).IsUnique(false);
            });
        }
    }
}