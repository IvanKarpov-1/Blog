import { observer } from "mobx-react-lite";
import React from "react";
import { Header, Menu } from 'semantic-ui-react';
import { useStore } from "../../../app/stores/store";

export default observer(function ArticleFilter() {

    const { articleStore: { predicate, setPredicate, resetPredicates } } = useStore()
    const { userStore: { isLoggedIn } } = useStore();

    return (
        <>
            <Menu vertical size='large' style={{ width: '100$', marginTop: 27 }}>
                <Header icon='filter' attached color='grey' content='Фільтрувати' />
                <Menu.Item
                    content='Всі статті'
                    active={predicate.has('all')}
                    onClick={() => setPredicate('all', 'true')}
                />
                {isLoggedIn &&
                    <Menu.Item
                        content='Мої статті'
                        active={predicate.has('isAuthor')}
                        onClick={() => setPredicate('isAuthor', 'true')}
                    />
                }
                <Menu.Item
                    content='Скинути фільтри'
                    onClick={resetPredicates}
                />
            </Menu>
            <Header />
        </>
    )
})