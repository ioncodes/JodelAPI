# JodelAPI
We are glad to tell you that the API is in the final state. Issue reports and enhancement requests are welcome!

## Builds
* [JodelAPI](https://ci.appveyor.com/api/projects/ioncodes/jodelapi/artifacts/JodelAPI/JodelAPI/bin/Debug/JodelAPI.dll)
* [Newtonsoft.Json](https://ci.appveyor.com/api/projects/ioncodes/jodelapi/artifacts/JodelAPI/JodelAPI/bin/Debug/Newtonsoft.Json.dll)

## Setup
In the class 'Account' set the following variables first.
```c#
using JodelAPI; // add namespace
// ...
Account.AccessToken = "";
Account.Longitude = "";
Account.Latitude = "";
Account.City = "";
Account.CountryCode = "";
```

## Jodel
The class 'Jodel' contains all functions for getting, deleting, posting, etc Jodels.

### Getting the Jodels
To get the Jodels call ```GetAllJodels()```.

**Return:**
```List<Jodels>```

```c#
public string PostId { get; set; }
public string Message { get; set; }
public string HexColor { get; set; }
public bool IsImage { get; set; }
public int VoteCount { get; set; }
public int CommentsCount { get; set; }
public string LocationName { get; set; }
```

### Upvoting & Downvoting
To down/upvote an Jodel use ```UpvoteJodel(string postId)``` or ```DownvoteJodel(string postId)```.
It takes the Post Id of the Jodel.

### Posting
To post an Jodel with a message use
```public static string PostJodel(string message, PostColor colorParam = PostColor.Random, string postId = null)```.

To post an image use 
```public static string PostJodel(Image image, PostColor colorParam = PostColor.Random, string postId = null)```

**Arguments:**
* message, image: the post content.
* colorParam: PostColor enum which sets the post color.
* postId: the original jodel Post ID (optional)

PostColor is defined as follows:
```c#
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
* postId: the jodel postId

**Return:**
```List<Comments>```
```c#
public string PostId { get; set; }
public string Message { get; set; }
public string UserHandle { get; set; }
public int VoteCount { get; set; }
```

### My Jodels, Comments and Votes$

**COMMING SOON**

