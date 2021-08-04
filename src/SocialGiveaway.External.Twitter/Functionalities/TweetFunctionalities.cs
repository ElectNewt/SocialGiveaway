namespace SocialGiveaway.External.Twitter.Functionalities
{
    public class TweetFunctionalities
    {
        public readonly TwitterLikes Likes;
        public readonly TwitterRetweets Retweets;
        public readonly TwitterComments Comments;
        public readonly TwitterFollowers Followers;
        public readonly TwitterAccount Account;

        public TweetFunctionalities(TwitterLikes likes, TwitterRetweets retweets,
            TwitterComments comments, TwitterFollowers followers, TwitterAccount account)
        {
            Likes = likes;
            Retweets = retweets;
            Comments = comments;
            Followers = followers;
            Account = account;
        }
    }
}
