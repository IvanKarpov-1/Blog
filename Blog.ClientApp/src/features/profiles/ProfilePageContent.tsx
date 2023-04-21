import React from 'react'
import { Tab } from 'semantic-ui-react'
import ProfilePane from './panes/ProfileAbout';

export default function ProfilePageContent() {
    const panes = [
        { menuItem: 'Про', render: () => <ProfilePane /> },
        { menuItem: 'Недавні статті', render: () => <Tab.Pane>Недавні статті</Tab.Pane> },
        { menuItem: 'Всі статті', render: () => <Tab.Pane>Всі статті</Tab.Pane> },
        { menuItem: 'Коментарі', render: () => <Tab.Pane>Коментарі</Tab.Pane> },
    ];

    return (
        <Tab
            menu={{ fluid: true, vertical: true }}
            menuPosition='right'
            panes={panes}
        />
    )
}