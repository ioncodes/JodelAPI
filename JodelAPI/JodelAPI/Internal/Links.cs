using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Shared;

namespace JodelAPI.Internal
{
    internal static class Links
    {
        #region Const

        //Placholder
        public const string PhAccessToken = "{AT}";
        public const string PhLat = "{LAT}";
        public const string PhLng = "{LNG}";
        public const string PhPostId = "{PID}";
        public const string PhChannelId = "{CH}";


        //Links
        public const string LinkGenAt = "https://api.go-tellm.com/api/v2/users/";
        public const string LinkGenAtPayload = "api.go-tellm.com%443%/api/v2/users/";

        public const string LinkRefreshToken = "https://api.go-tellm.com/api/v2/users/refreshToken?access_token=" + PhAccessToken;


        //OldLinks
        //TODO: Links auf Aktualitä̱t Checken (z.B. v2/v3)
        public const string LinkFirstJodels =
            "https://api.go-tellm.com/api/v2/posts/location/combo?lat=" + PhLat + "&lng=" + PhLng + "&access_token=" + PhAccessToken;

        public const string LinkSecondJodels =
            "https://api.go-tellm.com/api/v2/posts/location?lng=" + PhLng + "&lat=" + PhLat + "&after=" + PhPostId + "&access_token=" + PhAccessToken + "&limit=1000";

        public const string LinkUpvoteJodel = "https://api.go-tellm.com/api/v2/posts/" + PhPostId + "/upvote/";
        public const string LinkDownvoteJodel = "https://api.go-tellm.com/api/v2/posts/" + PhPostId + "/downvote/";
        public const string LinkGetKarma = "https://api.go-tellm.com/api/v2/users/karma?access_token=" + PhAccessToken;
        public const string LinkPostJodel = "https://api.go-tellm.com/api/v2/posts/";
        public const string LinkGetComments = "https://api.go-tellm.com/api/v2/posts/" + PhPostId + "?access_token=" + PhAccessToken;
        public const string LinkModeration = "https://api.go-tellm.com/api/v3/moderation/?access_token=" + PhAccessToken;
        public const string LinkUserLocation = "https://api.go-tellm.com/api/v2/users/location?access_token=" + PhAccessToken;
        public const string LinkReportJodel = "https://api.go-tellm.com/api/v2/posts/" + PhPostId + "/flag?" + PhAccessToken;
        public const string LinkLoadFollowedChannels = "https://api.go-tellm.com/api/v3/user/followedChannelsMeta?access_token=" + PhAccessToken;

        public const string LinkGetMyJodels =
            "https://api.go-tellm.com/api/v2/posts/mine?limit=150&access_token=" + PhAccessToken + "&skip=0";

        public const string LinkGetMyComments =
            "https://api.go-tellm.com/api/v2/posts/mine/replies?skip=0&access_token=" + PhAccessToken + "&limit=150";

        public const string LinkGetMyVotes =
            "https://api.go-tellm.com/api/v2/posts/mine/votes?limit=150&access_token=" + PhAccessToken + "&skip=0";

        public const string LinkConfig = "https://api.go-tellm.com/api/v3/user/config?access_token=" + PhAccessToken;
        public const string LinkPinJodel = "https://api.go-tellm.com/api/v2/posts/" + PhPostId + "/pin?access_token=" + PhAccessToken;

        public const string LinkMyPins =
            "https://api.go-tellm.com/api/v2/posts/mine/pinned?limit=1000&access_token=" + PhAccessToken + "&skip=0";

        public const string LinkFollowChannel = "https://api.go-tellm.com/api/v3/user/followChannel?channel="+ PhChannelId;

        public const string LinkUnfollowChannel = "https://api.go-tellm.com/api/v3/user/unfollowChannel?channel=" + PhChannelId;

        public const string LinkGetJodelsFromChannel =
            "https://api.go-tellm.com/api/v3/posts/channel/combo?access_token=" + PhAccessToken + "&channel=" + PhChannelId;

        public const string LinkGetRecommendedChannels =
            "https://api.go-tellm.com/api/v3/user/recommendedChannels?access_token=" + PhAccessToken;

        public const string LinkDeleteJodel = "https://api.go-tellm.com/api/v2/posts/" + PhPostId;
        public const string LinkPostImage = "https://api.go-tellm.com/api/v2/posts?access_token=" + PhAccessToken;

        #endregion

        #region static Methods

        internal static string ToLink(this string link, User user, string postIdOrChannel = "")
        {
            //AccessToken
            if (link.Contains(PhAccessToken))
            {
                link = link.Replace(PhAccessToken, user.Token.Token);
            }

            //Latitide
            if (link.Contains(PhLat))
            {
                link = link.Replace(PhLat, user.Place.Latitude.ToString(CultureInfo.InvariantCulture));
            }

            //Longitude
            if (link.Contains(PhLng))
            {
                link = link.Replace(PhLng, user.Place.Longitude.ToString(CultureInfo.InvariantCulture));
            }

            //Longitude
            if (link.Contains(PhLng))
            {
                link = link.Replace(PhLng, user.Place.Longitude.ToString(CultureInfo.InvariantCulture));
            }

            //PostId
            if (link.Contains(PhPostId))
            {
                link = link.Replace(PhPostId, postIdOrChannel);
            }

            //ChannelId
            if (link.Contains(PhChannelId))
            {
                link = link.Replace(PhChannelId, postIdOrChannel);
            }
            
            return link;
        }

        #endregion
    }
}
