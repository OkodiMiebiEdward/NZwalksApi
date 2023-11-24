using NZWalksAPI.Models.Domain;
using System.Globalization;

namespace NZWalksAPI.Models.DTO
{
    public class WalkDto
    {
        //The Model to be displayed to the User
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        /*Because We Have The Region And Difficulty 
        * Model Theres Actually No need for the
        Id's*/

        //public Guid DifficultyId { get; set; }
        //public Guid RegionId { get; set; }
        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
    }
}
