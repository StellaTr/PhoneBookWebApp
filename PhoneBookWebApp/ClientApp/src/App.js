import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import AddEntry from './components/AddEntry';
import EditEntry from './components/EditEntry';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Home} />    
            <Route exact path='/add' component={AddEntry} />
            <Route exact path='/edit' component={EditEntry} /> 
      </Layout>
    );
  }
}
