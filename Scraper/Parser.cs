using System.Text.Json.Nodes;
using System.Web;

namespace Scraper;

public static class Parser
{
    private const string StartOfJson = "<script id=\"__NEXT_DATA__\" type=\"application/json\">";
    private const string EndOfJson = "</script>";

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
        var indexOfNextData = html.IndexOf(StartOfJson, StringComparison.Ordinal);
        var restOfPage = html.Substring(indexOfNextData);
        var endOfJson = restOfPage.IndexOf(EndOfJson, StringComparison.Ordinal);
        var substringLength = endOfJson - StartOfJson.Length;
        var line = restOfPage.Substring(StartOfJson.Length, substringLength);
        return line;
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