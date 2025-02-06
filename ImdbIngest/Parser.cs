using System.Text.Json.Nodes;
using System.Web;

namespace ImdbIngest;

public static class Parser
{
    private static readonly ReadOnlyMemory<char> StartOfJson = "<script id=\"__NEXT_DATA__\" type=\"application/json\">".AsMemory();
    private static readonly ReadOnlyMemory<char> EndOfJson = "</script>".AsMemory();

    public static Episode ParseHtml(string html)
    {
        var node = ParseJson(html);
        return BuildEpisodeFromJsonNode(node);
    }

    private static JsonNode? ParseJson(string html)
    {
        var jsonString = ExtractJsonString(html);
        var node = JsonNode.Parse(jsonString);
        return node;
    }
    
    private static string ExtractJsonString(string html)
    {
        var htmlSpan = html.AsSpan();
        var startSpan = StartOfJson.Span;
        var endSpan = EndOfJson.Span;
        
        var startIndex = htmlSpan.IndexOf(startSpan, StringComparison.Ordinal);
        if (startIndex == -1) return string.Empty;
        
        var afterStart = htmlSpan[(startIndex + startSpan.Length)..];
        var endIndex = afterStart.IndexOf(endSpan, StringComparison.Ordinal);
        if (endIndex == -1) return string.Empty;
        
        return afterStart.Slice(0, endIndex).ToString();
    }

    private static Episode BuildEpisodeFromJsonNode(JsonNode? node)
    {
        return new Episode(
            Season: node.ToSeason(),
            EpisodeInSeason: node.ToEpisodeInSeason(),
            Title: node.ToTitle(),
            NextEpisodeId: node.ToNextEpisodeId(),
            Summary: node.ToSummary(),
            Synopsis: node.ToSynopsis()
        );
    }

    private static string ToSynopsis(this JsonNode node)
    {
        return HttpUtility.HtmlDecode(
                node["props"]["pageProps"]["contentData"]["data"]["title"]["plotSynopsis"]["edges"][0]["node"]["plotText"]["plaidHtml"].ToString())
            .Replace("<br/>", " ")
            .Replace("\n", " ")
            .Replace("  ", " ");
    }

    private static string ToSummary(this JsonNode node)
    {
        return HttpUtility.HtmlDecode(
            node["props"]["pageProps"]["contentData"]["entityMetadata"]["plot"]["plotText"]["plainText"].ToString());
    }

    private static string ToNextEpisodeId(this JsonNode node)
    {
        return node["props"]["pageProps"]["contentData"]["entityMetadata"]["series"]["nextEpisode"]?["id"].ToString();
    }

    private static string ToTitle(this JsonNode node)
    {
        return node["props"]["pageProps"]["contentData"]["entityMetadata"]["titleText"]["text"].ToString();
    }

    private static int ToEpisodeInSeason(this JsonNode node)
    {
        return int.Parse(node["props"]["pageProps"]["contentData"]["entityMetadata"]["series"]["episodeNumber"]["episodeNumber"].ToString());
    }

    private static int ToSeason(this JsonNode node)
    {
        return int.Parse(
            node["props"]["pageProps"]["contentData"]["entityMetadata"]["series"]["episodeNumber"]["seasonNumber"]
                .ToString());
    }
}