# JodelAPI (Not working on this anymore)
[![forthebadge](http://forthebadge.com/images/badges/built-with-swag.svg)](http://forthebadge.com)
[![forthebadge](http://forthebadge.com/images/badges/gluten-free.svg)](http://forthebadge.com)
[![forthebadge](http://forthebadge.com/images/badges/certified-snoop-lion.svg)](http://forthebadge.com)

*Big thanks to KirschbaumP*

[![Build status](https://ci.appveyor.com/api/projects/status/2dx3f591ubmp978t?svg=true)](https://ci.appveyor.com/project/ioncodes/jodelapi)
[![Github All Releases](https://img.shields.io/github/downloads/ioncodes/JodelAPI/total.svg)](https://github.com/ioncodes/JodelAPI/releases)
[![NuGet](https://img.shields.io/nuget/v/JodelAPI.svg)](https://www.nuget.org/packages/JodelAPI/)
[![Status](https://img.shields.io/badge/api-working-brightgreen.svg)]()
[![Implementation](https://img.shields.io/badge/api--version-4.47.0-brightgreen.svg)]()
[![Test status](http://teststatusbadge.azurewebsites.net/api/status/ioncodes/jodelapi)](https://ci.appveyor.com/project/ioncodes/jodelapi)
[![Gitter chat](https://badges.gitter.im/ioncodes/JodelAPI.svg)](https://gitter.im/JodelAPI/Lobby?utm_source=share-link&utm_medium=link&utm_campaign=share-link)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=4JTJ7KE332VBE)

## READ THIS FIRST

I received a message from the Jodel Team, and they would like me to close this repo. We are currently in a discussion with them, if it's possible to let this repo open for educational purposes.
**However, I'm not allowed to work on this library until I receive their permission**  
**All GET requests are ok to use.**

## INFO

* Jodel dropped the captcha support, which means you wont be able to verify new accounts. If you have verified accounts, they will continue to work on 4.47.0, but if you use the 4.48.0 version, they will get unverified again, so use the 4.48.0 branch with caution!

## Support

You can show me your love by paying me a coffee :) You can find the PayPal page at the badge bar.

## Introduction

This is the carefully reverse-engineered .NET Jodel API in C#. Please feel free to grab the source or compiled library from the link provided below.

*This is a Swiss software product and therefore might be awesome!*

## Binaries and pre-requisites
* [GitHub Releases](https://github.com/ioncodes/JodelAPI/releases) or from Nuget

## Let's get right into it!
Create a Jodel object which holds all functions needed:
```cs
Jodel jodel = new Jodel(string place, string countrycode, string city, bool createToken = false); // YOU HAVE TO SET YOUR OWN TOKEN
```
Where 'place' is the place how you would enter the location in Google Maps, 'countrycode' and 'city' are the values that are sent to Jodel! You might have your own token, in that case you can set createToken to false (You have to). If you do this, make sure to set the data found in the AccessToken (Account.AccessToken) class:
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

After creating your 'Jodel' object, you will find all methods you need in there.

A plain list of all supported endpoints can be found [here](https://github.com/ioncodes/JodelAPI/blob/master/JodelAPI/JodelAPI/Internal/Links.cs)
