import React, { useState, useEffect } from "react";
import { Button, Header, Segment } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link, useNavigate, useParams } from "react-router-dom";
import { ArticleFormValues } from '../../../app/models/article';
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { v4 as uuid } from 'uuid';
import { Formik, Form } from "formik";
import * as Yup from 'yup';
import MyTextInput from "../../../app/common/form/MyTextInput";
import MyTextArea from "../../../app/common/form/MyTextArea";
import MySelectInput from "../../../app/common/form/MySelectInput";

export default observer(function ArticleForm() {

    const { articleStore } = useStore();
    const { createArticle, updateArticle, loadArticle, loadingIntial } = articleStore;
    const { rubricStore } = useStore();
    const { rubricOptions, loadRubrics } = rubricStore;
    const { id } = useParams();
    const navigate = useNavigate();

    const [article, setArticle] = useState<ArticleFormValues>(new ArticleFormValues());

    const validationSchema = Yup.object({
        title: Yup.string().required("Заголовк статті обов'язковий!"),
        rubricId: Yup.string().required("Рубрика обов'язква"),
        description: Yup.string().required("Опис статті обов'язковий"),
        content: Yup.string().required("Контент статті обов'язковий"),
    })

    useEffect(() => {
        if (id) loadArticle(id).then(article => setArticle(new ArticleFormValues(article)))
    }, [id, loadArticle])

    useEffect(() => {
        if (rubricOptions.length <= 0) loadRubrics();
    }, [rubricOptions.length, loadRubrics])

    function handleFormSubmit(article: ArticleFormValues) {
        if (!article.id) {
            article.id = uuid();
            createArticle(article).then(() => navigate(`/articles/${article.id}`));
        } else {
            updateArticle(article).then(() => navigate(`/articles/${article.id}`));
        }
    }

    if (loadingIntial) return <LoadingComponent content='Завантаження статті...' />

    return (
        <Segment clearing>
            <Header content='Деталі статті' sub color="grey" />
            <Formik
                validationSchema={validationSchema}
                enableReinitialize
                initialValues={article}
                onSubmit={values => handleFormSubmit(values)}>
                {({ handleSubmit, isValid, isSubmitting, dirty }) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name='title' placeholder='Назва' />
                        <MySelectInput name='rubricId' placeholder='Рубрика' options={rubricOptions} />
                        <MyTextArea rows={3} name='description' placeholder='Опис' />
                        <MyTextArea rows={5} name='content' placeholder='Зміст' />
                        <Button
                            disabled={isSubmitting || !dirty || !isValid}
                            loading={isSubmitting}
                            floated='right'
                            positive
                            type='submit'
                            content='Надіслати' />
                        <Button as={Link} to='/articles' floated='right' type='button' content='Скасувати' />
                    </Form>
                )}
            </Formik>
        </Segment>
    )
})