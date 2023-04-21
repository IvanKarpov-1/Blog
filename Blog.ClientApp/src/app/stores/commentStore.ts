import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { AppComment, AppCommentFormValues } from "../models/comment";
import { makeAutoObservable, runInAction } from "mobx";
import { store } from "./store";

export default class CommentStore {
    comments: AppComment[] = [];
    hubConnection: HubConnection | null = null;
    articleId: string = '';
    currentCommentId: string = '';

    constructor() {
        makeAutoObservable(this);
    }

    createHubConnection = (articleId: string) => {
        if (store.articleStore.selectedArticle) {
            this.articleId = articleId;
            this.hubConnection = new HubConnectionBuilder()
                .withUrl('http://localhost:5000/comment?commentableId=' + articleId, {
                    accessTokenFactory: () => store.userStore.user?.token!
                })
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();

            this.hubConnection.start().catch(error => console.log('Помилка встановлення підключення: ', error));

            this.hubConnection.on('LoadComments', (comments: AppComment[]) => {
                runInAction(() => {
                    comments.forEach(comment => {
                        comment.createdDate = new Date(comment.createdDate + 'Z');
                    })
                    this.comments = comments
                });
            })

            this.hubConnection.on('ReceiveCreatedComment', (comment: AppComment) => {
                runInAction(() => {
                    if (this.articleId === comment.parentId) {
                        this.comments.push(comment);
                    } else {
                        this.comments.some(currentComment => this.addRecievedComment(currentComment, comment.parentId, comment));
                    }
                });
            })

            this.hubConnection.on('ReceiveUpdatedComment', (comment: AppComment) => {
                runInAction(() => {
                    this.comments.some(currentComment => this.updateRecievedComment(currentComment, comment));
                })
            })

            this.hubConnection.on('ReceiveDeletedCommentId', (id: string) => {
                runInAction(() => {
                    if (this.comments.find(comment => comment.id === id)) {
                        this.comments = this.comments.filter(comment => comment.id !== id);
                    } else {
                        this.comments.some(currentComment => this.deleteRecievedComment(currentComment, id));
                    }
                })
            })
        }
    }

    private addRecievedComment = (currentComment: AppComment, parentId: string, commentToAdd: AppComment): boolean => {
        if (currentComment.id === parentId) {
            currentComment.comments.push(commentToAdd);
            return true;
        }

        return currentComment.comments.some(comment => this.addRecievedComment(comment, parentId, commentToAdd));
    }

    private updateRecievedComment = (currentComment: AppComment, updatedComment: AppComment): boolean => {
        if (currentComment.id === updatedComment.id) {
            currentComment.content = updatedComment.content;
            return true;
        }

        return currentComment.comments.some(comment => this.updateRecievedComment(comment, updatedComment));
    }

    private deleteRecievedComment = (currentComment: AppComment, id: string): boolean => {
        if (currentComment.comments.find(comment => comment.id === id)) {
            currentComment.comments = currentComment.comments.filter(comment => comment.id !== id);
            return true;
        }

        return currentComment.comments.some(comment => this.deleteRecievedComment(comment, id));
    }

    stopHubConnection = () => {
        this.hubConnection?.stop().catch(error => console.log("Помилка припинення з'єднання: ", error));
        this.articleId = '';
        this.currentCommentId = '';
    }

    clearComments = () => {
        this.comments = [];
        this.stopHubConnection();
    }

    addComment = async (comment: AppCommentFormValues) => {
        try {
            const values: any = {};
            values.content = comment.content;
            values.parentId = comment.parentId;
            await this.hubConnection?.invoke('SendComment', values, this.articleId)
        } catch (error) {
            console.log(error);
        }
    }

    updateComment = async (comment: AppCommentFormValues) => {
        try {
            await this.hubConnection?.invoke('EditComment', comment, this.articleId)
        } catch (error) {
            console.log(error);
        }
    }

    deleteComment = async (id: string) => {
        try {
            const comment: any = {}
            comment.id = id;
            await this.hubConnection?.invoke('DeleteComment', comment, this.articleId);
        } catch (error) {
            console.log(error)
        }
    }

    loadComment = async (id: string) => {
        const comment = this.getComment(this.comments, id);
        return comment;
    }

    private getComment = (comments: AppComment[], id: string): AppComment | undefined => {
        for (const comment of comments) {
            if (comment.id === id) {
                return comment;
            }

            const childComment = this.getComment(comment.comments, id);
            if (childComment) {
                return childComment;
            }
        }

        return undefined;
    }

    setCurrentComment = (id: string) => {
        this.currentCommentId = id;
    }
}