using System;
using Newtonsoft.Json;

namespace Infrastructure.Models
{
    public class BaseEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

      /*   [JsonIgnore]
        public DateTime? CreatedDate { get; set; }

        [JsonIgnore]
        public DateTime? ModifiedDate { get; set; } */
    }
}