namespace SocialGiveaway.Dto.Twitter
{
    public record TwitterUserDto
    {
        public string Name { get; init; }
        public string At { get; init; }

        public TwitterUserDto(string name, string at)
        {
            Name = name;
            At = at;
        }
    }
}