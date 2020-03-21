import React, { useState } from 'react';
import PhoneBookEntryForm from './PhoneBookEntryForm'
import axios from 'axios';
import { isEmpty, resolveApiURL, isPhoneValid } from './Utils';
import { Alert } from 'reactstrap';

const AddEntry = (props) => {
    const initialEntryState = {
        firstName: '',
        lastName: '',
        countryCode: '',
        areaCode: '',
        phoneNumber: ''
    };

    const [entryState, setEntryState] = useState(initialEntryState);
    const [isSuccessful, setIsSuccessful] = useState(false);
    const [hasError, setHasError] = useState(false);

    const clearEntryValues = () => {
        setEntryState(prevState => {
            return { ...prevState, ...initialEntryState };
        });
    }

    const onValuesChange = (data) => {
        const { name, value } = data;
        setEntryState({ ...entryState, [name]: value });
    }

    const addNewEntry = () => {
        const { firstName, lastName, ...contactPhone } = { ...entryState };

        if (isEmpty(entryState) ||
            !isPhoneValid(contactPhone)
        )
            return;

        axios.post(resolveApiURL("post"), {
            FirstName: firstName,
            LastName: lastName,
            ContactPhones: [contactPhone]
        })
            .then(function (response) {
                setHasError(false);
                setIsSuccessful(true);
            })
            .catch(function (error) {
                setHasError(true);
                setIsSuccessful(false);
            });
    }

    return (
        <div>
            {
                (isSuccessful === true) &&
                <Alert color="success">
                    Save was successful!
                </Alert>
            }
            {
                (hasError === true) &&
                <Alert color="danger">
                    An error occured!
                </Alert>
            }
            <PhoneBookEntryForm entry={entryState} onSubmit={addNewEntry} onClear={clearEntryValues} onChange={onValuesChange} isEmpty={true} />
        </div>
    );
};

export default AddEntry;