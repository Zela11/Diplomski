export interface EventGet {
    id: number;
    name: string;
    description: string;
    date: Date;
    managerId: number;
    image?: string; // Add an optional image property
}