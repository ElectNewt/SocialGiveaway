# SocialGiveaway
This project aims to provide the functionality to trigger social giveaways (raffle) using different social network APIs.

## Prerequisites
- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)

### Dependencies
* [Netmentor.DiContainer](https://github.com/ElectNewt/Netmentor.DiContainer)
* [RailwayOrientedProgramming](https://github.com/ElectNewt/EjemploRop)
* [Tweetinvi](https://github.com/linvi/tweetinvi) only twitter.

Note: this is still a development version. Things like the token to access the twitter API is not implemented automatically.  The configuration can specify it in the `appsettings.json` or `appsettings.private.json`, which will not get committed in Git. 

## How it works
The entire application is based on `Tickets`, those `Tickets` are based on `Rules`, and *all the rules* in a `Ticket` must fulfil to get that ticket.

### Example Giveaway 
In a typical giveaway on Twitter, You may set Retweet + Follow the account to get one "ticket", but you may want to add more interaction like, "add a comment to increase your chances", which translates into another ticket inside the application.

Explanation:
1 Raffle
- Ticket1: Follow + RT (Two rules)
- Ticket2: Comment (One rule)

Any of the users who fulfil any of the tickets will have a chance to win.

## Twitter Giveaway
Note: the application is still in development, some of the code is done using [Tweetinvi](https://github.com/linvi/tweetinvi), but for me, `GetBearerToken` was not working. At the moment, the token has to be specified in the `appsettings.json`.


Remember that you will be limited by the [Twitter API limitations](https://developer.twitter.com/en/docs/twitter-api/rate-limits)

### Twitter Rules
#### Follow
By the `TweetId`, it gets the Author and checks who follows him.

- TODO: #1 Specify the account that should be followed.

#### Likes
By the TweetId gets the likes of a tweet.

Note: Limited to 100 likes due to a Twitter API limitation
- TODO: #3 Update Twitter Like Rule to get more than 100 likes.

#### Comments
Gets the users who replied to a tweet

#### Retweets
Gets the users who retweeted a tweet

#### CommentPlusQuote
TODO: (no task created, #2 related)

#### Hashtag
TODO: (no task created)


## YouTube Giveaway
Due to the limitations of the YouTube API  the app will support only calls done with the APi Key [YouTube Documentation](https://developers.google.com/youtube/v3/getting-started) 

### YouTube Rules
#### Comments
Gets the comments in a video.



## GitHub
### GitHub Rules
#### Stared a repo
TODO: #7



## Issues and contribution
Please do not hesitate in adding some issues or contribute to the code.
