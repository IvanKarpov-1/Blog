import axios, { AxiosError, AxiosResponse } from 'axios';
import { Article, ArticleFormValues } from '../models/article';
import { toast } from 'react-toastify';
import { router } from '../router/Routes';
import { store } from '../stores/store';
import { User, UserFormValues } from '../models/user';
import { Profile } from '../models/profile';
import { AppComment } from '../models/comment';
import { Rubric } from '../models/rubric';
import { PaginatedResult } from '../models/pagination';

const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay)
    })
}

axios.defaults.baseURL = 'http://localhost:5000/api';

axios.interceptors.request.use(config => {
    const token = store.commonStore.token;
    if (token && config.headers) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response => {
    await sleep(1000);
    const pagination = response.headers['pagination'];
    if (pagination) {
        response.data = new PaginatedResult(response.data, JSON.parse(pagination));
        return response as AxiosResponse<PaginatedResult<any>>;
    }
    return response;
}, (error: AxiosError) => {
    const { data, status, config } = error.response as AxiosResponse;
    switch (status) {
        case 400:
            if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
                router.navigate('/not-found');
            }
            if (data.errors) {
                const modalStateErrors = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        modalStateErrors.push(data.errors[key]);
                    }
                }
                throw modalStateErrors.flat();
            } else {
                toast.error(data);
            }
            break;
        case 401:
            toast.error('несанкціоновано');
            break;
        case 403:
            toast.error('заборонено');
            break;
        case 404:
            router.navigate('/not-found')
            break;
        case 500:
            store.commonStore.setServerError(data);
            router.navigate('/server-error')
            break;
    }
    return Promise.reject(error);
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Articles = {
    list: (params: URLSearchParams) => axios.get<PaginatedResult<Article[]>>('/articles', { params })
        .then(responseBody),
    details: (id: string) => requests.get<Article>(`/articles/${id}`),
    create: (user: ArticleFormValues) => requests.post<void>('/articles', user),
    update: (user: ArticleFormValues) => requests.put<void>(`/articles/${user.id}`, user),
    delete: (id: string) => requests.del<void>(`/articles/${id}`)
}

const Account = {
    current: () => requests.get<User>('/account'),
    login: (user: UserFormValues) => requests.post<User>('/account/login', user),
    register: (user: UserFormValues) => requests.post<User>('/account/register', user)
}

const Profiles = {
    get: (userName: string) => requests.get<Profile>(`/profiles/${userName}`),
    update: (profile: Partial<Profile>) => requests.put('/profiles', profile)
}

const Comments = {
    list: (commentableId: string) => requests.get<AppComment[]>(`/comments/${commentableId}`)
}

const Rubrics = {
    list: (parentId: string | null) => requests.get<Rubric[]>(`/rubrics/${parentId ?? ''}`)
}

const agent = {
    Articles,
    Account,
    Profiles,
    Comments,
    Rubrics
}

export default agent;