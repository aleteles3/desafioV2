using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.NoSql.UserToken;

public class UserRefreshToken
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("userId")]
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    
    [BsonElement("refreshToken")]
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
    
    [BsonElement("refreshTokenExpirationDate")]
    [JsonPropertyName("refreshTokenExpirationDate")]
    public DateTime RefreshTokenExpirationDate { get; set; }
}