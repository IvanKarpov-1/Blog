import React from 'react'
import { observer } from 'mobx-react-lite';
import { Image, Header, Item, Segment, Button, Popup } from 'semantic-ui-react'
import { Article } from "../../../app/models/article";
import { Link } from 'react-router-dom';
import { format } from 'date-fns';
import { useStore } from '../../../app/stores/store';
import DeleteForm from '../form/DeleteForm';
import ProfileCard from '../../profiles/ProfileCard';

const articleImageStyle = {
    filter: 'brightness(30%)',
    objectFit: 'cover',
    width: '100%',
    marginTop: '-15%',
};

const articleImageTextStyle = {
    position: 'absolute',
    bottom: '1%',
    left: '5%',
    width: '100%',
    height: 'auto',
    color: 'white'
};

const articleButtonsStyle = {
    position: 'absolute',
    bottom: '1%',
    right: '5%',
    width: '60%',
    height: 'auto',
    color: 'white',
    marginBottom: '14px'
};

interface Props {
    article: Article
}

export default observer(function ArticleDetailedHeader({ article }: Props) {

    const { articleStore: { loading } } = useStore();
    const { modalStore } = useStore()

    return (
        <Segment basic attached='top' style={{ padding: '0', height: '350px', overflow: 'hidden' }}>
            <Image src={`/assets/logo/logo_grey.svg`} fluid style={articleImageStyle} />
            <Segment style={articleImageTextStyle} basic>
                <Item.Group>
                    <Item>
                        <Item.Content>
                            <Header
                                size='huge'
                                content={article.title}
                                style={{ color: 'white' }}
                            />
                            <p>{format(article.createdDate!, 'dd MM yyyy HH:mm')}</p>
                            <p>
                                Автор: <strong>
                                    <Popup
                                        hoverable
                                        trigger={
                                            <Link to={`/profiles/${article.author?.userName}`}>
                                                {article.author?.displayName}
                                            </Link>
                                        }
                                    >
                                        <Popup.Content>
                                            <ProfileCard profile={article.author!} />
                                        </Popup.Content>
                                    </Popup>
                                </strong>
                            </p>
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment style={articleButtonsStyle} basic>
                {article.isAuthor && (
                    <>
                        <Button
                            onClick={() => modalStore.openModal(<DeleteForm id={article.id} />)}
                            floated='right'
                            color='grey'
                            loading={loading}
                        >
                            Видалити статтю
                        </Button>
                        <Button as={Link} to={`/manage/${article.id}`} floated='right' color='grey' inverted>
                            Керувати статтею
                        </Button>
                    </>
                )}
            </Segment>
        </Segment>
    )
})