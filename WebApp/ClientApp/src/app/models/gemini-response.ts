export interface GeminiResponse {
    candidates: Candidate[];
    promptFeedback: string;
}

interface Candidate {
    content: Content;
    finishReason: string;
    index: number;
    safetyRatings: SafetyRating[];
}

interface Content {
    parts: Part[];
    role: string;
}

interface Part {
    text: string;
}

interface SafetyRating {
    category: string;
    probability: string;
}
