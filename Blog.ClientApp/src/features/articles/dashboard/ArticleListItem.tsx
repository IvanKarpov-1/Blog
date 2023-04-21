import React from "react";
import { Link } from "react-router-dom";
import { Button, Header, Icon, Item, Label, Segment } from "semantic-ui-react";
import { Article } from "../../../app/models/article";
import { format } from "date-fns";

interface Props {
    article: Article
}

export default function ArticleListItem({ article }: Props) {
    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Image style={{ marginBottom: 3 }} size='tiny' circular src={article.author?.image || '/assets/user.png'} />
                        <Item.Content>
                            <Item.Header as={Link} to={`/articles/${article.id}`}>
                                {article.title}
                            </Item.Header>
                            {article.isAuthor ? (
                                <Item.Description>
                                    <Label basic>
                                        Ви автор цієї статті
                                    </Label>
                                </Item.Description>
                            ) : (
                                <Item.Description>Автор: <Link to={`/profiles/${article.author?.userName}`}>{article.author?.displayName}</Link></Item.Description>
                            )}
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment>
                <span>
                    <Icon name='calendar' /> {format(article.createdDate!, 'dd MM yyyy HH:mm')}
                </span>
                <Header content={article.rubricName ?? ''} sub style={{ marginTop: '5px' }} />
            </Segment>
            <Segment clearing>
                <span>{article.description}</span>
                <Button
                    as={Link}
                    to={`/articles/${article.id}`}
                    color='grey'
                    floated='right'
                    content='Читати'
                />
            </Segment>
        </Segment.Group>
    )
}