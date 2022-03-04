import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import Login from './components/login/Login'
import './custom.css'
import 'bootstrap/dist/css/bootstrap.css';
import { Redirect } from 'react-router-dom';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (

            //<Layout>
            //    {/*<Route exact path='/' component={Home} />*/}
            //    {/*<Route path='/counter' component={Counter} />*/}
            //    {/*<Route path='/fetch-data' component={FetchData} />*/}
            //    <Route path='/login' component={Login} />
            //</Layout>
            <Login />
        );
    }
}