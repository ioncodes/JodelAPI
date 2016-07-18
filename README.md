# JodelAPI
[WIP] API for the Jodel app in .NET

## How the private Jodel API works...

The API is using 3 paramters in general: lat, lng, access_token.
lat: This is the latitude of your location.
lng: This is the longitude of your location.
access_token: In the app, navigate to the options and click on "write us", then scroll down to "client info". There is something called "access_token=". The token is the string after the "=".


### Getting the Jodels

To get the first Jodels, open this as GET: "https://api.go-tellm.com/api/v2/posts/location/combo?lat={LAT}&lng={LNG}&access_token={YOUR_ACCESS_TOKEN}"

To obtain the rest, open this as GET:
"https://api.go-tellm.com/api/v2/posts/location?lng={LNG}&lat={LAT}&after={THE_LAST_POST_ID_OBTAINED_ABOVE}&access_token={YOUR_ACCESS_TOKEN}&limit=1000000"


### Get Karma

GET: "https://api.go-tellm.com/api/v2/users/karma?access_token={ACCESS_TOKEN}"


### More

Visit this link: http://jodel-app.wikia.com/


## Documentation

Jodels are returned as List<Tuple<string, string, string, bool>> which is this: postID, message, hexcolor, isImage
Comments are returned as List<Tuple<string, string, string, int>> which is this: postID, message, user_handle, vote_count

# Setup

Add the vars properly:

public static string accessToken = "";
public static string latitude = "";
public static string longitude = "";
public static string countryCode = "";
public static string city = "";

Add the namespace "JodelAPI" and the baseclass is called "API".


# Get Jodels

Use GetAllJodels();


# Upvote & Downvote

You can use Upvote() and Downvote(). Remember that you can pass the postID (string) or the index (int) from the List as argument.
BUT: using index is only working with the cached version of the jodels which is automaticaly done after calling GetAllJodels(). 

# Karma

GetKarma() will return an int with your amount of Karma.


# Posting

PostJodel(string message, string color) --> message is the text to post and color is optional; you can pass the background color, but it must be one of the official ones.

TODO: PostComment


# Access Token

With GenerateAccessToken you will be able to generate a new token; it's not working at the moment.


# Helpers

If you're not familiar with Tuples you can just use the simple function "FilterItem()". It can only handle the jodel List<> and only filter between postID and message.
Usage:
FilterItem(List<> jodels, int index, bool filterMessage);
index = position in the List<>
filterMessage = if true then it filters the message, else it will return the postID
