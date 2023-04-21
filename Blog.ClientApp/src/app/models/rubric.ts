export interface Rubric {
    id: string;
    createdDate: Date | null;
    name: string;
    parent?: string;
    rubrics?: Rubric[];
}