using Pgvector;

namespace Scraper.Db;

public record EpisodeRow(
    int? Id,
    int SeasonNumber,
    int EpisodeNumber,
    string Title,
    string Summary,
    string Synopsis,
    Vector SynopsisEmbedding);