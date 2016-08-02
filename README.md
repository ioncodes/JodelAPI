# JodelAPI

Build Status: [![Build status](https://ci.appveyor.com/api/projects/status/2dx3f591ubmp978t/branch/master?svg=true)](https://ci.appveyor.com/project/ioncodes/jodelapi/branch/master)

Download JodelAPI.dll: https://ci.appveyor.com/api/projects/ioncodes/jodelapi/artifacts/JodelAPI/JodelAPI/bin/Debug/JodelAPI.dll
Download Newtonsoft.Json.dll: https://ci.appveyor.com/api/projects/ioncodes/jodelapi/artifacts/JodelAPI/JodelAPI/bin/Debug/Newtonsoft.Json.dll

Alternatively download it via NuGet package manager!

[WIP] API for the Jodel app in .NET

We're proud to announce that our JodelAPI finally reached revision 1.0!
Let's talk about the current state of things:

Jodel changed their authentication system few time ago. We applied the new authentication system to our API and you are able to generate access tokens and post whatever you want!

## How the private Jodel API works...

The API generally takes 3 arguments for requests: lat, lng, access_token.
lat: This is the latitude of your location.
lng: This is the longitude of your location.
access_token: In the app, navigate to the options and click on "write us", then scroll down to "client info". There is something called "access_token=". The token is the string after the "=". Alternatively you can gather it by performing a mitm.

Each PUT/POST request needs to be a signed request. This is accomplished by using a HMAC hash that is built from the stringified payload of the desired request.

### Getting the Jodels

To get the first Jodels, open this as GET: "https://api.go-tellm.com/api/v2/posts/location/combo?lat={LAT}&lng={LNG}&access_token={YOUR_ACCESS_TOKEN}"

To obtain the rest, open this as GET:
"https://api.go-tellm.com/api/v2/posts/location?lng={LNG}&lat={LAT}&after={THE_LAST_POST_ID_OBTAINED_ABOVE}&access_token={YOUR_ACCESS_TOKEN}&limit=1000000"

Notice: Instead of providing your access token as a parameter, you can always set "Authentication: Bearer {access_token}" as a request header.

### Get Karma

GET: "https://api.go-tellm.com/api/v2/users/karma?access_token={ACCESS_TOKEN}"

### More

Visit this link: http://jodel-app.wikia.com/

## Documentation

Jodels are returned as ```List<Jodels>```.

```
public string PostId { get; set; }
public string Message { get; set; }
public string HexColor { get; set; }
public bool IsImage { get; set; }
public int VoteCount { get; set; }
public string Latitude { get; set; }
public string Longitude { get; set; }
public string LocationName { get; set; }
```

Comments are returned as ```List<Comments>```.

```
public string PostId { get; set; }
public string Message { get; set; }
public string UserHandle { get; set; }
public int VoteCount { get; set; }
```

The lists both contain formatted json objects.

# Setup

Add the vars and namespace properly:

```c#
using JodelAPI;

// ...

public static string AccessToken = "";
public static string Latitude = "";
public static string Longitude = "";
public static string CountryCode = "";
public static string City = "";
```

Baseclass is called 'API'.

# Get Jodels

Use ```GetAllJodels()```;

# Upvote & Downvote

You can use ```Upvote()``` and ```Downvote()```. Remember that you can pass the postID (string) or the index (int) from the List as argument.

Voting more than 199 times in a row will get your IP banned!

# Karma

```GetKarma()``` will return an int with your amount of Karma.

# Posting

```PostJodel(string message, PostColor colorParam = PostColor.Random, string postId = null);```

message: message to post.

colorParam: Color from enum for post.

postId: add postID if it is a comment, otherwise don't change it.

## Posting a Comment

Set the postId (see above).

## Colors

The following colors are currently accepted by the Jodel server:

```
public enum PostColor
{
   Orange,
   Yellow,
   Red,
   Blue,
   Bluegreyish,
   Green,
   Random
}
```

# Access Token

We were able to reverse their new Authentication system and now GenerateAccessToken() works! It returns a string containing the token.
Notice: Spamming the function (>60 times) will get your IP banned!

# Moderation

You can now queue reported Jodel posts through our API! Please note that you will only be able to do this if you reached about 30k Karma and got your moderation enrollment.

Posts are returned as ```List<ModerationQueue>```.

```
public string PostId { get; set; }
public string Message { get; set; }
public int VoteCount { get; set; }
public string HexColor { get; set; }
public string UserHandle { get; set; }
public int TaskId { get; set; }
public int FlagCount { get; set; }
public string ParentId { get; set; }
public int FlagReason { get; set; }
```

## Flaging Jodels

You can flag now Jodels/Comments by calling the function ```FlagJodel(int taskId, Decision decision)```! taskId is the ID retrieved from ```List<ModerationQueue>```

```
public enum Decision
{
   Allow = 0,
   Block = 2,
   DontKnow = 1
} // ignore the numbers, they are for internal use.
```
