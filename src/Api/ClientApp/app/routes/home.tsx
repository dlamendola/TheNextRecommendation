import type {Route} from "./+types/home";
import {search} from "~/api";
import {Form, Link, useNavigation} from "react-router";
import {usePlaceholder} from "~/usePlaceholder";
import EpisodeCard from "~/components/EpisodeCard";

export function meta({}: Route.MetaArgs) {
    return [
        {title: "The Next Recommendation"},
    ];
}

export async function clientAction({request}: Route.ClientActionArgs) {
    const formData = await request.formData();
    const searchText = formData.get("query") as string;
    if (!searchText) {
        return undefined;
    }

    return await search(searchText);
}

export default function Home({actionData}: Route.ComponentProps) {
    const placeholder = usePlaceholder();
    const navigation = useNavigation();
    const isNavigating = Boolean(navigation.location);

    return (
        <div className="search">
            <div className="search-container">
                <h1>The Next Recommendation</h1>
                <Form method="post">
                    <input type="text" autoFocus name="query" className="search-input" placeholder={placeholder}/>
                    <button type="submit" disabled={isNavigating}>Search {isNavigating &&
                        <span id="search-spinner"/>}</button>
                </Form>
            </div>

            <div className="results">
                {actionData ? (
                    actionData.map((ep, i) => (
                        <EpisodeCard key={i} seasonNumber={ep.season} episodeNumber={ep.episode} title={ep.title}
                                     summary={ep.summary}/>
                    ))
                ) : null}
            </div>
        </div>
    );
}
