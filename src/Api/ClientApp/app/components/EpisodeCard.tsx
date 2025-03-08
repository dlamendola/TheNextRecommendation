import {Link} from "react-router";

export default function EpisodeCard({seasonNumber, episodeNumber, title, summary}: {seasonNumber: number; episodeNumber: number; title: string; summary: string})  {
    return (
        <div className="episode">
            <h3><Link to={`/app/season/${seasonNumber}/ep/${episodeNumber}`}>{`${title} (S${seasonNumber}E${episodeNumber})`}</Link></h3>
            <p>{summary}</p>
        </div>
    );
}