import React, { useCallback, useEffect, useState } from 'react'
import { observer } from 'mobx-react-lite'
import { Segment, Header, Comment } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store';
import CommentForm from '../../comments/CommentForm';
import CommentListItem from '../../comments/CommentListItem';

interface Props {
    articleId: string;
}


export default observer(function ArticleDetailedCommentSection({ articleId }: Props) {

    const { commentStore } = useStore();
    const [state, updateState] = useState();
    const forceUpdate = useCallback(() => {
        updateState(state)
    }, [state])

    useEffect(() => {
        if (articleId) {
            commentStore.createHubConnection(articleId);
        }
        return () => {
            commentStore.clearComments();
        }
    }, [commentStore, articleId])

    return (
        <>
            <Segment
                textAlign='center'
                attached='top'
                inverted
                color='grey'
                style={{ border: 'none' }}
            >
                <Header>Коментарі</Header>
            </Segment>
            <Segment attached clearing>
                <CommentForm
                    setHiden={() => { }}
                    forceUpdate={forceUpdate}
                    commentableId={articleId}
                    placeholder='Введіть коментар (Enter для підтвердження, Enter + Shift для нового рядка)'
                />
                <Comment.Group>
                    {commentStore.comments.map(comment => (
                        <CommentListItem forceUpdate={forceUpdate} key={comment.id} comment={comment} />
                    ))}
                </Comment.Group>
            </Segment>
        </>
    )
})