using Scraper;

namespace ScraperTests;

public class ParserTests
{
    [Fact]
    public void ParseHtml_Ep1()
    {
        var html = File.ReadAllText("testFiles/s1e1.html");
        var expected = new Episode(
            Season: 1, 
            EpisodeInSeason: 1, 
            Title: "Encounter at Farpoint", 
            NextEpisodeId: "tt0708810",
            Summary: "On the maiden mission of the U.S.S. Enterprise (NCC-1701-D), an omnipotent being known as Q challenges the crew to discover the secret of a mysterious base in an advanced and civilized fashion.",
            Synopsis: "Captain Jean-Luc Picard (Patrick Stewart); Commander William Riker (Jonathan Frakes); Lt. (j.g.) Geordi La Forge (LeVar Burton); Lt. Tasha Yar (Denise Crosby), security chief; Lt. (j.g.) Worf (Michael Dorn), a Klingon; Dr. (Cmdr.) Beverly Crusher (Gates McFadden). Medical officer; Counsellor (Lt. Cmdr.) Deanna Troi (Marina Sirtis), Psychic; Lt. Cmdr. Data (Brent Spiner), Android; Wesley Crusher (Wil Wheaton); Capt. Jean-Luc Picard takes command of the new Galaxy-class star ship, the fifth USS Enterprise. They are on route to Farpoint Station on Planet Deneb IV. Beyond Deneb IV is the great unexplored mass of the Galaxy. Picard is short his First Officer, who is scheduled to join him at Farpoint. The station was built by the natives of the planet who have now invited the Federation to inspect it and to use its facilities. Picard is supposed to negotiate the use of the station for the Federation, while trying to find out why the natives built it. They are stopped by Q (John De Lancie), a powerful member of a collective who refuses to let them continue and in fact puts them on trial for all of man's past transgressions. Q appears on the bridge of Enterprise and asks Picard to return to his own Solar System. Q calls humanity a dangerous, savage, child race. Just 400 years ago millions were slaughtered in world war II. Q vanishes after delivering his warning. Picard decides to outrun the gauntlet laid down by Q, by reversing engines and running at full warp speed. But Q follows and overtakes the Enterprise without much effort. Picard orders emergency detachment of the saucer. He transfers all non-combatants into the saucer and Worf is to captain it. Picard transfers to the battle bridge in the star drive to confront Q. Picard then orders a dead stop and surrenders the star drive to Q. Q transports the crew of the star drive to an ancient Earth court modeled after the horrors of the atomic wars on Earth. Q acts as Judge, Jury, and Prosecutor. He is adamant that humans have been savage in the past, murdering millions, and hence Picard has to answer as guilty and seek punishment for it. Picard pleads for Q to judge them based on how they act today, not in history. Q decides to accept the challenge and tells them the way they deal with the Farpoint issue will determine their fate. Picard, Data, Tasha and Troi are returned to the bridge of the Star Drive and allowed to proceed to Farpoint. Farpoint was built very quickly and seems to meet all the Federation's needs. There is even consideration of having them build other such stations. Riker is assigned as the First Officer of the Enterprise and is supposed to join the ship at the Farpoint station. Riker understands that the planet inhabitants use geothermal energy for their energy surpluses. This allows them to use very advanced materials for the construction of the space station. Riker and Dr Crusher & her son Wesley meet on the planet. Forge is also there, waiting to join the Enterprise crew. Riker shares his suspicions that the planet has an uncanny ability to produce exactly what you were wishing/hoping for at that moment. The administrator of the station, Groppler Zorn is shown to issue a warning to an unknown entity to stop displaying its acts of \"magic\" in front of visitors. Riker beams to the Enterprise when the Star Drive arrives. Data informs Picard that the saucer section will arrive in 51 mins. Picard tests Riker by ordering him to perform a manual docking with the saucer section. It is executed flawlessly. Picard welcomes Riker to the Enterprise. Geordi has damaged optic nerves and uses an instrument that converts visual images into brain waves to be fed directly to his brain. Data has been assigned the duty to transfer Admiral McCoy (DeForest Kelley) to USS Hood by shuttle craft, as he hates the transporter. Deanna and Riker have romantic feelings for each other. Dr Crusher's husband was killed serving with Picard. Picard is not sure if Dr Crusher wanted this assignment, and she staunchly declares that she requested this assignment aboard the Enterprise. Riker tells Picard that the planet gets surplus geothermal energy, which allowed them to create a star base to federation standards. Many materials used in the construction are not found on the planet at all. As they soon learn, it is all too good to be true and the station's chief Zorn (Michael Bell) is hiding something. Zorn is evasive about lending his engineers and architects to Starfleet and threatens to ally with the Ferengi instead, if Starfleet does not like the base built for them, as per their standards. Deanna senses pain and loneliness in a form near Zorn. Riker assembles an away team to inspect the star base closely. He avoids being too close to Deanna though. Deanna has sensed some passages underneath the star base and Riker assigns Deanna, Yar and Geordi to investigate. Riker and Data investigate topside. Deanna again senses extreme pain in the tunnels right beneath the star base. Riker and Data beam to their location. This is when an unknown star ship enters orbit around the planet Deneb IV. The ship is 12 times bigger than the Enterprise. Picard checks with Zorn, who says that the alliance with Ferengi was only a thought and an empty threat. The ship starts firing at the old Bandi city, leaving the Star base untouched. Picard orders Riker to enter the city, abduct Zorn and beam him to the Enterprise. Picard then orders Yar to lock Phasers onto the unidentified ship. At this point Q appears, it becomes clear that Picard and his crew are not dealing with the situation as he had expected or perhaps hoped. Q again charges humans of savagery and for showing no regard to save the lives of the Bandi city folk. Before Riker can abduct Zorn, he disappears in front of their eyes. Riker and Data beam back to the Enterprise, where Riker yells at Q and forces him to back off. Yar, Riker, Deanna and Data beam over to the unidentified vessel. They find similar tunnels as they found beneath the Star base. Deanna senses anger and hate, towards the old Bandi city. They find Zorn being tortured by an alien energy based being, who dissipates with phaser fire. That's when Q arrives on Enterprise and stops Picard from beaming his team back. Picard says he will do anything and begs for his team back, which Q does. But Deanna and Riker report that it wasn't Q, but the alien being who beamed them back. Zorn wants Picard to fire on the ship, but Picard wont. Turns out Zorn had found an injured alien being and helped it recover and learned that it could transport energy into any matter. But Zorn grew greedy and enslaved it underneath the new star base. That was the pain. This drew its mate, the alien spaceship, who fired on the Bandi city and was full of anger. The Alien ship converts into a type of space jelly fish. Picard fires on the Star base to release the alien mate. The pair unites and goes away. Q is satisfied and leaves Humans alone, for now."
        );
        
        var actual = Parser.ParseHtml(html);
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ParseHtml_S5E2()
    {
        var html = File.ReadAllText("testFiles/s5e2.html");
        var expected = new Episode(
            Season: 5,
            EpisodeInSeason: 2,
            Title: "Darmok",
            NextEpisodeId: "tt0708708",
            Summary: "Picard must learn to communicate with a race that speaks in metaphor under a difficult set of circumstances.",
            Synopsis: """
                      The Enterprise approaches the uninhabited El-Adrel solar system, near the territory of the enigmatic, presumably pacific race known as the Children of Tamar, which is now establishing contact. Federation has encountered Tamarian ships 7 times in the last 100 years, but each time relations could not be established since the Tamarians cannot be comprehended. Language has proven to be a barrier. Even this time they have sent a code, a mathematical progression in their hail to the Enterprise. Picard expected fairly easy diplomacy, but the Tamarians are truly incomprehensible. They repeat phrases which don't mean anything to humans. Rai & Jiri at Lungha. Rai of Lowani. Lowani under two moons. Jiri of Ubaya. Ubaya of crossroads at Lungha. Lungha, her sky gray. Deanna can sense only good intentions from the Tamarians. There are no hostile intentions. They transport Picard and their own captain Dathon (Paul Winfield) down to the nearest planet surface. The planet is made impenetrable behind a particle shield extended by the Tamarian ship. Hence it is not possible to beam Picard back. The object seems to be to pit them for a duel, which Picard is unwilling to engage. Dathon throws a dagger at Picard, which he throws back. Soon it is night and the temperature drops. The 2 captains are less than 20 m from each other and yet build their own fires to survive. Both the captain and his crew on the bridge desperately study the Tamarian language, which they find to be unusually focused on mythical narrative, notably the epic of Darmok. Riker sends a shuttle to rescue Picard. But the Tamarians shoot at the aft propulsion, just enough to turn it back, but not enough to cause it any damage. Ensign Robin Lefler (Ashley Judd) works with Geordi to improve transporter efficiency to beam Picard off the surface. Meanwhile Picard and Dathon are attacked by an entity which is Electro-magnetic in nature. Riker orders Geordi to use what they have and try to beam Picard off the planet, since the Enterprise sensors can also see the entity moving towards Picard's and Dathon's position. Picard starts understanding some of the phrases used by Dathon, but still can't comprehend their meaning. Meanwhile Deanna and Data do their research and try to find the meaning of the language. Picard figures that the Tamarians communicate by citing examples, by metaphors. As Picard understands, the entity attacks. Picard and Dathon work together to try and defeat the entity, but that's when Enterprise tries to beam him back. Picard is unable to help Dathon as he is attacked by the entity. Eventually the beam up fails, when Riker appeals to the Tamarians to withdraw their shield, but the Tamarians refuse to talk to them and shut down the channels. In fact, the Tamarians intensify the particle field around the planet, making any beam up impossible. Dathon is injured badly. Deanna and Data tell Riker that Tamarians communicate in metaphors. It's like saying Juliet on the balcony. But unless you know what Juliet was and what she was doing on the balcony, the phrase is meaningless. The language holds the key to its metaphorical 'code', and thus to the whole situation. Picard spends another night with Dathon, but this time he shares his own stories from Earth and tries to keep him awake. In the morning, Dathon dies, and the entity prepares to attack Picard Picard learns that the whole situation was planned by the Tamarians. They knew that there was a dangerous entity on the planet. They planned that sharing and facing this danger together would bring the 2 races together and help them understand each other. Riker attacks the particle generator on the Tamarian ship and creates a gap to beam Picard over, just before the entity attacks. The Tamarians attack the Enterprise and are about to destroy it when Picard enters the bridge and talks to the first officer in their own metaphors. The first officer understands, and a new metaphor is created that day: Picard and Dathon at El-Adrel.
                      """
        );
        
        var actual = Parser.ParseHtml(html);
        
        Assert.Equal(expected, actual);
    }
    
}