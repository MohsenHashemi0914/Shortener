using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace ShortenerEndpoint.Models;

[Collection("UrlLinks")]
public sealed class UrlLink
{
    public ObjectId Id { get; set; }
    public required string DestinationUrl { get; set; }
    public required string ShortenCode { get; set; }
    public DateTime CreatedOn { get; set; }
}