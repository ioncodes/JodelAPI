using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Internal;
using JodelAPI.Json;
using JodelAPI.Json.Request;
using JodelAPI.Json.Response;
using JodelAPI.Shared;
using Newtonsoft.Json;

namespace JodelAPI
{
    public class Jodel
    {
        #region Fields and Properties

        public User Account { get; private set; }

        #endregion

        #region Constructor

        public Jodel(User user)
        {
            this.Account = user;
        }

        public Jodel(string place, string countrCode, string cityName, bool createToken = true)
            : this(new User
            {
                CountryCode = countrCode,
                CityName = cityName,
                Place = new Location(place),
            })
        {
            if (createToken)
                GenerateAccessToken();
        }

        #endregion

        #region Methods

        #region Account

        public bool GenerateAccessToken()
        {
            return Account.Token.GenerateNewAccessToken();
        }

        public bool RefreshAccessToken()
        {
            return Account.Token.RefreshAccessToken();
        }

        public void GetConfig()
        {
            string jsonString = Links.GetConfig.ExecuteRequest(Account);

            JsonConfig.RootObject config = JsonConvert.DeserializeObject<JsonConfig.RootObject>(jsonString);


            List<User.Experiment> experiments = new List<User.Experiment>(config.experiments.Count);
            experiments.AddRange(config.experiments.Select(experiment => new User.Experiment(experiment.name, experiment.@group, experiment.features)));

            List<Channel> channels = new List<Channel>(config.followed_hashtags.Count);
            channels.AddRange(config.followed_channels.Select(channelname => new Channel(channelname, true)));

            Account.ChannelsFollowLimit = config.channels_follow_limit;
            Account.Experiments = experiments;
            Account.HomeName = config.home_name;
            Account.HomeSet = config.home_set;
            Account.FollowedHashtags = config.followed_hashtags;
            Account.Location = config.location;
            Account.Moderator = config.moderator;
            Account.TripleFeedEnabled = config.triple_feed_enabled;
            Account.UserType = config.user_type;
            Account.Verified = config.verified;
            Account.FollowedChannels = channels;
        }

        public int GetKarma()
        {
            string jsonString = Links.GetKarma.ExecuteRequest(Account);

            JsonKarma.RootObject karma = JsonConvert.DeserializeObject<JsonKarma.RootObject>(jsonString);
            return karma.karma;
        }

        public void SetLocation()
        {
            JsonRequestSetLocation payload = new JsonRequestSetLocation
            {
                location =
                {
                    city = Account.CityName,
                    country = Account.CountryCode,
                    loc_accuracy = 0.0,
                    name = Account.CityName,
                    loc_coordinates =
                    {
                        lat = Account.Place.Latitude,
                        lng = Account.Place.Longitude
                    }
                }
            };

            Links.SetPosition.ExecuteRequest(Account, payload: payload);
        }

        #endregion

        #region Channels

        public List<Channel> GetRecommendedChannels()
        {
            string jsonString = Links.GetRecommendedChannels.ExecuteRequest(Account, new Dictionary<string, string> { { "home", "false" } }, payload: new JsonRequestRecommendedChannels());

            JsonRecommendedChannels.RootObject channels = JsonConvert.DeserializeObject<JsonRecommendedChannels.RootObject>(jsonString);

            List<Channel> recommendedChannels = new List<Channel>();
            foreach (JsonRecommendedChannels.Recommended recommended in channels.recommended)
            {
                if (Account.FollowedChannels.Any(x => x.ChannelName == recommended.channel))
                {
                    Channel ch = Account.FollowedChannels.First(x => x.ChannelName == recommended.channel).UpdateProperties(recommended.image_url, recommended.followers);
                    recommendedChannels.Add(ch);
                }
                else
                {
                    recommendedChannels.Add(new Channel(recommended.channel) { ImageUrl = recommended.image_url, Followers = recommended.followers });
                }
            }

            return recommendedChannels;
        }

