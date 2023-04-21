import React, { Fragment } from 'react';
import { Header } from 'semantic-ui-react';
import { useStore } from '../../../app/stores/store';
import { observer } from 'mobx-react-lite';
import ArticleListItem from './ArticleListItem';

export default observer(function ArticleList() {

    const { articleStore } = useStore();
    const { groupedArticles } = articleStore;

    return (
        <>
            {groupedArticles.map(([group, articles]) => (
                <Fragment key={group}>
                    <Header sub color='grey'>
                        {group}
                    </Header>
                    <>
                        {articles.map(article => (
                            <ArticleListItem key={article.id} article={article} />
                        ))}
                    </>
                </Fragment>
            ))}
        </>
    )
})