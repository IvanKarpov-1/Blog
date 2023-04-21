import React from 'react'
import { observer } from "mobx-react-lite";
import { ErrorMessage, Formik } from 'formik';
import MyTextArea from '../../../app/common/form/MyTextArea';
import MyTextInput from '../../../app/common/form/MyTextInput';
import { useStore } from '../../../app/stores/store';
import * as Yup from 'yup';
import { Button, Form } from 'semantic-ui-react';
import ValidationError from '../../errors/ValidationError';

interface Props {
    setEditMode: (editMode: boolean) => void;
}

export default observer(function ProfileEditForm({ setEditMode }: Props) {

    const { profileStore: { profile, updateProfile } } = useStore();

    const validationSchema = Yup.object({
        displayName: Yup.string().required("Відображувальне ім'я статті обов'язкове!")
    })

    return (
        <Formik
            initialValues={{ displayName: profile?.displayName, bio: profile?.bio || '', error: null }}
            validationSchema={validationSchema}
            onSubmit={(values, { setErrors }) => {
                updateProfile(values)
                .catch(error => setErrors({ error }))
                .then(() => setEditMode(false))
            }}
        >
            {({ handleSubmit, isValid, errors, isSubmitting, dirty }) => (
                <Form className='ui form' onSubmit={handleSubmit}>
                    <MyTextInput placeholder="Відображуване ім'я" name='displayName' />
                    <MyTextArea rows={3} placeholder='Біо' name='bio' />
                    <ErrorMessage
                        name='error' render={() =>
                            <ValidationError errors={errors.error} />}
                    />
                    <Button
                        disabled={isSubmitting || !dirty || !isValid}
                        loading={isSubmitting}
                        floated='right'
                        positive
                        content='Підтвердити'
                        type='submit'
                    />
                </Form>
            )}
        </Formik>
    )
})