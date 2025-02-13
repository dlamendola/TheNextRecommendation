async function searchEpisodes() {
    const searchText = document.getElementById('searchInput').value;
    if (!searchText) return;
    const resultsDiv = document.getElementById('results');

    try {
        const request = new Request("/search", { method: 'POST',
            body: JSON.stringify({searchText: searchText}),
            headers: { 'Content-Type': 'application/json' }});
        const response = await fetch(request);
        const data = await response.json();

        resultsDiv.innerHTML = data.episodes.map(episode => `
                    <div class="episode">
                        <h3>${episode.title}</h3>
                        <p>${episode.summary}</p>
                    </div>
                `).join('');

    } catch (error) {
        resultsDiv.innerHTML = '<p>Error fetching results. Please try again.</p>';
        console.error('Error:', error);
    }
}

// Add event listener for Enter key
document.getElementById('searchInput').addEventListener('keypress', function(e) {
    if (e.key === 'Enter') {
        searchEpisodes();
    }
});