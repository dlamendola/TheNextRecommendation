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

document.getElementById('searchInput').addEventListener('keypress', function(e) {
    if (e.key === 'Enter') {
        searchEpisodes();
    }
});

const searchInput = document.getElementById('searchInput'); // Reference to the input field

const placeholders = [
    'Try: "ethical dilemmas involving AI"',
    'Try: "time paradoxes"',
    'Try: "cultural misunderstandings"',
    'Try: "leadership lessons"',
    'Try: "military ethics"',
    'Try: "trust and betrayal"',
    'Try: "technological advancement consequences"',
];

let currentIndex = 0;
let placeholderInterval;

function updatePlaceholder() {
    searchInput.setAttribute('placeholder', placeholders[currentIndex]);
    currentIndex = (currentIndex + 1) % placeholders.length;
}

function startPlaceholderRotation() {
    placeholderInterval = setInterval(updatePlaceholder, 4000); // Update every 4 seconds
}

function stopPlaceholderRotation() {
    clearInterval(placeholderInterval);
}

searchInput.addEventListener('focus', stopPlaceholderRotation);
searchInput.addEventListener('blur', startPlaceholderRotation);

updatePlaceholder();
startPlaceholderRotation()