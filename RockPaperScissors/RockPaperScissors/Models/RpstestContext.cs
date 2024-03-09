using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RockPaperScissors;

public partial class RpstestContext : DbContext
{
    public RpstestContext()
    {
    }

    public RpstestContext(DbContextOptions<RpstestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GameTransaction> GameTransactions { get; set; }

    public virtual DbSet<MatchHistory> MatchHistories { get; set; }

    public virtual DbSet<MatchResult> MatchResults { get; set; }

    public virtual DbSet<ResultOption> ResultOptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=rpstest;Username=postgres;Password=qwerty");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_transactions_pkey");

            entity.ToTable("game_transactions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.RecieverId).HasColumnName("reciever_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("transaction_date");

            entity.HasOne(d => d.Reciever).WithMany(p => p.GameTransactionRecievers)
                .HasForeignKey(d => d.RecieverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_transactions_to_users2_fk");

            entity.HasOne(d => d.Sender).WithMany(p => p.GameTransactionSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("game_transactions_to_users1_fk");
        });

        modelBuilder.Entity<MatchHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("match_history_pkey");

            entity.ToTable("match_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bid)
                .HasPrecision(10, 2)
                .HasColumnName("bid");
            entity.Property(e => e.FirstUserId).HasColumnName("first_user_id");
            entity.Property(e => e.MatchDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("match_date");
            entity.Property(e => e.SecondUserId).HasColumnName("second_user_id");

            entity.HasOne(d => d.FirstUser).WithMany(p => p.MatchHistoryFirstUsers)
                .HasForeignKey(d => d.FirstUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("match_history_to_users1_fk");

            entity.HasOne(d => d.SecondUser).WithMany(p => p.MatchHistorySecondUsers)
                .HasForeignKey(d => d.SecondUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("match_history_to_users2_fk");
        });

        modelBuilder.Entity<MatchResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("match_results_pkey");

            entity.ToTable("match_results");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MatchId).HasColumnName("match_id");
            entity.Property(e => e.ResultOptionId)
                .HasMaxLength(50)
                .HasColumnName("result_option_id");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchResults)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("match_results_to_match_history_fk");

            entity.HasOne(d => d.ResultOption).WithMany(p => p.MatchResults)
                .HasForeignKey(d => d.ResultOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("match_results_to_result_options_fk");
        });

        modelBuilder.Entity<ResultOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("result_options_pkey");

            entity.ToTable("result_options");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasColumnName("balance");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
