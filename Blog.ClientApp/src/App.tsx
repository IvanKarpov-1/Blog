import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';
import { Header, List, ListItem, Button } from 'semantic-ui-react'

function App() {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5000/api/users')
            .then(response => {
                setUsers(response.data);
            });
    }, []);

    return (
    <div>
        <Header as='h2' icon='users' content='Users'/>
            <List>
                {users.map((user: any) => (
                    <ListItem key={user.id}>
                        {user.name}
                    </ListItem>
                ))}
            </List>
    </div>
  );
}

export default App;
