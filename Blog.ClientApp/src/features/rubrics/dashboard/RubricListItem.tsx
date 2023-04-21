import React from 'react'
import { Rubric } from '../../../app/models/rubric'
import { Button, Grid, Icon } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { useStore } from '../../../app/stores/store';

interface Props {
    rubric: Rubric;
}

export default function RubricListItem({ rubric }: Props) {

    const { articleStore: { setPredicate } } = useStore()

    return (
        <Grid>
            <Grid.Column width={1}>
                <Icon
                    size='large'
                    name={rubric.rubrics?.length === 0 ? 'folder' : 'folder open'}
                    style={{ marginTop: '12px' }}
                />
            </Grid.Column>
            <Grid.Column width={15}>
                <Button
                    className='rubricListButton'
                    size='big'
                    as={Link}
                    to={'/articles'}
                    content={rubric.name}
                    onClick={() => setPredicate('rubricId', rubric.id)}
                />
                {rubric.rubrics?.map(rubric => (

                    <RubricListItem rubric={rubric} key={rubric.id} />
                ))}
            </Grid.Column>
        </Grid>
    )
}