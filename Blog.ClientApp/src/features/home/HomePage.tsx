import { Container, Header, Segment, Image, Button, Grid } from "semantic-ui-react";
import { Link } from 'react-router-dom';
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import LoginForm from "../users/LoginForm";
import RegisterForm from "../users/RegisterForm";

export default observer(function HomePage() {

    const { userStore, modalStore } = useStore();

    return (
        <Segment inverted textAlign='center' vertical className='masthead'>
            <Container text>
                <Header as='h1' inverted>
                    <Image src='/assets/logo/favicon_white_transparent.svg' alt='logo' style={{ marginBottom: 9 }} />
                </Header>
                {userStore.isLoggedIn ? (
                    <>
                        <Header as='h2' inverted content='Ласкаво просимо до Some Blog' />
                        <Button as={Link} to='/articles' size='huge' inverted>
                            Перейти до статей!
                        </Button>
                    </>
                ) : (
                    <Grid>
                        <Grid.Row>
                            <Grid.Column width={3} />
                            <Grid.Column width={10}>
                                <Button as={Link} to='/articles' size='huge' inverted fluid>
                                    Увійти як гість
                                </Button>
                            </Grid.Column>
                            <Grid.Column width={3} />
                        </Grid.Row>
                        <Grid.Row>
                            <Grid.Column width={3} />
                            <Grid.Column width={4}>
                                <Button onClick={() => modalStore.openModal(<LoginForm />)} size='huge' inverted fluid>
                                    Увійти!
                                </Button>
                            </Grid.Column>
                            <Grid.Column width={6}>
                                <Button onClick={() => modalStore.openModal(<RegisterForm />)} size='huge' inverted fluid>
                                    Зареєструватися!
                                </Button>
                            </Grid.Column>
                            <Grid.Column width={2} />
                        </Grid.Row>
                    </Grid>
                )}
            </Container>
        </Segment>
    )
})