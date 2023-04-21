import React, { useEffect } from 'react';
import { Grid } from "semantic-ui-react";
import ProfilePageHeader from "./ProfilePageHeader";
import ProfilePageContent from "./ProfilePageContent";
import { observer } from "mobx-react-lite";
import { useParams } from "react-router-dom";
import { useStore } from "../../app/stores/store";
import LoadingComponent from "../../app/layout/LoadingComponent";

export default observer(function ProfilePage() {

    const { userName } = useParams();
    const { profileStore } = useStore();
    const { loadingProfile, loadProfile, profile } = profileStore;

    useEffect(() => {
        loadProfile(userName!);
    }, [loadProfile, userName])

    if (loadingProfile) return <LoadingComponent content='Завантаження профілю...' />

    return (
        <Grid>
            <Grid.Column width={16}>
                {profile &&
                    <ProfilePageHeader profile={profile} />
                }
                <ProfilePageContent />
            </Grid.Column>
        </Grid>
    )
})