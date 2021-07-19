using System.Threading.Tasks;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using SocialGiveaway.External.Twitter.Credentials;
using ROP;
using System.Collections.Generic;

namespace SocialGiveaway.External.Twitter.Functionalities
{
    public class Youtube
    {

        private readonly IYouTubeClientFactory _youTubeClientFactory;

        public Youtube(IYouTubeClientFactory youTubeClientFactory)
        {
            _youTubeClientFactory = youTubeClientFactory;
        }

        public async Task<Result<List<(string channelUrl, string username)>>> GetCommentsAuthorsOnVideo(string videoId)
        {
            List<(string channelUrl, string username)> comments = new();

            YouTubeService client = _youTubeClientFactory.GetYouTubeClient();

            var nextPageToken = "";
            while (nextPageToken != null)
            {
                var commentThreads = client.CommentThreads.List("snippet,replies");
                commentThreads.VideoId = videoId;
                commentThreads.MaxResults = 50;
                commentThreads.PageToken = nextPageToken;

                // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
                CommentThreadListResponse commentsThreadsResponse = await commentThreads.ExecuteAsync();
                foreach (var commentThread in commentsThreadsResponse.Items)
                {
                    string channelUrl = commentThread.Snippet.TopLevelComment.Snippet.AuthorChannelUrl;
                    string displayName = commentThread.Snippet.TopLevelComment.Snippet.AuthorDisplayName;
                    comments.Add((channelUrl, displayName));
                }
                nextPageToken = commentsThreadsResponse.NextPageToken;
            }
            return comments;
        }
    }
}
