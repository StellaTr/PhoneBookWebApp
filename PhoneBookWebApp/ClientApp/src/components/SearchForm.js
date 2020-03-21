import React, { Component } from 'react';
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';
import axios from 'axios';
import { resolveApiURL } from './Utils';

export class SearchForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            value: ''
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        this.setState({ value: event.target.value.trim() });
    }

    handleSubmit = async (event) => {
        if (this.state.value === '') return;

        const response = await axios.get(resolveApiURL("getWithQuery", this.state.value));
        this.props.onSubmit(response.data);

        this.setState({ value: '' });
    }

    render() {
        return (
            <Form inline>
                <FormGroup className="mb-4 mr-md-4 mb-md-0">
                    <Input type="text"
                        name="searchQuery"
                        id="searchQuery"
                        onChange={this.handleChange}
                        value={this.state.value}
                        placeholder="Search by Name or Phone"
                        style={{ width: 250 }} />
                </FormGroup>
                <Button onClick={this.handleSubmit}>Go!</Button>
            </Form>
        );
    }
}  