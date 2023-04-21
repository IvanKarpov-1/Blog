import React from 'react'
import { observer } from 'mobx-react-lite';
import { Segment, Grid, Icon } from 'semantic-ui-react'
import { Article } from "../../../app/models/article";

interface Props {
    article: Article
}

export default observer(function ArticleDetailedInfo({ article }: Props) {
    return (
        <Segment.Group>
            <Segment attached='top'>
                <Grid>
                    <Grid.Column width={1}>
                        <Icon size='large' color='grey' name='info' />
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <p>{article.description}</p>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='marker' size='large' color='grey' />
                    </Grid.Column>
                    <Grid.Column width={11}>
                        <span>{article.content}</span>
                    </Grid.Column>
                </Grid>
            </Segment>
        </Segment.Group>
    )
})