# JodelAPI
[WIP] API for the Jodel app in .NET

## How the private Jodel API works...

The API is using 3 paramters in general: lat, lng, access_token.
lat: This is the latitude of your location.
lng: This is the longitude of your location.
access_token: This one is kinda difficult to obtain. It's the "password"/"key" to use the API and as authentication for your own Jodels. To obtain your own key, use mitmproxy and open the Jodel app. (Tutorial coming soon!)


### Getting the Jodels

To get the first Jodels, open this as GET: "https://api.go-tellm.com/api/v2/posts/location/combo?lat={LAT}&lng={LNG}&access_token={YOUR_ACCESS_TOKEN}"

To obtain the rest, open this as GET:
"https://api.go-tellm.com/api/v2/posts/location?lng={LNG}&lat={LAT}&after={THE_LAST_POST_ID_OBTAINED_ABOVE}&access_token={YOUR_ACCESS_TOKEN}&limit=1000000"


### Get Karma

GET: "https://api.go-tellm.com/api/v2/users/karma?access_token={ACCESS_TOKEN}"
