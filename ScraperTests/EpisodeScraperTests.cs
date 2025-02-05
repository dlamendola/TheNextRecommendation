using Scraper;

namespace ScraperTests;

public class EpisodeScraperTests
{
    [Fact]
    public async Task ScrapeFirstEpisode()
    {
        var firstEpisodeId = "tt0094030";
        var expected = FirstEpisode;
        var scraper = new EpisodeScraper(firstEpisodeId);

        await foreach (var actual in scraper.ScrapeEpisodesAsync())
        {
            Assert.Equal(expected, actual);
            return;
        }
    }

    [Fact]
    public async Task ScrapesSubsequentEpisodes()
    {
        var firstEpisodeId = "tt0094030";
        var scraper = new EpisodeScraper(firstEpisodeId);
        var epNumber = 1;

        await foreach (var actual in scraper.ScrapeEpisodesAsync())
        {
            Assert.Equal(epNumber, actual.EpisodeInSeason);
            epNumber++;
            if (epNumber == 2)
            {
                return;
            }
        }
    }

    [Fact]
    public async Task ScrapeLastEpisode_ReturnsOneResult()
    {
        var lastEpisodeId = "tt0111281";
        var scraper = new EpisodeScraper(lastEpisodeId);
        var numResults = 0;

        await foreach (var _ in scraper.ScrapeEpisodesAsync())
        {
            numResults++;
            if (numResults > 1)
            {
                Assert.Fail("Expected one result, but got two");
            }
        }
    }

