using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Json.Response;

namespace JodelAPI.Shared
{
    public class JodelPost
    {
        #region Fields and Properties

        public string PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; }
        public int Discovered { get; set; }
        public int DiscoveredBy { get; set; }
        public int Distance { get; set; }
        public bool GotThanks { get; set; }
        public Location Place { get; set; }
        public bool NotificationsEnabled { get; set; }
        public int PinCounted { get; set; }
        public string PostOwn { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserHandle { get; set; }
        public int VoteCount { get; set; }
        public PostColor Color { get; set; }
        public int ColorHex
        {
            get { return (int)Color; }
            set { Color = (PostColor)value; }
        }
        public int ChildCount { get; set; }
        public List<JodelPost> Children { get; set; }
        public string ImageUrl { get; set; }
        public string ImageHost { get; set; }
        public string ImageAuthorization { get; set; }
        public string ThumbnailUrl { get; set; }

        public int? Next { get; set; } = null;
        public int Remaining { get; set; } = 0;

        #endregion

        #region Enum

        public enum UpvoteReason
        {
            Stub = -1,
            Cancel = 1,
            Funny = 1,
            Interesting = 2,
            SoTrue = 3,
            SaveJodel = 4,
            Other = 5
        }

        public enum DownvoteReason
        {
            Stub = -1,
            Cancel = 1,
            NotInteresting = 1,
            Repost = 2,
            NotAllowedOnJodel = 3,
            GoogleIt = 4,
            Other = 5
        }

        #endregion

        #region Constructor

        internal JodelPost()
        {

        }

        internal JodelPost(JsonPostJodels.Post jodel)
        {
            ColorHex = int.Parse(jodel.color, NumberStyles.HexNumber);
            ChildCount = jodel.child_count ?? 0;
            Children = jodel.children?.Select(c => new JodelPost(c)).ToList();
            CreatedAt = DateTime.ParseExact(jodel.created_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null);
            Discovered = jodel.discovered;
            DiscoveredBy = jodel.discovered_by;
            Distance = jodel.distance;
            GotThanks = jodel.got_thanks;
            ImageAuthorization = jodel.image_headers?.Authorization;
            ImageUrl = jodel.image_url;
            ImageHost = jodel.image_headers?.Host;
            Message = jodel.message;
            NotificationsEnabled = jodel.notifications_enabled;
            PinCounted = jodel.pin_count;
            Place = new Location
            {
                Longitude = jodel.location.loc_coordinates.lng,
                Latitude = jodel.location.loc_coordinates.lat,
                City = jodel.location.city,
                Accuracy = jodel.location.loc_accuracy,
                Name = jodel.location.name,
                Country = jodel.location.country
            };
            PostId = jodel.post_id;
            PostOwn = jodel.post_own;
            ThumbnailUrl = jodel.thumbnail_url;
            UpdatedAt = DateTime.ParseExact(jodel.updated_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null);
            UserHandle = jodel.user_handle;
            VoteCount = jodel.vote_count;
        }

        internal JodelPost(JsonPostDetail.RootObject jodel)
        {
            Children = jodel.replies?.Select(c => new JodelPost(c)).ToList();
            Next = !string.IsNullOrWhiteSpace(jodel.next) ? int.Parse(jodel.next) : (int?)null;
            Remaining = jodel.remaining;

            if (jodel.details != null)
            {
                ColorHex = int.Parse(jodel.details.color, NumberStyles.HexNumber);
                ChildCount = jodel.details.child_count ?? 0;
                CreatedAt = DateTime.ParseExact(jodel.details.created_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null);
                Discovered = jodel.details.discovered;
                DiscoveredBy = jodel.details.discovered_by;
                Distance = jodel.details.distance;
                GotThanks = jodel.details.got_thanks;
                ImageAuthorization = jodel.details.image_headers?.Authorization;
                ImageUrl = jodel.details.image_url;
                ImageHost = jodel.details.image_headers?.Host;
                Message = jodel.details.message;
                NotificationsEnabled = jodel.details.notifications_enabled;
                PinCounted = jodel.details.pin_count;
                Place = new Location
                {
                    Longitude = jodel.details.location.loc_coordinates.lng,
                    Latitude = jodel.details.location.loc_coordinates.lat,
                    City = jodel.details.location.city,
                    Accuracy = jodel.details.location.loc_accuracy,
                    Name = jodel.details.location.name,
                    Country = jodel.details.location.country
                };
                PostId = jodel.details.post_id;
                PostOwn = jodel.details.post_own;
                ThumbnailUrl = jodel.details.thumbnail_url;
                UpdatedAt = DateTime.ParseExact(jodel.details.updated_at.Replace("Z", "").Replace("T", " "), "yyyy-MM-dd HH:mm:ss.fff", null);
                UserHandle = jodel.details.user_handle;
                VoteCount = jodel.details.vote_count;
            }
        }

        #endregion

        #region Methods



        #endregion

        #region Classes/Enums

        /// <summary>
        /// Colors for JodelPost
        /// </summary>
        public enum PostColor
        {
            Orange = 0xFF9908,
            Yellow = 0xFFBA00,
            Red = 0xDD5F5F,
            Blue = 0x06A3CB,
            Bluegreyish = 0x8ABDB0,
            Green = 0x9EC41C,
            Random
        }

        public class Location
        {
            public string City { get; set; }
            public string Country { get; set; }
            public double Accuracy { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Name { get; set; }
        }

        #endregion
    }
}
