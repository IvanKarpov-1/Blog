import React, { useEffect, useState } from 'react'
import { useStore } from '../../app/stores/store';
import { Formik, Form, Field, FieldProps } from 'formik';
import * as Yup from 'yup'
import { Button, Loader } from 'semantic-ui-react';
import { AppCommentFormValues } from '../../app/models/comment';

interface Props {
    commentableId: string;
    placeholder: string;
    hiden?: boolean;
    editMode?: boolean;
    setHiden: (hiden: boolean) => void;
    setEditMode?: (editMode: boolean) => void;
    forceUpdate: () => void;
}

export default function CommentForm({ commentableId, placeholder, hiden, editMode, setHiden, setEditMode, forceUpdate }: Props) {

    const { commentStore } = useStore();
    const { loadComment, addComment, updateComment } = commentStore;
    const [comment, setComment] = useState<AppCommentFormValues>(new AppCommentFormValues());

    useEffect(() => {
        if (editMode) loadComment(commentableId).then(comment => setComment(new AppCommentFormValues(comment)))
        else setComment(new AppCommentFormValues())
    }, [loadComment, commentableId, editMode]);

    function handleFormSubmit(comment: AppCommentFormValues) {
        if (!comment.id) {
            comment.parentId = commentableId
            addComment(comment).then(() => {
                setHiden(true)
                // forceUpdate()
            })
        } else {
            updateComment(comment).then(() => {
                setHiden(true)
                // forceUpdate()
            })
        }
    }

    return (
        <Formik
        enableReinitialize
        initialValues={comment}
        validationSchema={Yup.object({
            content: Yup.string().required()
        })}
        onSubmit={values => handleFormSubmit(values)}
        >
            {({ isSubmitting, isValid, handleSubmit }) => (
                <Form className='ui form clearing' hidden={hiden}>
                    <Field name='content'>
                        {(props: FieldProps) => (
                            <div style={{ position: 'relative' }}>
                                <Loader active={isSubmitting} />
                                <textarea
                                    style={{ marginBottom: '10px' }}
                                    placeholder={placeholder}
                                    rows={2}
                                    {...props.field}
                                    onKeyDown={e => {
                                        if (e.key === 'Enter' && e.shiftKey) {
                                            return;
                                        }
                                        if (e.key === 'Enter' && !e.shiftKey) {
                                            e.preventDefault();
                                            isValid && handleSubmit();
                                        }
                                    }}
                                />
                                <Button
                                    loading={isSubmitting}
                                    disabled={isSubmitting || !isValid}
                                    content='Додати коментар'
                                    labelPosition='left'
                                    icon='edit'
                                    type='submit'
                                    floated='right'
                                />
                            </div>
                        )}
                    </Field>
                </Form>
            )}
        </Formik>
    )
}