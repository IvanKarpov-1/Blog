import { ErrorMessage, Formik } from "formik";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../app/stores/store";
import { Button, Container, Header, Label } from "semantic-ui-react";
import { Form } from "react-router-dom";

export interface Props {
    id: string;
}

export default observer(function DeleteForm({ id }: Props) {

    const { articleStore: { deleteArticle } } = useStore();

    return (
        <Formik
            initialValues={{ errors: null }}
            onSubmit={(values, { setErrors }) => deleteArticle(id).catch(error =>
                setErrors(values.errors = error))}>
            {({ handleSubmit, isSubmitting, errors }) => (
                <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' color='violet' textAlign='center'>
                        Ви дійсно бажаєте видалити статтю?
                    </Header>
                    <ErrorMessage
                        name='error' render={() =>
                            <Label style={{ marginBottom: 10 }} basic color='red' content={errors.errors} />}
                    />
                    <Container>
                        <Button.Group widths='3'>
                            <Button loading={isSubmitting} content='Ні' type="reset" />
                            <Button.Or text='або' className='modalOr' />
                            <Button loading={isSubmitting} content='Tak' type="submit" />
                        </Button.Group>
                    </Container>
                </Form>
            )}
        </Formik>
    )
})