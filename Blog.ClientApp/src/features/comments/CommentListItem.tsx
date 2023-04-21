import React, { useState } from 'react'
import { Comment, } from 'semantic-ui-react'
import CommentForm from './CommentForm'
import { AppComment } from '../../app/models/comment';
import { format } from 'date-fns';
import { Link } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../app/stores/store';

interface Props {
    comment: AppComment;
    forceUpdate: () => void;
}

export default observer(function CommentListItem({ comment, forceUpdate }: Props) {

    const [hiden, setHiden] = useState(true);
    const [editMode, setEditMode] = useState(false);
    const [replyMode, setReplyMode] = useState(false);
    const { userStore } = useStore();
    const { commentStore } = useStore();
    const { currentCommentId, setCurrentComment, deleteComment,  } = commentStore;

    return (
        <Comment>
            <Comment.Avatar src={(comment.author.image == null)
                ? '/assets/user.png'
                : comment.author.image
            } />
            <Comment.Content>
                <Comment.Author as={Link} to={`/profiles/${comment.author.userName}`}>{comment.author.displayName}</Comment.Author>
                <Comment.Metadata>
                    <p>{format(Date.parse(comment.createdDate!.toString()), 'dd.MM.yyyy HH:mm')}</p>
                </Comment.Metadata>
                <Comment.Text style={{ whiteSpace: 'pre-wrap' }}>{comment.content}</Comment.Text>
                <Comment.Actions>
                    <Comment.Action>
                        <p onClick={() => {
                            !editMode && setHiden(!hiden)
                            setCurrentComment(comment.id)
                            setReplyMode(!replyMode)
                            setEditMode(false)
                        }}>Відповісти</p>
                    </Comment.Action>
                    {comment.author.userName === userStore.user?.userName &&
                    <>
                        <Comment.Action>
                            <p onClick={() => {
                                !replyMode && setHiden(!hiden)
                                setCurrentComment(comment.id)
                                setReplyMode(false)
                                setEditMode(!editMode);
                            }}>Редагувати</p>
                        </Comment.Action>
                        <Comment.Action>
                            <p onClick={() => {
                                deleteComment(comment.id)
                            }}>Видалити</p>
                        </Comment.Action>
                    </>
                    }
                </Comment.Actions>
            </Comment.Content>
            {currentCommentId === comment.id &&
                <CommentForm
                    hiden={hiden}
                    editMode={editMode}
                    setHiden={setHiden}
                    setEditMode={setEditMode}
                    forceUpdate={forceUpdate}
                    commentableId={comment.id}
                    placeholder='Введіть відповідь (Enter для підтвердження, Enter + Shift для нового рядка)'
                />
            }
            <Comment.Group>
                {comment.comments?.map(comment => (
                    <CommentListItem forceUpdate={forceUpdate} key={comment.id} comment={comment} />
                ))}
            </Comment.Group>
        </Comment>
    )
})