using JodelAPI.Json;

namespace JodelAPI.Objects
{
    public class MyJodels : Jodels
    {
        internal MyJodels(JsonMyJodels.Post jsonMyJodel) : base(jsonMyJodel)
        {
            Latitude = jsonMyJodel.location.loc_coordinates.lat;
            Longitude = jsonMyJodel.location.loc_coordinates.lng;
        }

        public int Latitude { get; set; }
        public int Longitude { get; set; }
    }
}