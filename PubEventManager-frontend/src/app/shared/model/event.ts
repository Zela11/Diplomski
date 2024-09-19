export interface EventModel {
    id: number;
    name: string;
    description: string;
    date: Date;
    managerId: number;
    image?: File;
    imageSrc?: string; // Dodaj ovo za Base64 kodiranu sliku
}