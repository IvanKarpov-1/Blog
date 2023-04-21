import { observer } from 'mobx-react-lite'
import React, { useState } from 'react'
import { Button, Grid, Header, Tab } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store'
import ProfileEditForm from '../form/ProfileEditForm'

export default observer(function ProfileAbout() {

    const { profileStore } = useStore()
    const { isCurrentUser, profile } = profileStore;
    const [editMode, setEditMode] = useState(false);

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header
                        floated='left'
                        icon='user'
                        content={`Про ${profile?.displayName}`}
                    />
                    {isCurrentUser && (
                        <Button
                            basic
                            floated='right'
                            onClick={() => setEditMode(!editMode)}
                            content={editMode ? 'Відмінити' : 'Редагувати'}
                        />
                    )}
                </Grid.Column>
                <Grid.Column width={16}>
                    {editMode ? (
                        <ProfileEditForm setEditMode={setEditMode} />
                    ) : (
                        <span style={{ whiteSpace: 'pre-wrap' }}>{profile?.bio}</span>
                    )}
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    )
})