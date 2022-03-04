import React, { Component, Link, useState } from 'react';
import Login from './components/login/Login'
import './custom.css'
import 'bootstrap/dist/css/bootstrap.css';
import useToken from './components/app/useToken'
import Logout from './components/logout/logout';

function App() {
    const displayName = App.name;
    const { token, setToken } = useToken();

    if (!token) {
        return <Login setToken={setToken} />
    }

    return (
        <div>
            <Logout setToken={setToken}>Logout</Logout>
        </div>
    );
}

export default App;