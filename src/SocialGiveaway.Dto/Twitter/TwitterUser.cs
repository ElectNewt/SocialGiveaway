namespace SocialGiveaway.Dto.Twitter
{
    public record TwitterUser
    {
        public readonly int Id;

        public TwitterUser(int id)
        {
            Id = id;
        }
    }
}