import React, { useEffect, useState } from "react";
import { Grid, GridColumn, Loader } from "semantic-ui-react";
import ArticleList from "./ArticleList";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import ArticleFilter from "./ArticleFilter";
import { PagingParams } from "../../../app/models/pagination";
import InfiniteScroll from "react-infinite-scroller";
import ArticleListItemPlaceholder from "./ArticleListItemPlaceholder";

export default observer(function ArticleDashboard() {

    const { articleStore } = useStore();
    const { loadArticles, articleRegistry, loadingIntial, setPagingParams, pagination } = articleStore;
    const [loadingNext, setLoadingNext] = useState(false);

    function handleGetNext() {
        setLoadingNext(true);
        setPagingParams(new PagingParams(pagination!.currentPage + 1));
        loadArticles().then(() => setLoadingNext(false));
    }

    useEffect(() => {
        if (articleRegistry.size <= 1) loadArticles();
    }, [loadArticles, articleRegistry.size])

    return (
        <Grid>
            <Grid.Column width='10'>
                {loadingIntial && !loadingNext ? (
                    <>
                        <ArticleListItemPlaceholder />
                        <ArticleListItemPlaceholder />
                        <ArticleListItemPlaceholder />
                    </>
                ) : (
                    <InfiniteScroll
                        pageStart={0}
                        loadMore={handleGetNext}
                        hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
                        initialLoad={false}
                    >
                        <ArticleList />
                    </InfiniteScroll>
                )}

            </Grid.Column>
            <Grid.Column width='6'>
                <ArticleFilter></ArticleFilter>
            </Grid.Column>
            <GridColumn width={10}>
                <Loader active={loadingNext} />
            </GridColumn>
        </Grid>
    )
})