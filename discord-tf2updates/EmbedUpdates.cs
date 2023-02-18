using Discord;

namespace discordtf2updates
{
    public interface IEmbedUpdates
    {
        EmbedBuilder BuildTF2Embed(Newsitem newsitem);
    }

    public class EmbedUpdates : IEmbedUpdates
    {
        public EmbedBuilder BuildTF2Embed(Newsitem newsitem)
        {
            var converter = new ReverseMarkdown.Converter();

            string contentsToMd = converter.Convert(newsitem.contents);

            var embed = new EmbedBuilder
            {
                Title = newsitem.title,
                Description = contentsToMd,
                Color = Color.Orange,
                Url = newsitem.url,
            };
            return embed;
        }
    }
}