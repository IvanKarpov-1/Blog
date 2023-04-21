import React, { useEffect } from 'react'
import { Grid } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { observer } from 'mobx-react-lite';
import { useParams } from 'react-router-dom';
import ArticleDetailedHeader from './ArticleDetailedHeader';
import ArticleDetailedInfo from './ArticleDetailedInfo';
import ArticleDetailedCommentSection from './ArticleDetailedCommentSection';
import ArticleDetailedSidebar from './ArticleDetailedSidebar';

export default observer(function ArticleDetails() {

    const { articleStore } = useStore();
    const { selectedArticle: article, loadArticle, loadingIntial, clearSelectedArticle } = articleStore;
    const { id } = useParams();

    useEffect(() => {
        if (id) loadArticle(id);
        return () => clearSelectedArticle();
    }, [id, loadArticle, clearSelectedArticle])

    if (loadingIntial || !article) return <LoadingComponent />;

    return (
        <Grid>
            <Grid.Column width={10}>
                <ArticleDetailedHeader article={article} />
                <ArticleDetailedInfo article={article} />
                <ArticleDetailedCommentSection articleId={article.id} />
            </Grid.Column>
            <Grid.Column width={6}>
                <ArticleDetailedSidebar />
            </Grid.Column>
        </Grid>
    )
})