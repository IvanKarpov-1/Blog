import { Navigate, RouteObject, createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import ArticleDashboard from "../../features/articles/dashboard/ArticleDashboard";
import ArticleForm from "../../features/articles/form/ArticleForm";
import ArticleDetails from "../../features/articles/details/ArticleDetails";
import TestErrors from "../../features/errors/TestError";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";
import LoginForm from "../../features/users/LoginForm";
import ProfilePage from "../../features/profiles/ProfilePage";
import RubricDashboard from "../../features/rubrics/dashboard/RubricDashboard";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            { path: 'rubrics', element: <RubricDashboard /> },
            { path: 'articles', element: <ArticleDashboard /> },
            { path: 'articles/:id', element: <ArticleDetails /> },
            { path: 'createArticle', element: <ArticleForm key='create' /> },
            { path: 'profiles/:userName', element: <ProfilePage /> },
            { path: 'manage/:id', element: <ArticleForm key='manage' /> },
            { path: 'login', element: <LoginForm /> },
            { path: 'errors', element: <TestErrors /> },
            { path: 'not-found', element: <NotFound /> },
            { path: 'server-error', element: <ServerError /> },
            { path: '*', element: <Navigate replace to='/not-found' /> },
        ]
    }
]

export const router = createBrowserRouter(routes)