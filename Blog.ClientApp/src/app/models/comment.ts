import { Profile } from "./profile";

export interface AppComment {
    id: string;
    createdDate: Date | null;
    content: string;
    authorId: string;
    author: Profile;
    parentId: string;
    comments: AppComment[];
}

export class AppComment implements AppComment {
    constructor(init?: AppCommentFormValues) {
        Object.assign(this, init);
    }
} 

export class AppCommentFormValues {
    id?: string = undefined;
    content: string = '';
    parentId: string = '';

    constructor(comment?: AppCommentFormValues) {
        if (comment) {
            this.id = comment.id;
            this.content = comment.content;
            this.parentId = comment.parentId;
        }
    }
}