        public List<Channel> GetFollowedChannelsMeta()
        {
            JsonRequestFollowedChannelMeta payload = new JsonRequestFollowedChannelMeta();
            foreach (Channel channel in Account.FollowedChannels.Where(x => x.Following).ToList())
            {
                payload.Values.Add(channel.ChannelName, -1);
            }
            string jsonString = Links.GetFollowedChannelsMeta.ExecuteRequest(Account, new Dictionary<string, string>() { { "home", "false" } }, payload: payload);

            JsonFollowedChannelsMeta.RootObject data = JsonConvert.DeserializeObject<JsonFollowedChannelsMeta.RootObject>(jsonString);


            return data.channels.Select(channel => Account.FollowedChannels
                .FirstOrDefault(x => x.ChannelName == channel.channel)?
                .UpdateProperties(channel.followers, channel.sponsored, channel.unread))
                .Where(c => c != null).ToList();
        }

        #endregion

        #region Jodels

        public JodelMainData GetPostLocationCombo(bool stickies = false, bool home = false)
        {
            string jsonString = Links.GetCombo.ExecuteRequest(Account, new Dictionary<string, string>
            {
                { "lat", Account.Place.Latitude.ToString("F",CultureInfo.InvariantCulture) },
                { "lng", Account.Place.Longitude.ToString("F",CultureInfo.InvariantCulture) },
                { "stickies", stickies.ToString().ToLower() },
                { "home", home.ToString().ToLower() }
            });

            JsonJodelsFirstRound.RootObject jodels = JsonConvert.DeserializeObject<JsonJodelsFirstRound.RootObject>(jsonString);
            JodelMainData data = new JodelMainData { Max = jodels.max };


            foreach (JsonJodelsFirstRound.Recent jodel in jodels.recent)
            {
                var item = new JodelPost
                {
                    ColorHex = int.Parse(jodel.color, NumberStyles.HexNumber),
                    ChildCount = jodel.child_count ?? 0,
                    CreatedAt = jodel.created_at,
                    Discovered = jodel.discovered,
                    DiscoveredBy = jodel.discovered_by,
                    Distance = jodel.distance,
                    GotThanks = jodel.got_thanks,
                    ImageAuthorization = jodel.image_headers?.Authorization,
                    ImageUrl = jodel.image_url,
                    ImageHost = jodel.image_headers?.Host,
                    Message = jodel.message,
                    NotificationsEnabled = jodel.notifications_enabled,
                    PinCounted = jodel.pin_count,
                    Place = new JodelPost.Location
                    {
                        Longitude = jodel.location.loc_coordinates.lng,
                        Latitude = jodel.location.loc_coordinates.lat,
                        City = jodel.location.city,
                        Accuracy = jodel.location.loc_accuracy,
                        Name = jodel.location.name,
                        Country = jodel.location.country
                    },
                    PostId = jodel.post_id,
                    PostOwn = jodel.post_own,
                    ThumbnailUrl = jodel.thumbnail_url,
                    UpdatedAt = jodel.updated_at,
                    UserHandle = jodel.user_handle,
                    VoteCount = jodel.vote_count
                };
                data.RecentJodels.Add(item);
            }

            foreach (JsonJodelsFirstRound.Replied jodel in jodels.replied)
            {
                data.RepliedJodels.Add(new JodelPost
                {
                    ColorHex = int.Parse(jodel.color, NumberStyles.HexNumber),
                    ChildCount = jodel.child_count,
                    CreatedAt = jodel.created_at,
                    Discovered = jodel.discovered,
                    DiscoveredBy = jodel.discovered_by,
                    Distance = jodel.distance,
                    GotThanks = jodel.got_thanks,
                    ImageAuthorization = jodel.image_headers?.Authorization,
                    ImageUrl = jodel.image_url,
                    ImageHost = jodel.image_headers?.Host,
                    Message = jodel.message,
                    NotificationsEnabled = jodel.notifications_enabled,
                    PinCounted = jodel.pin_count,
                    Place = new JodelPost.Location
                    {
                        Longitude = jodel.location.loc_coordinates.lng,
                        Latitude = jodel.location.loc_coordinates.lat,
                        City = jodel.location.city,
                        Accuracy = jodel.location.loc_accuracy,
                        Name = jodel.location.name,
                        Country = jodel.location.country
                    },
                    PostId = jodel.post_id,
                    PostOwn = jodel.post_own,
                    ThumbnailUrl = jodel.thumbnail_url,
                    UpdatedAt = jodel.updated_at,
                    UserHandle = jodel.user_handle,
                    VoteCount = jodel.vote_count
                });
            }

            foreach (JsonJodelsFirstRound.Voted jodel in jodels.voted)
            {
                data.VotedJodels.Add(new JodelPost
                {
                    ColorHex = int.Parse(jodel.color, NumberStyles.HexNumber),
                    ChildCount = jodel.child_count,
                    CreatedAt = jodel.created_at,
                    Discovered = jodel.discovered,
                    DiscoveredBy = jodel.discovered_by,
                    Distance = jodel.distance,
                    GotThanks = jodel.got_thanks,
                    ImageAuthorization = jodel.image_headers?.Authorization,
                    ImageUrl = jodel.image_url,
                    ImageHost = jodel.image_headers?.Host,
                    Message = jodel.message,
                    NotificationsEnabled = jodel.notifications_enabled,
                    PinCounted = jodel.pin_count,
                    Place = new JodelPost.Location
                    {
                        Longitude = jodel.location.loc_coordinates.lng,
                        Latitude = jodel.location.loc_coordinates.lat,
                        City = jodel.location.city,
                        Accuracy = jodel.location.loc_accuracy,
                        Name = jodel.location.name,
                        Country = jodel.location.country
                    },
                    PostId = jodel.post_id,
                    PostOwn = jodel.post_own,
                    ThumbnailUrl = jodel.thumbnail_url,
                    UpdatedAt = jodel.updated_at,
                    UserHandle = jodel.user_handle,
                    VoteCount = jodel.vote_count
                });
            }

            return data;
        }

