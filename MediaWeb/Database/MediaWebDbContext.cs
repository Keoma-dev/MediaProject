using MediaWeb.Domain;
using MediaWeb.Domain.Movie;
using MediaWeb.Domain.Music;
using MediaWeb.Domain.Podcast;
using MediaWeb.Domain.TVShow;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Database
{
    public class MediaWebDbContext : IdentityDbContext<MediaWebUser>
    {
    
    public MediaWebDbContext(DbContextOptions<MediaWebDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Many to Many Movie & Actor

        builder.Entity<MovieActor>()
            .HasKey(ma => new { ma.MovieId, ma.ActorId });

        builder.Entity<MovieActor>()
            .HasOne(ma => ma.Movie)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(ma => ma.MovieId);

        builder.Entity<MovieActor>()
            .HasOne(ma => ma.Actor)
            .WithMany(a => a.MovieActors)
            .HasForeignKey(ma => ma.ActorId);

        //Many to Many Movie & Director

        builder.Entity<MovieDirector>()
            .HasKey(md => new { md.MovieId, md.DirectorId });

        builder.Entity<MovieDirector>()
            .HasOne(md => md.Movie)
            .WithMany(m => m.MovieDirectors)
            .HasForeignKey(md => md.MovieId);

        builder.Entity<MovieDirector>()
            .HasOne(md => md.Director)
            .WithMany(d => d.MovieDirectors)
            .HasForeignKey(md => md.DirectorId);

        //Many to Many Movie & Genre

        builder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieId, mg.GenreId });

        builder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId);

        builder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId);

        // Many to Many Song & Artist

        builder.Entity<SongArtist>()
            .HasKey(sa => new { sa.SongId, sa.ArtistId });

        builder.Entity<SongArtist>()
            .HasOne(sa => sa.Song)
            .WithMany(s => s.SongArtists)
            .HasForeignKey(sa => sa.SongId);

        builder.Entity<SongArtist>()
            .HasOne(sa => sa.Artist)
            .WithMany(a => a.SongArtists)
            .HasForeignKey(sa => sa.ArtistId);

        // Many to Many Podcast & Host

        builder.Entity<PodcastHost>()
            .HasKey(ph => new { ph.PodcastId, ph.HostId });

        builder.Entity<PodcastHost>()
            .HasOne(ph => ph.Podcast)
            .WithMany(p => p.PodCastHosts)
            .HasForeignKey(ph => ph.PodcastId);

        builder.Entity<PodcastHost>()
            .HasOne(ph => ph.Host)
            .WithMany(h => h.PodCastHosts)
            .HasForeignKey(ph => ph.HostId);

        // Many to Many Podcast & Guest

        builder.Entity<PodcastGuest>()
           .HasKey(pg => new { pg.PodcastId, pg.GuestId });

        builder.Entity<PodcastGuest>()
            .HasOne(pg => pg.Podcast)
            .WithMany(p => p.PodCastGuests)
            .HasForeignKey(ph => ph.PodcastId);

        builder.Entity<PodcastGuest>()
            .HasOne(pg => pg.Guest)
            .WithMany(g => g.PodCastGuests)
            .HasForeignKey(pg => pg.GuestId);

        // Many to Many TVShow & Actor

        builder.Entity<TVShowActor>()
            .HasKey(ta => new { ta.TVShowId, ta.ActorId });

        builder.Entity<TVShowActor>()
            .HasOne(ta => ta.TVShow)
            .WithMany(t => t.TVShowActors)
            .HasForeignKey(ta => ta.TVShowId);

        builder.Entity<TVShowActor>()
            .HasOne(ta => ta.Actor)
            .WithMany(a => a.TVShowActors)
            .HasForeignKey(ta => ta.ActorId);

        // Many to Many TVShow & Director

        builder.Entity<TVShowDirector>()
            .HasKey(td => new { td.TVShowId, td.DirectorId });

        builder.Entity<TVShowDirector>()
            .HasOne(td => td.TVShow)
            .WithMany(t => t.TVShowDirectors)
            .HasForeignKey(td => td.TVShowId);

        builder.Entity<TVShowDirector>()
            .HasOne(td => td.Director)
            .WithMany(d => d.TVShowDirectors)
            .HasForeignKey(td => td.DirectorId);

        // Many to Many TvShow & Genre

        builder.Entity<TVShowGenre>()
            .HasKey(tg => new { tg.TVShowId, tg.GenreId });

        builder.Entity<TVShowGenre>()
            .HasOne(tg => tg.TVShow)
            .WithMany(t => t.TVShowGenres)
            .HasForeignKey(tg => tg.TVShowId);

        builder.Entity<TVShowGenre>()
            .HasOne(tg => tg.Genre)
            .WithMany(g => g.TVShowGenres)
            .HasForeignKey(tg => tg.GenreId);

        // Many to Many Episode & Actor

        builder.Entity<EpisodeActor>()
            .HasKey(ea => new { ea.EpisodeId, ea.ActorId });

        builder.Entity<EpisodeActor>()
            .HasOne(ea => ea.Episode)
            .WithMany(e => e.EpisodeActors)
            .HasForeignKey(ea => ea.EpisodeId);

        builder.Entity<EpisodeActor>()
            .HasOne(ea => ea.Actor)
            .WithMany(a => a.EpisodeActors)
            .HasForeignKey(ea => ea.ActorId);

        // Many to Many Episode & Director

        builder.Entity<EpisodeDirector>()
           .HasKey(ed => new { ed.EpisodeId, ed.DirectorId });

        builder.Entity<EpisodeDirector>()
            .HasOne(ed => ed.Episode)
            .WithMany(e => e.EpisodeDirectors)
            .HasForeignKey(ed => ed.EpisodeId);

        builder.Entity<EpisodeDirector>()
            .HasOne(ed => ed.Director)
            .WithMany(d => d.EpisodeDirectors)
            .HasForeignKey(ed => ed.DirectorId);
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<PodCast> PodCasts { get; set; }
    public DbSet<Host> Hosts { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<TVshow> TVShows { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<PlayList> PlayLists { get; set; }

}
}
