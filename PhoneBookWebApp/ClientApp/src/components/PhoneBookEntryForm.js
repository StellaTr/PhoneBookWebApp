import React from 'react';
import { Col, Row, Button, FormGroup, Label } from 'reactstrap';
import { AvForm, AvField } from 'availity-reactstrap-validation';

const PhoneBookEntryForm = (props) => {
    const entryState = props.entry;

    const handleChange = (event) => {
        props.onChange(event.target);
    }

    const returnEmptyValueMessage = (value) => (
        !(value.trim() === '') ? true : "The field cannot be empty"
    );

    const handleSave = (event) => {
        event.preventDefault();
        props.onSubmit();
    }

    const handleClear = (event) => {
        props.onClear();
    }

    return (
        <AvForm>
            <Row form>
                <Col md={4}>
                    <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                        <Label for="firstName">First Name</Label>
                        <AvField
                            type="text"
                            name="firstName"
                            id="firstName"
                            value={entryState.firstName}
                            onChange={handleChange}
                            validate={{ returnEmptyValueMessage }} required />
                    </FormGroup>
                </Col>
            </Row>
            <br />
            <br />
            <Row form>
                <Col md={4}>
                    <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                        <Label for="lastName">Last Name</Label>
                        <AvField
                            type="text"
                            name="lastName"
                            id="lastName"
                            value={entryState.lastName}
                            onChange={handleChange}
                            validate={{ returnEmptyValueMessage }} required />
                    </FormGroup>
                </Col>
            </Row>
            <br />
            <br />
            <Row form>
                <Col md={2}>
                    <FormGroup>
                        <Label for="countryCode">Country Code</Label>
                        <AvField
                            type="text"
                            name="countryCode"
                            id="countryCode"
                            value={entryState.countryCode}
                            onChange={handleChange}
                            pattern="^[0-9]+$" validate={{ returnEmptyValueMessage }} required />
                    </FormGroup>
                </Col>
                <Col md={3}>
                    <FormGroup>
                        <Label for="areaCode">Area Code</Label>
                        <AvField
                            type="text"
                            name="areaCode"
                            id="areaCode"
                            value={entryState.areaCode}
                            onChange={handleChange}
                            pattern="^[0-9]+$" validate={{ returnEmptyValueMessage }} required />
                    </FormGroup>
                </Col>
                <Col md={3}>
                    <FormGroup>
                        <Label for="phoneNumber">Phone Number (at least 6 digits)</Label>
                        <AvField
                            type="text"
                            name="phoneNumber"
                            id="phoneNumber"
                            value={entryState.phoneNumber}
                            onChange={handleChange}
                            pattern="^[0-9]{6,}$" validate={{ returnEmptyValueMessage }} required />
                    </FormGroup>
                </Col>
            </Row>
            <Button onClick={handleSave}>Save</Button>{' '}
            <Button onClick={handleClear}>Clear</Button>
        </AvForm>
    );
};

export default PhoneBookEntryForm;