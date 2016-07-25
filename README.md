# JodelAPI
[WIP] API for the Jodel app in .NET
With this commit we'd like to introduce version 0.9 which will be updated soon to version 1.0! 
Well... What's new boss? Good question!
Jodel changed their authentication system not long ago. We are glad to tell you that we've been able to reverse it!
The most important fix in this version is that we finally go GenerateAccessToken working!

## How the private Jodel API works...

The API is using 3 paramters in general: lat, lng, access_token.
lat: This is the latitude of your location.
lng: This is the longitude of your location.
access_token: In the app, navigate to the options and click on "write us", then scroll down to "client info". There is something called "access_token=". The token is the string after the "=".

UPDATE: Since the authsystem changed, this theory might be outdated. At this time I share this (look in the code for more info): It uses a HMAC auth.


### Getting the Jodels

To get the first Jodels, open this as GET: "https://api.go-tellm.com/api/v2/posts/location/combo?lat={LAT}&lng={LNG}&access_token={YOUR_ACCESS_TOKEN}"

To obtain the rest, open this as GET:
"https://api.go-tellm.com/api/v2/posts/location?lng={LNG}&lat={LAT}&after={THE_LAST_POST_ID_OBTAINED_ABOVE}&access_token={YOUR_ACCESS_TOKEN}&limit=1000000"


### Get Karma

GET: "https://api.go-tellm.com/api/v2/users/karma?access_token={ACCESS_TOKEN}"


### More

Visit this link: http://jodel-app.wikia.com/


## Documentation

Jodels are returned as new List<Tuple<string, string, string, bool, int, string, string, Tuple<string>>> which is this: 

postid, message, hexcolor, isImage, votecount, lat, lng, name


Comments are returned as List<Tuple<string, string, string, int>> which is this: 

postID, message, user_handle, vote_count

# Setup

Add the vars and namespace properly:

```c#
using JodelAPI;

// ...

public static string accessToken = "";
public static string latitude = "";
public static string longitude = "";
public static string countryCode = "";
public static string city = "";
```

Baseclass is called 'API'.

# Get Jodels

Use GetAllJodels();


# Upvote & Downvote

You can use Upvote() and Downvote(). Remember that you can pass the postID (string) or the index (int) from the List as argument.
BUT: using index is only working with the cached version of the jodels which is automaticaly done after calling GetAllJodels(). 

# Karma

GetKarma() will return an int with your amount of Karma.


# Posting

PostJodel(string message) --> message is the text to post

TODO: PostComment


# Access Token

We were able to reverse their new Authentication system and now GenerateAccessToken() works! It returns a string containing the token.
Keep in mind that spamming the function will get your IP banned!


# Helpers

If you're not familiar with Tuples you can just use the simple function "FilterItem()". It can only handle the jodel List<> and only filter between postID and message.

Usage:
```c#
FilterItem(List<> jodels, int index, bool filterMessage);
```
index = position in the List<>

filterMessage = if true then it filters the message, else it will return the postID
