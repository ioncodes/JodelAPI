using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Shared
{
    public class JodelPost
    {
        #region Fields and Properties

        public string PostId { get; set; }
        public string CreatedAt { get; set; }
        public string Message { get; set; }
        public int Discovered { get; set; }
        public int DiscoveredBy { get; set; }
        public int Distance { get; set; }
        public bool GotThanks { get; set; }
        public Location Place { get; set; }
        public bool NotificationsEnabled { get; set; }
        public int PinCounted { get; set; }
        public string PostOwn { get; set; }
        public string UpdatedAt { get; set; }
        public string UserHandle { get; set; }
        public int VoteCount { get; set; }
        public PostColor Color { get; set; }
        public int ColorHex
        {
            get { return (int)Color; }
            set { Color = (PostColor)value; }
        }
        public int ChildCount { get; set; }
        public string ImageUrl { get; set; }
        public string ImageHost { get; set; }
        public string ImageAuthorization { get; set; }
        public string ThumbnailUrl { get; set; }

        #endregion

        #region Constructor

        internal JodelPost()
        {

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
