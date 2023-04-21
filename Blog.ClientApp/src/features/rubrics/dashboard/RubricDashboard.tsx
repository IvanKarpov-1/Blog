import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../app/stores/store";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { Container, Grid } from "semantic-ui-react";
import RubricListItem from "./RubricListItem";

export default observer(function RubricDashboard() {

    const { rubricStore } = useStore();
    const { loadRubrics, rubrics, loadingIntial } = rubricStore;

    useEffect(() => {
        if (rubrics.length <= 0) loadRubrics();
    }, [loadRubrics, rubrics.length])

    if (loadingIntial) return <LoadingComponent content='Завантаження рубрик...' />

    return (
        <Grid>
            <Grid.Column width='3'>
            </Grid.Column>
            <Grid.Column width='10'>
                <Container>
                    {rubrics.map(rubric => (
                        <RubricListItem rubric={rubric} key={rubric.id} />
                    ))}
                </Container>
            </Grid.Column>
            <Grid.Column width='3'>
            </Grid.Column>
        </Grid>
    )
})