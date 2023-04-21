import React from 'react'
import { Grid, Header, Item, Segment, Statistic } from 'semantic-ui-react'
import { Profile } from '../../app/models/profile'
import { observer } from 'mobx-react-lite'

interface Props {
    profile: Profile
}

export default observer(function ProfilePageHeader({ profile }: Props) {
    return (
        <Segment>
            <Grid>
                <Grid.Column width={10}>
                    <Item.Group>
                        <Item>
                            <Item.Image avatar size='small' src={profile.image || '/assets/user.png'} />
                            <Item.Content verticalAlign='middle'>
                                <Header as='h1' content={profile.displayName} />
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Grid.Column>
                <Grid.Column width={6}>
                    <Statistic.Group widths={2}>
                        <Statistic label='Написаних статей' value='0' />
                        <Statistic label='Написаних коментарів' value='0' />
                    </Statistic.Group>
                </Grid.Column>
            </Grid>
        </Segment>
    )
})