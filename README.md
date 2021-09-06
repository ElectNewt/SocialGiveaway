# SocialGiveaway
This project aims to provide the functionality to trigger social giveaways (raffle) using different social network APIs.

![SocialGiveaway pipeline result](https://github.com/ElectNewt/SocialGiveaway/actions/workflows/pipeline.yml/badge.svg)

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
If the rule is sent with no condition, the code will check the author of the tweet based on the `TweetId`.
```json
[
  {
    "rules": [
      {
        "type": "Follow"
      }
    ]
  }
]
```
there is an alternative, which is specifying the accounts to follow based on the `@TwitterAccount`:
```json
[
   {
      "rules":[
         {
            "type":"Follow",
            "conditions":[
               {
                  "subRule":"Follow",
                  "condition":"Account1"
               },
               {
                  "subRule":"Follow",
                  "condition":"Account2"
               }
            ]
         }
      ]
   }
]
```


#### Likes
By the TweetId gets the likes of a tweet.

Note: Limited to 100 likes due to a Twitter API limitation
- TODO: #3 Update Twitter Like Rule to get more than 100 likes.

#### Comments
Gets the users who replied to a tweet

Inside the comments you can evaluate Hashtags and Mentions. They are threaten as subRule.

##### Hashtags 
It allows you to filter by any hashtag used within the comment.

##### Mentions
It allows you to specify who should be mentioned. 
- note: the validation is case **insensitive**



#### Retweets
Gets the users who retweeted a tweet



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
