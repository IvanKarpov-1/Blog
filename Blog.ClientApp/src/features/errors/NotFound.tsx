import { Button, Container, Header, Icon, Segment } from "semantic-ui-react";
import { Link } from 'react-router-dom';

export default function NotFound() {
    return (
        <Segment placeholder>
            <Header icon>
                <Icon name='search' />
                <Container text content="На жаль, ми шукали всюди, але не змогли знайти те, що ви шукаєте =(" />
            </Header>
            <Segment.Inline>
                <Button as={Link} to='/articles'>
                    Повернутися до сторінки статей
                </Button>
            </Segment.Inline>
        </Segment>
    )
}