import { createContext, useContext } from "react";
import ArticleStore from "./articleStore";
import CommonStore from './commonStore';
import UserStore from "./userStore";
import ModalStore from "./modalStore";
import ProfileStore from "./profileStore";
import CommentStore from './commentStore';
import RubricStore from "./rubricStore";

interface Store {
    articleStore: ArticleStore
    commonStore: CommonStore
    userStore: UserStore
    modalStore: ModalStore
    profileStore: ProfileStore
    commentStore: CommentStore
    rubricStore: RubricStore
}

export const store: Store = {
    articleStore: new ArticleStore(),
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    profileStore: new ProfileStore(),
    commentStore: new CommentStore(),
    rubricStore: new RubricStore()
}

export const StoreContex = createContext(store);

export function useStore() {
    return useContext(StoreContex);
}