    private Episode FirstEpisode => new
    (
        Season: 1,
        EpisodeInSeason: 1,
        Title: "Encounter at Farpoint",
        NextEpisodeId: "tt0708810",
        Summary: "On the maiden mission of the U.S.S. Enterprise (NCC-1701-D), an omnipotent being known as Q challenges the crew to discover the secret of a mysterious base in an advanced and civilized fashion.",
        Synopsis: "Captain Jean-Luc Picard (Patrick Stewart); Commander William Riker (Jonathan Frakes); Lt. (j.g.) Geordi La Forge (LeVar Burton); Lt. Tasha Yar (Denise Crosby), security chief; Lt. (j.g.) Worf (Michael Dorn), a Klingon; Dr. (Cmdr.) Beverly Crusher (Gates McFadden). Medical officer; Counsellor (Lt. Cmdr.) Deanna Troi (Marina Sirtis), Psychic; Lt. Cmdr. Data (Brent Spiner), Android; Wesley Crusher (Wil Wheaton); Capt. Jean-Luc Picard takes command of the new Galaxy-class star ship, the fifth USS Enterprise. They are on route to Farpoint Station on Planet Deneb IV. Beyond Deneb IV is the great unexplored mass of the Galaxy. Picard is short his First Officer, who is scheduled to join him at Farpoint. The station was built by the natives of the planet who have now invited the Federation to inspect it and to use its facilities. Picard is supposed to negotiate the use of the station for the Federation, while trying to find out why the natives built it. They are stopped by Q (John De Lancie), a powerful member of a collective who refuses to let them continue and in fact puts them on trial for all of man's past transgressions. Q appears on the bridge of Enterprise and asks Picard to return to his own Solar System. Q calls humanity a dangerous, savage, child race. Just 400 years ago millions were slaughtered in world war II. Q vanishes after delivering his warning. Picard decides to outrun the gauntlet laid down by Q, by reversing engines and running at full warp speed. But Q follows and overtakes the Enterprise without much effort. Picard orders emergency detachment of the saucer. He transfers all non-combatants into the saucer and Worf is to captain it. Picard transfers to the battle bridge in the star drive to confront Q. Picard then orders a dead stop and surrenders the star drive to Q. Q transports the crew of the star drive to an ancient Earth court modeled after the horrors of the atomic wars on Earth. Q acts as Judge, Jury, and Prosecutor. He is adamant that humans have been savage in the past, murdering millions, and hence Picard has to answer as guilty and seek punishment for it. Picard pleads for Q to judge them based on how they act today, not in history. Q decides to accept the challenge and tells them the way they deal with the Farpoint issue will determine their fate. Picard, Data, Tasha and Troi are returned to the bridge of the Star Drive and allowed to proceed to Farpoint. Farpoint was built very quickly and seems to meet all the Federation's needs. There is even consideration of having them build other such stations. Riker is assigned as the First Officer of the Enterprise and is supposed to join the ship at the Farpoint station. Riker understands that the planet inhabitants use geothermal energy for their energy surpluses. This allows them to use very advanced materials for the construction of the space station. Riker and Dr Crusher & her son Wesley meet on the planet. Forge is also there, waiting to join the Enterprise crew. Riker shares his suspicions that the planet has an uncanny ability to produce exactly what you were wishing/hoping for at that moment. The administrator of the station, Groppler Zorn is shown to issue a warning to an unknown entity to stop displaying its acts of \"magic\" in front of visitors. Riker beams to the Enterprise when the Star Drive arrives. Data informs Picard that the saucer section will arrive in 51 mins. Picard tests Riker by ordering him to perform a manual docking with the saucer section. It is executed flawlessly. Picard welcomes Riker to the Enterprise. Geordi has damaged optic nerves and uses an instrument that converts visual images into brain waves to be fed directly to his brain. Data has been assigned the duty to transfer Admiral McCoy (DeForest Kelley) to USS Hood by shuttle craft, as he hates the transporter. Deanna and Riker have romantic feelings for each other. Dr Crusher's husband was killed serving with Picard. Picard is not sure if Dr Crusher wanted this assignment, and she staunchly declares that she requested this assignment aboard the Enterprise. Riker tells Picard that the planet gets surplus geothermal energy, which allowed them to create a star base to federation standards. Many materials used in the construction are not found on the planet at all. As they soon learn, it is all too good to be true and the station's chief Zorn (Michael Bell) is hiding something. Zorn is evasive about lending his engineers and architects to Starfleet and threatens to ally with the Ferengi instead, if Starfleet does not like the base built for them, as per their standards. Deanna senses pain and loneliness in a form near Zorn. Riker assembles an away team to inspect the star base closely. He avoids being too close to Deanna though. Deanna has sensed some passages underneath the star base and Riker assigns Deanna, Yar and Geordi to investigate. Riker and Data investigate topside. Deanna again senses extreme pain in the tunnels right beneath the star base. Riker and Data beam to their location. This is when an unknown star ship enters orbit around the planet Deneb IV. The ship is 12 times bigger than the Enterprise. Picard checks with Zorn, who says that the alliance with Ferengi was only a thought and an empty threat. The ship starts firing at the old Bandi city, leaving the Star base untouched. Picard orders Riker to enter the city, abduct Zorn and beam him to the Enterprise. Picard then orders Yar to lock Phasers onto the unidentified ship. At this point Q appears, it becomes clear that Picard and his crew are not dealing with the situation as he had expected or perhaps hoped. Q again charges humans of savagery and for showing no regard to save the lives of the Bandi city folk. Before Riker can abduct Zorn, he disappears in front of their eyes. Riker and Data beam back to the Enterprise, where Riker yells at Q and forces him to back off. Yar, Riker, Deanna and Data beam over to the unidentified vessel. They find similar tunnels as they found beneath the Star base. Deanna senses anger and hate, towards the old Bandi city. They find Zorn being tortured by an alien energy based being, who dissipates with phaser fire. That's when Q arrives on Enterprise and stops Picard from beaming his team back. Picard says he will do anything and begs for his team back, which Q does. But Deanna and Riker report that it wasn't Q, but the alien being who beamed them back. Zorn wants Picard to fire on the ship, but Picard wont. Turns out Zorn had found an injured alien being and helped it recover and learned that it could transport energy into any matter. But Zorn grew greedy and enslaved it underneath the new star base. That was the pain. This drew its mate, the alien spaceship, who fired on the Bandi city and was full of anger. The Alien ship converts into a type of space jelly fish. Picard fires on the Star base to release the alien mate. The pair unites and goes away. Q is satisfied and leaves Humans alone, for now."
    );
}