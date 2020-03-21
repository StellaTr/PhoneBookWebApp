import React, { Component } from 'react';
import { SearchForm } from './SearchForm'
import { Container, Row, Col } from 'reactstrap';


export class Home extends Component {
    state = {
        entries: ''
    }

    displayMatchingEntries = (entriesData) => {
        const processedData = this.flattenData(entriesData);
        this.setState({ entries: processedData });
        console.log(this.state.entries);
    }

    flattenData = (data) => {
        const flattenData = data.map((contact, index) => {
            return (contact.contactPhones.map((phone) => {
                return { ...phone, 'firstName': data[index].firstName, 'lastName': data[index].lastName }
            }))
        });

        return Array.prototype.concat.apply([], flattenData);
    }

    render() {
        return (
            <Container>
                <Row>
                    <Col lg>
                        <SearchForm onSubmit={this.displayMatchingEntries} />
                    </Col>                    
                </Row>
                <br />
                <br />
                <Row>                    
                </Row>
            </Container>
        );
    }
}
