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
