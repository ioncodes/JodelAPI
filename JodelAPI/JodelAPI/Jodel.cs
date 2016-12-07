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

        public Jodel(string place, string countrCode, string cityName)
            : this(new User
            {
                CountryCode = countrCode,
                CityName = cityName,
                Place = new Location(place)
            })
        { }

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
            foreach (JsonConfig.Experiment experiment in config.experiments)
            {
                experiments.Add(new User.Experiment(experiment.name, experiment.group, experiment.features));
            }

            List<Channel> channels = new List<Channel>(config.followed_hashtags.Count);
            foreach (string channelname in config.followed_channels)
            {
                channels.Add(new Channel(channelname, true));
            }

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

        #endregion

        #region Channels

        public List<Channel> GetRecommendedChannels()
        {
            string jsonString = Links.GetRecommendedChannels.ExecuteRequest(Account, new Dictionary<string, string>() { { "home", "false" } }, payload: new JsonRequestRecommendedChannels());

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
            JsonRequestFollowedChannelMeta payload= new JsonRequestFollowedChannelMeta();
            foreach (Channel channel in Account.FollowedChannels.Where(x=>x.Following).ToList())
            {
                payload.Values.Add(channel.ChannelName, -1);
            }
            string jsonString = Links.GetFollowedChannelsMeta.ExecuteRequest(Account, new Dictionary<string, string>() { { "home", "false" } }, payload: payload);

            JsonFollowedChannelsMeta.RootObject data = JsonConvert.DeserializeObject<JsonFollowedChannelsMeta.RootObject>(jsonString);
            List<Channel> channels = new List<Channel>();
            foreach (JsonFollowedChannelsMeta.Channel channel in data.channels)
            {
                Channel c = Account.FollowedChannels.FirstOrDefault(x => x.ChannelName == channel.channel)?
                        .UpdateProperties(channel.followers, channel.sponsored, channel.unread);
                if(c != null)
                    channels.Add(c);
            }


            return channels;
        }

        #endregion

        #region Jodels

        public JodelMainData GetPostLocationCombo(bool stickies = false, bool home = false)
        {
            JsonRequestPostsLocationCombo payload = new JsonRequestPostsLocationCombo
            {
                Lat = Account.Place.Latitude.ToString(CultureInfo.InvariantCulture),
                Lng = Account.Place.Longitude.ToString(CultureInfo.InvariantCulture),
                Home = home,
                Stickies=stickies
            };
            string jsonString = Links.GetCombo.ExecuteRequest(Account, new Dictionary<string, string>
            {
                { "lat", Account.Place.Latitude.ToString(CultureInfo.InvariantCulture) },
                { "lng", Account.Place.Longitude.ToString(CultureInfo.InvariantCulture) },
                { "stickies", stickies.ToString() },
                { "home", home.ToString() }
            }, payload: payload);
            JsonJodelsFirstRound.RootObject jodels = JsonConvert.DeserializeObject<JsonJodelsFirstRound.RootObject>(jsonString);
            JodelMainData data = new JodelMainData();

            foreach (JsonJodelsFirstRound.Recent jodel in jodels.recent)
            {
                var item = new JodelPost();
                item.ColorHex = int.Parse(jodel.color, NumberStyles.HexNumber);
                item.ChildCount = jodel.child_count ?? 0;
                item.CreatedAt = jodel.created_at;
                item.Discovered = jodel.discovered;
                item.DiscoveredBy = jodel.discovered_by;
                item.Distance = jodel.distance;
                item.GotThanks = jodel.got_thanks;
                item.ImageAuthorization = jodel.image_headers?.Authorization;
                item.ImageUrl = jodel.image_url;
                item.ImageHost = jodel.image_headers?.Host;
                item.Message = jodel.message;
                item.NotificationsEnabled = jodel.notifications_enabled;
                item.PinCounted = jodel.pin_count;
                item.Place = new JodelPost.Location();
                item.Place.Longitude = jodel.location.loc_coordinates.lng;
                item.Place.Latitude = jodel.location.loc_coordinates.lat;
                item.Place.City = jodel.location.city;
                item.Place.Accuracy = jodel.location.loc_accuracy;
                item.Place.Name = jodel.location.name;
                item.Place.Country = jodel.location.country;
                item.PostId = jodel.post_id;
                item.PostOwn = jodel.post_own;
                item.ThumbnailUrl = jodel.thumbnail_url;
                item.UpdatedAt = jodel.updated_at;
                item.UserHandle = jodel.user_handle;
                item.VoteCount = jodel.vote_count;
                data.RencentJodels.Add(item);
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

        #endregion

        #endregion
    }
}
