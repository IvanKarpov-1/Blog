import { Profile } from "./profile";

export interface Article {
    id: string;
    createdDate: Date | null;
    title: string;
    description: string;
    content: string;
    isAuthor: boolean;
    author?: Profile;
    rubricName: string;
    rubricId: string;
}

export class Article implements Article {
    constructor(init?: ArticleFormValues) {
        Object.assign(this, init);
    }
}

export class ArticleFormValues {
    id?: string = undefined;
    title: string = '';
    description: string = '';
    content: string = '';
    createdDate: Date | null = new Date();
    rubricName: string = '';
    rubricId: string = '';

    constructor(article?: ArticleFormValues) {
        if (article) {
            this.id = article.id;
            this.title = article.title;
            this.description = article.description;
            this.content = article.content;
            this.rubricName = article.rubricName;
            this.rubricId = article.rubricId;
        }
    }
}