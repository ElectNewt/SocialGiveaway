namespace SocialGiveaway.Dto.Twitter
{
    public record TwitterUser
    {
        public readonly long Id;

        public TwitterUser(long id)
        {
            Id = id;
        }
    }
}