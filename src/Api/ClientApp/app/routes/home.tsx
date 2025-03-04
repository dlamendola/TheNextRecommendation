import type { Route } from "./+types/home";
import {search} from "~/api";
import {Form, useNavigation} from "react-router";

export function meta({}: Route.MetaArgs) {
  return [
    { title: "The Next Recommendation" },
  ];
}

export async function clientAction({request}: Route.ClientActionsArgs) {
  const formData = await request.formData();
  const searchText = formData.get("query");

  const episodes = await search(searchText);
  return episodes;
}

export default function Home({actionData}: Route.ComponentProps<any>) {
  const navigation = useNavigation();
  const isNavigating = Boolean(navigation.location);
  
  return (
      <div className="search">
        <div className="search-container">
          <h1>The Next Recommendation</h1>
          <Form method="post">
            <input type="text" name="query" className="search-input"/>
            <button type="submit" disabled={isNavigating}>Search</button>
          </Form>
        </div>

        <div className="results">
          {isNavigating && <p>Loading...</p>}
          {actionData ? (
              actionData.episodes.map((ep, i) => (
                  <div key={i} className="episode">
                    <h3>{`${ep.title} (S${ep.season}E${ep.episode}`}</h3>
                    <p>{ep.summary}</p>
                  </div>
              ))
          ) : null}
        </div>
      </div>
  )
}
