# JodelAPI
## Introduction

This is the carefully reverse-engineered .NET Jodel API in C#. Please feel free to grab the source or compiled library from the link provided below.

*This is a Swiss software product and therefore might be awesome!*

***PLEASE NOTE:***

**As of October 29th 2016 the API is completely non-static and should be easier to use than ever before.**

## Binaries and pre-requisites
* [JodelAPI](https://ci.appveyor.com/api/projects/ioncodes/jodelapi/artifacts/JodelAPI/JodelAPI/bin/Debug/JodelAPI.dll), built with love
* [Newtonsoft.Json (Json.Net)](https://github.com/JamesNK/Newtonsoft.Json/releases), also available as a NuGet Package

## Getting Started

### Initial Setup
In order to set your location and start interacting with the various methods of the API you must first set up a few objects.
Namely, you will need a new User object, the Account class as well as a Jodel object. You will also need to generate your
own arsenal of access tokens by using the ```Account.GenerateAccessToken()``` method. (This generates up to 60 access tokens per IP, please be careful to not get yourself banned!)

```csharp
using JodelAPI; // add namespace
// ...

User user = new User(string.Empty, MyLoc.Latitude, MyLoc.Longitude, MyLoc.CountryCode, MyLoc.City);

List<Tokens> myAccessToken = Account.GenerateAccessToken(user);
user.AccessToken = myAccessToken;

Jodel jodel = new Jodel(user);
jodel.Account.SetUserLocation(myAccessToken);
```

## Jodel
The class 'Jodel' contains all functions for getting, deleting, posting, etc Jodels.

### Getting the Jodels
To get the Jodels call 
```csharp
GetAllJodels()
```

**Returns:**
```csharp
List<Jodels>
```

**Properties:**
```csharp
public string PostId { get; set; }
public string Message { get; set; }
public DateTime CreatedAt { get; set; }
public DateTime UpdatedAt { get; set; }
public int PinCount { get; set; }
public string HexColor { get; set; }
public bool IsNotificationEnabled { get; set; }
public string PostOwn { get; set; }
public int Distance { get; set; }
public int ChildCount { get; set; }
public bool IsImage { get; set; }
public int VoteCount { get; set; }
public int CommentsCount { get; set; }
public string LocationName { get; set; }
public string UserHandle { get; set; }
public string ImageUrl { get; set; }
```

### Upvoting & Downvoting
To down-/upvote a Jodel use ```UpvoteJodel(string postId)``` or ```DownvoteJodel(string postId)```.
It takes the post id of the Jodel.

### Posting
To post a Jodel with a message use
```PostJodel(string message, PostColor colorParam = PostColor.Random, string postId = null)```.

To post an image use 
```PostJodel(Image image, PostColor colorParam = PostColor.Random, string postId = null)```

*Please note that posting images is still in the works!*

**Arguments:**
* message, image: the post content.
* colorParam: PostColor enum which sets the post color.
* postId: the original Jodel post id (optional)

PostColor is defined as follows:
```csharp
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

### Getting comments
To get the comments from an Jodel you can use ```GetComments(string postId)```.

**Arguments:**
* postId: the Jodel postId

**Returns:**
```csharp
List<Comments>
```

**Properties:**

```csharp
public string PostId { get; set; }
public string Message { get; set; }
public string UserHandle { get; set; }
public int VoteCount { get; set; }
public DateTime CreatedAt { get; set; }
public DateTime UpdatedAt { get; set; }
public bool IsImage { get; set; }
public string ImageUrl { get; set; }
```

### My Jodels, Comments and Votes

**To be completed ...**
