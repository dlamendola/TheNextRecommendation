import {useEffect, useState} from "react";

const placeholders = [
    'Try: "ethical dilemmas involving AI"',
    'Try: "time paradoxes"',
    'Try: "cultural misunderstandings"',
    'Try: "leadership lessons"',
    'Try: "military ethics"',
    'Try: "trust and betrayal"',
    'Try: "technological advancement consequences"',
]

export function usePlaceholder() {
    const [placeholder, setPlaceholder] = useState<string>();
    useEffect(() => {
        let currentIndex = 0;
        setPlaceholder(placeholders[0]);
        const intervalId = setInterval(() => {
            currentIndex = (currentIndex + 1) % placeholders.length;
            setPlaceholder(placeholders[currentIndex]);
        }, 4000);

        return () => clearInterval(intervalId);
    }, []);

    return placeholder;
}