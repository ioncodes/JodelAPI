# JodelAPI
[![forthebadge](http://forthebadge.com/images/badges/built-with-swag.svg)](http://forthebadge.com)
[![forthebadge](http://forthebadge.com/images/badges/gluten-free.svg)](http://forthebadge.com)
[![forthebadge](http://forthebadge.com/images/badges/certified-snoop-lion.svg)](http://forthebadge.com)

*Big thanks to KirschbaumP*

[![Build status](https://ci.appveyor.com/api/projects/status/2dx3f591ubmp978t?svg=true)](https://ci.appveyor.com/project/ioncodes/jodelapi)
[![Github All Releases](https://img.shields.io/github/downloads/ioncodes/JodelAPI/total.svg)](https://github.com/ioncodes/JodelAPI/releases)
[![NuGet](https://img.shields.io/nuget/v/JodelAPI.svg)](https://www.nuget.org/packages/JodelAPI/)
[![Status](https://img.shields.io/badge/api-working-brightgreen.svg)]()
[![Implementation](https://img.shields.io/badge/api--version-4.44.1-brightgreen.svg)]()
[![Test status](http://teststatusbadge.azurewebsites.net/api/status/ioncodes/jodelapi)](https://ci.appveyor.com/project/ioncodes/jodelapi)
[![Gitter chat](https://badges.gitter.im/ioncodes/JodelAPI.svg)](https://gitter.im/JodelAPI/Lobby?utm_source=share-link&utm_medium=link&utm_campaign=share-link)

## Introduction

This is the carefully reverse-engineered .NET Jodel API in C#. Please feel free to grab the source or compiled library from the link provided below.

*This is a Swiss software product and therefore might be awesome!*

## Binaries and pre-requisites
* [GitHub Releases](https://github.com/ioncodes/JodelAPI/releases) or from Nuget

## Let's get right into it!
Create a Jodel object which holds all functions needed:
```cs
Jodel jodel = new Jodel(string place, string countrycode, string city, bool createToken = true);
```
Where 'place' is the place how you would enter the location in Google Maps, 'countrycode' and 'city' are the values that are sent to Jodel! You might have your own token, in that case you can set createToken to false. If you do this, make sure to set the data found in the AccessToken (Account.AccessToken) class:
```cs
jodel.Account.Token.Token = "";
jodel.Account.Token.RefreshToken = "";
```

Your data is stored in the 'Account' field, which comes from the 'User' class. You can defined a 'User' object yourself and pass it to the Jodel constructor. This gives you extra options, such as setting the 'Location' object yourself (Latitude, Longitude), etc:
```cs
User user = new User();
user.Location.Longitude = 13.37;
// ...
Jodel jodel = new Jodel(user);
```

To set your location, you need to call 'SetLocation' and it's variants:
```cs
jodel.Account.Place.SetNewPlace(47.213, 8.1233333); // Lat, Lng
// OR
jodel.Account.Place.SetNewPlace($"Deutschland Berlin"); // GoogleMaps Query string
// THEN
jodel.SetLocation(); // Set the location applied via SetNewPlace or via Longitude and Latitude.
```

After creating your 'Jodel' object, you will find all methods you need in there. Here is a list:
```cs
// Fields and Properties
Account:User

// Constructor
Jodel(User user)
Jodel(string place, string countrCode, string cityName, bool createToken)

/* Methods */
// Account
GenerateAccessToken(string proxy):bool
RefreshAccessToken():bool
GetUserConfig():void
GetKarma():int
SetLocation():void
GetCaptcha(bool advanced):Captcha
SolveCaptcha(Captcha captcha, int[] answer):bool
VerifyAutomatically():bool

// Channels
GetRecommendedChannels():IEnumerable<Channel>
GetFollowedChannelsMeta(bool home):IEnumerable<Channel>

// Jodels
GetPostLocationCombo(bool stickies, bool home):JodelMainData
GetPostHashtagCombo(string hashtag, bool home):JodelMainData
GetPostChannelCombo(string channel, bool home):JodelMainData
GetRecentPostsAfter(string afterPostId, bool home):IEnumerable<JodelPost>
Upvote(string postId, UpvoteReason reason, string proxy):void
Downvote(string postId, DownvoteReason reason, string proxy):void
Post(string message, string parentPostId, PostColor color, byte[] image, bool home):string
GetPost(string postId):JodelPost
GetPostDetails(string postId, bool details, bool reversed, int next):JodelPost
SharePost(string postId):string
```

The Account object (User class) contains this:
```cs
/* User */
// Fields and Properties
Place:Location
CountryCode:string
CityName:string
Token:AccessToken

// UserProperties
Moderator:bool
UserType:object
Experiments:List<Experiment>
FollowedChannels:List<Channel>
FollowedHashtags:List<string>
ChannelsFollowLimit:int
TripleFeedEnabled:bool
HomeName:string
HomeSet:bool
Location:string
Verified:bool
  
// Constructor
User()
User(string deviceUid, string accessToken)
User(AccessToken token)

/* Experiment */
// Fields and Properties
Name:string
Group:string
Features:List<string>

// Constructor
Experiment(string name, string group, List<string> features)
```

This is the definition of the Place field (Location class):
```cs
/* Location */
// Fields and Properties
Place:string
Longitude:double
Latitude:double

// Constructor
Location(string place)

// Methods
SetNewPlace(string place):void
SetNewPlace(double lat, double lng):void
FindCoordinates():void
```

If you have questions or requests, feel free to create issues, open PRs or ask me via Gitter!
This API should always have all endpoints implemented. The version of the currently implemented API can be found on the top of the README.

A plain list of all supported endpoints can be found [here](https://github.com/ioncodes/JodelAPI/blob/master/JodelAPI/JodelAPI/Internal/Links.cs)
