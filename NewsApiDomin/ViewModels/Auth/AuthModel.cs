using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NewsApiDomin.Models
{
    public class AuthModel
    {   public int? Id { get; set; }
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Roles { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}