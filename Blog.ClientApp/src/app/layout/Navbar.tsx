import { Menu, Container, Button, Image, Dropdown } from 'semantic-ui-react';
import { Link, NavLink } from 'react-router-dom';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';
import LoginForm from '../../features/users/LoginForm';
import RegisterForm from '../../features/users/RegisterForm';

export default observer(function NavBar() {

    const { userStore: { user, logout, isLoggedIn } } = useStore()
    const { modalStore } = useStore();
    const { articleStore: { resetPredicates } } = useStore()

    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item as={NavLink} to='/' header>
                    <img src="/assets/logo/favicon_white_transparent.svg" alt="logo" style={{ marginRight: '10px', width: '3em' }} />
                </Menu.Item>
                <Menu.Item as={NavLink} to='/rubrics' name="Рубрики" />
                <Menu.Item as={NavLink} to='/articles' onClick={() => resetPredicates()} name="Статті" />
                <Menu.Item as={NavLink} to='/errors' name="Помилки" />
                {isLoggedIn &&
                    <Menu.Item>
                        <Button as={NavLink} to='/createArticle' content='Написати статтю' />
                    </Menu.Item>
                }
                <Menu.Item position='right'>
                    <Image src={user?.image || '/assets/user.png'} avatar spaced='right' />
                    <Dropdown pointing='top left' text={user?.displayName}>
                        {isLoggedIn ? (
                            <Dropdown.Menu>
                                <Dropdown.Item as={Link} to={`/profiles/${user?.userName}`} text='Мій профіль' icon='user' />
                                <Dropdown.Item onClick={logout} text='Вийти' icon='power' />
                            </Dropdown.Menu>
                        ) : (
                            <Dropdown.Menu>
                                <Dropdown.Item onClick={() => modalStore.openModal(<LoginForm />)} text='Увійти' icon='id card' />
                                <Dropdown.Item onClick={() => modalStore.openModal(<RegisterForm />)} text='Зареєструватися' icon='sign in alternate' />
                            </Dropdown.Menu>
                        )}
                    </Dropdown>
                </Menu.Item>
            </Container>
        </Menu>
    );
})