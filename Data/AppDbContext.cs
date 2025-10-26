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
        public DbSet<Faskes> Faskes { get; set; }
        public DbSet<Klaim> Klaim { get; set; }
        public DbSet<Aduan> Aduan { get; set; }
        public DbSet<LoginActivity> LoginActivities { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; } // ✅ SECURITY ENHANCEMENT

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

            // Table: faskes
            modelBuilder.Entity<Faskes>(entity =>
            {
                entity.ToTable("faskes");
                entity.HasKey(e => e.IdFaskes);

                entity.Property(e => e.IdFaskes).HasColumnName("id_faskes");
                entity.Property(e => e.Nama).HasColumnName("nama").HasMaxLength(150);
                entity.Property(e => e.Tipe).HasColumnName("tipe").HasMaxLength(50);
                entity.Property(e => e.Alamat).HasColumnName("alamat");
                entity.Property(e => e.Kota).HasColumnName("kota").HasMaxLength(100);
                entity.Property(e => e.Provinsi).HasColumnName("provinsi").HasMaxLength(100);
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasIndex(e => new { e.Status, e.Kota });
            });

            // Table: klaim
            modelBuilder.Entity<Klaim>(entity =>
            {
                entity.ToTable("klaim");
                entity.HasKey(e => e.IdKlaim);

                entity.Property(e => e.IdKlaim).HasColumnName("id_klaim");
                entity.Property(e => e.IdBpjs).HasColumnName("id_bpjs");
                entity.Property(e => e.IdFaskes).HasColumnName("id_faskes");
                entity.Property(e => e.TanggalPengajuan).HasColumnName("tanggal_pengajuan");
                entity.Property(e => e.Diagnosis).HasColumnName("diagnosis");
                entity.Property(e => e.Tindakan).HasColumnName("tindakan");
                entity.Property(e => e.BiayaDiajukan).HasColumnName("biaya_diajukan");
                entity.Property(e => e.BiayaDisetujui).HasColumnName("biaya_disetujui");
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20);
                entity.Property(e => e.Catatan).HasColumnName("catatan");
                entity.Property(e => e.CreatedByUserId).HasColumnName("created_by");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(e => e.Bpjs)
                      .WithMany()
                      .HasForeignKey(e => e.IdBpjs)
                      .HasPrincipalKey(b => b.IdBpjs);

                entity.HasOne(e => e.Faskes)
                      .WithMany()
                      .HasForeignKey(e => e.IdFaskes)
                      .HasPrincipalKey(f => f.IdFaskes);

                entity.HasOne(e => e.CreatedBy)
                      .WithMany()
                      .HasForeignKey(e => e.CreatedByUserId)
                      .HasPrincipalKey(u => u.Id);

                entity.HasIndex(e => new { e.Status, e.TanggalPengajuan });
                entity.HasIndex(e => e.IdBpjs);
                entity.HasIndex(e => e.IdFaskes);
            });

            // Table: aduan
            modelBuilder.Entity<Aduan>(entity =>
            {
                entity.ToTable("aduan");
                entity.HasKey(e => e.IdAduan);

                entity.Property(e => e.IdAduan).HasColumnName("id_aduan");
                entity.Property(e => e.IdBpjs).HasColumnName("id_bpjs");
                entity.Property(e => e.NamaPengadu).HasColumnName("nama_pengadu").HasMaxLength(150);
                entity.Property(e => e.NoKartu).HasColumnName("no_kartu").HasMaxLength(30);
                entity.Property(e => e.Kategori).HasColumnName("kategori").HasMaxLength(50);
                entity.Property(e => e.Deskripsi).HasColumnName("deskripsi");
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20);
                entity.Property(e => e.Prioritas).HasColumnName("prioritas").HasMaxLength(20);
                entity.Property(e => e.AssignedToUserId).HasColumnName("assigned_to");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.TanggalSelesai).HasColumnName("tanggal_selesai");

                entity.HasOne(e => e.Bpjs)
                      .WithMany()
                      .HasForeignKey(e => e.IdBpjs)
                      .HasPrincipalKey(b => b.IdBpjs);

                entity.HasOne(e => e.AssignedTo)
                      .WithMany()
                      .HasForeignKey(e => e.AssignedToUserId)
                      .HasPrincipalKey(u => u.Id);

                entity.HasIndex(e => new { e.Status, e.CreatedAt });
                entity.HasIndex(e => e.AssignedToUserId);
            });

            // Table: login_activities
            modelBuilder.Entity<LoginActivity>(entity =>
            {
                entity.ToTable("login_activities");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(100);
                entity.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(50);
                entity.Property(e => e.Device).HasColumnName("device").HasMaxLength(200);
                entity.Property(e => e.Browser).HasColumnName("browser").HasMaxLength(100);
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20);
                entity.Property(e => e.LoginTime).HasColumnName("login_time");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .HasPrincipalKey(u => u.Id)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => new { e.Username, e.LoginTime });
                entity.HasIndex(e => e.UserId);
            });

            // ✅ Table: refresh_tokens
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_tokens");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Token).HasColumnName("token").HasMaxLength(500);
                entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
                entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
                entity.Property(e => e.CreatedByIp).HasColumnName("created_by_ip").HasMaxLength(50);
                entity.Property(e => e.CreatedByDevice).HasColumnName("created_by_device").HasMaxLength(200);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .HasPrincipalKey(u => u.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.Token).IsUnique();
                entity.HasIndex(e => new { e.UserId, e.IsRevoked });
            });
        }
    }
}