using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models.Entities;

namespace MoviesAPI.Models.Services.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // For Identity
            base.OnModelCreating(builder);

            builder.Entity<Movie>(entity =>
            {
                // Many-to-many relationships (join classes are manually created)
                entity.HasMany(movie => movie.Genres)
                      .WithMany(genre => genre.Movies)
                      .UsingEntity<MovieGenre>(entity =>
                      {
                          entity.ToTable("MovieGenres");
                      });

                entity.HasMany(movie => movie.MovieTheaters)
                      .WithMany(movieTheater => movieTheater.Movies)
                      .UsingEntity<MovieMovieTheater>(entity =>
                      {
                          entity.ToTable("MovieMovieTheaters");
                      });

                entity.HasMany(movie => movie.Actors)
                      .WithMany(actor => actor.Movies)
                      .UsingEntity<MovieActor>(entity =>
                      {
                          entity.ToTable("MovieActors");
                      });
            });
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieTheater> MovieTheaters { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieMovieTheater> MovieMovieTheaters { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
    }
}