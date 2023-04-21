import { makeAutoObservable, reaction, runInAction } from 'mobx';
import { Article, ArticleFormValues } from '../models/article'
import agent from "../api/agent";
import { store } from './store';
import { router } from '../router/Routes';
import { Pagination, PagingParams } from '../models/pagination';

export default class ArticleStore {
    articleRegistry = new Map<string, Article>();
    selectedArticle: Article | undefined = undefined;
    editMode = false;
    loading = false;
    loadingIntial = false;
    pagination: Pagination | null = null;
    pagingParams = new PagingParams();
    predicate = new Map().set('all', true);

    constructor() {
        makeAutoObservable(this);

        reaction(
            () => this.predicate.keys(),
            () => {
                this.pagingParams = new PagingParams();
                this.articleRegistry.clear();
                this.loadArticles();
            }
        )
    }

    setPagingParams = (pagingParams: PagingParams) => {
        this.pagingParams = pagingParams;
    }

    get axsiosParams() {
        const params = new URLSearchParams();
        params.append('pageNumber', this.pagingParams.pageNumber.toString());
        params.append('pageSize', this.pagingParams.pageSize.toString());
        this.predicate.forEach((value, key) => {
            params.append(key, value)
        })
        return params;
    }

    setPredicate = (predicate: string, value: string) => {
        const resetPredicate = () => {
            this.predicate.forEach((value, key) => {
                if (key !== 'rubricId') this.predicate.delete(key);
            })
        }
        switch (predicate) {
            case 'all':
                resetPredicate();
                this.predicate.set('all', true);
                break;
            case 'isAuthor':
                resetPredicate();
                this.predicate.set('isAuthor', true);
                break;
            case 'rubricId':
                this.predicate.delete('rubricId');
                this.predicate.set('rubricId', value);
        }
    }

    resetPredicates = () => {
        this.predicate = new Map().set('all', true);
    }

    get articlesByDate() {
        return Array.from(this.articleRegistry.values()).sort((a, b) => a.createdDate!.getTime() - b.createdDate!.getTime())
    }

    get articlesByTitleFirstLetter() {
        return Array.from(this.articleRegistry.values()).sort((a, b) => a.title.charCodeAt(0) - b.title.charCodeAt(0));
    }

    get groupedArticles() {
        return Object.entries(
            this.articlesByTitleFirstLetter.reduce((articles, article) => {
                const letter = article.title.charAt(0);
                articles[letter] = articles[letter] ? [...articles[letter], article] : [article];
                return articles;
            }, {} as { [key: string]: Article[] })
        )
    }

    loadArticles = async () => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.Articles.list(this.axsiosParams);
            result.data.forEach(article => {
                this.setArticle(article);
            })
            this.setPagination(result.pagination);
            this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    setPagination = (pagination: Pagination) => {
        this.pagination = pagination;
    }

    loadArticle = async (id: string) => {
        let article = this.getArticle(id);
        if (article) {
            this.selectedArticle = article
            return article;
        }
        else {
            this.setLoadingInitial(true);
            try {
                article = await agent.Articles.details(id);
                this.setArticle(article);
                runInAction(() => this.selectedArticle = article);
                this.setLoadingInitial(false);
                return article;
            } catch (error) {
                console.log(error)
                this.setLoadingInitial(false);
            }
        }
    }

    private setArticle = (article: Article) => {
        const user = store.userStore.user;
        article.isAuthor = article.author?.userName === user?.userName;
        article.createdDate = new Date(article.createdDate!);
        this.articleRegistry.set(article.id, article);
    }

    private getArticle = (id: string) => {
        return this.articleRegistry.get(id);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingIntial = state;
    }

    createArticle = async (article: ArticleFormValues) => {
        const user = store.userStore.user;
        try {
            await agent.Articles.create(article);
            const newArticle = new Article(article);
            this.setArticle(newArticle);
            newArticle.author = user!;
            newArticle.isAuthor = true;
            runInAction(() => {
                this.selectedArticle = newArticle;
            })
        } catch (error) {
            console.log(error);
        }
    }

    updateArticle = async (article: ArticleFormValues) => {
        try {
            await agent.Articles.update(article);
            runInAction(() => {
                if (article.id) {
                    let updatedArticle = { ...this.getArticle(article.id), ...article }
                    this.articleRegistry.set(article.id, updatedArticle as Article);
                    this.selectedArticle = updatedArticle as Article;
                }
            })
        } catch (error) {
            console.log(error);
        }
    }

    deleteArticle = async (id: string) => {
        this.loading = true;
        try {
            await agent.Articles.delete(id);
            runInAction(() => {
                this.articleRegistry.delete(id);
                this.loading = false;
            })
            router.navigate('/articles');
            store.modalStore.closeModal();
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            })
        }
    }

    clearSelectedArticle = () => {
        this.selectedArticle = undefined;
    }
}