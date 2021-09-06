using ROP;
using SocialGiveaway.Dto.YouTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialGiveaway.Shared.Extensions;

namespace SocialGiveaway.Services.YouTube
{

    public interface ISelectYouTubeWinnerDependencies
    {
        Task<Result<List<(string channelUrl, string username)>>> GetChannelIdsWoCommented(string videoId);
        int GetRandomNumber(int start, int end);
    }
    public class SelectYouTubeWinner
    {

        private readonly ISelectYouTubeWinnerDependencies _dependencies;

        public SelectYouTubeWinner(ISelectYouTubeWinnerDependencies dependencies)
        {
            _dependencies = dependencies;
        }
        public async Task<Result<YouTubeUserDto>> Execute(string videoId, List<YouTubeTicketDto> youTubeTicketDtos)
        {

            List<Result<YoutubeTicketResult>> userIdWhoOnTickets = new();
            foreach (YouTubeTicketDto ticket in youTubeTicketDtos)
            {
                Result<YoutubeTicketResult> contenders = await GetContenders(videoId, ticket)
                .Bind(GetUsersThatFullFillTheRules);
                userIdWhoOnTickets.Add(contenders);
            }

            return userIdWhoOnTickets
                .Traverse()
                  .Bind(SelectYouTubeUsersWithOptions)
                  .Bind(GetWinner);
        }


        private Result<YouTubeUserDto> GetWinner(List<YouTubeUserDto> userIdWhoOnTickets)
        {
            int winner = _dependencies.GetRandomNumber(0, userIdWhoOnTickets.Count - 1);
            return userIdWhoOnTickets[winner];
        }

        private Result<List<YouTubeUserDto>> SelectYouTubeUsersWithOptions(List<YoutubeTicketResult> ticketResults)
        {
            return ticketResults
                .SelectMany(a => a.Users)
                .ToList();
        }

        private Result<YoutubeTicketResult> GetUsersThatFullFillTheRules(List<List<YouTubeUserDto>> rulesResult)
        {
            List<YouTubeUserDto> usersInRule = rulesResult.GetCommonItems();

            return new YoutubeTicketResult(usersInRule);
        }

        private async Task<Result<List<List<YouTubeUserDto>>>> GetContenders(string videoId, YouTubeTicketDto youTubeTicket)
        {
            List<Result<List<YouTubeUserDto>>> userContenders = new();

            foreach (var rule in youTubeTicket.Rules)
            {
                Result<List<YouTubeUserDto>> usersWhoAccomplishTheRule =
                    await YouTubeAction(rule, videoId)
                    .Map(ToYoutubeUser);
                userContenders.Add(usersWhoAccomplishTheRule);
            }


            return userContenders
                .Traverse();

            Task<List<YouTubeUserDto>> ToYoutubeUser(List<(string channelUrl, string username)> users)
            {
                return Task.FromResult(users.Select(a => new YouTubeUserDto(a.channelUrl, a.username))
                    .DistinctBy(a => a.ChannelUrl).ToList());
            }
        }

        private async Task<Result<List<(string channelUrl, string username)>>> YouTubeAction(YouTubeRule rule, string videoId)
            => rule switch
            {
                YouTubeRule.Comment => await _dependencies.GetChannelIdsWoCommented(videoId),
                _ => throw new NotImplementedException("seems like you're using a rule that is not configured."),
            };

        private record YoutubeTicketResult
        {
            public readonly IReadOnlyCollection<YouTubeUserDto> Users;

            public YoutubeTicketResult(List<YouTubeUserDto> users)
            {
                Users = users;
            }
        }
    }
}