        public List<JodelPost> GetRecentPostsAfter(string afterPostId, bool home = false)
        {

            string jsonString = Links.GetPosts.ExecuteRequest(Account, new Dictionary<string, string>
            {
                { "after", afterPostId },
                { "lat", Account.Place.Latitude.ToString(CultureInfo.InvariantCulture) },
                { "lng", Account.Place.Longitude.ToString(CultureInfo.InvariantCulture) },
                { "home", home.ToString().ToLower() }
            });

            JsonPostJodels.RootObject data = JsonConvert.DeserializeObject<JsonPostJodels.RootObject>(jsonString);

            List<JodelPost> jodels = new List<JodelPost>();

            foreach (JsonPostJodels.Post jodel in data.posts)
            {
                jodels.Add(new JodelPost
                {
                    ColorHex = int.Parse(jodel.color, NumberStyles.HexNumber),
                    ChildCount = jodel.child_count ?? 0,
                    CreatedAt = jodel.created_at,
                    Discovered = jodel.discovered,
                    DiscoveredBy = jodel.discovered_by,
                    Distance = jodel.distance,
                    GotThanks = jodel.got_thanks,
                    ImageAuthorization = jodel.image_headers?.Authorization,
                    ImageUrl = jodel.image_url,
                    ImageHost = jodel.image_headers?.Host,
                    Message = jodel.message,
                    NotificationsEnabled = jodel.notifications_enabled,
                    PinCounted = jodel.pin_count,
                    Place = new JodelPost.Location
                    {
                        Longitude = jodel.location.loc_coordinates.lng,
                        Latitude = jodel.location.loc_coordinates.lat,
                        City = jodel.location.city,
                        Accuracy = jodel.location.loc_accuracy,
                        Name = jodel.location.name,
                        Country = jodel.location.country
                    },
                    PostId = jodel.post_id,
                    PostOwn = jodel.post_own,
                    ThumbnailUrl = jodel.thumbnail_url,
                    UpdatedAt = jodel.updated_at,
                    UserHandle = jodel.user_handle,
                    VoteCount = jodel.vote_count
                });
            }
            return jodels;
        }

        #endregion

        #endregion
    }
}
