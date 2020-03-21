import React, { useState, useEffect } from 'react';
import PhoneBookEntryForm from './PhoneBookEntryForm'
import axios from 'axios';
import { isEmpty, resolveApiURL, isPhoneValid } from './Utils';
import { Alert } from 'reactstrap';

let initialEntryState = {};

const EditEntry = (props) => {
    const [entryState, setEntryState] = useState(initialEntryState);
    const [isSuccessful, setIsSuccessful] = useState(false);
    const [hasError, setHasError] = useState(false);

    useEffect(() => {
        const { contactId, contactPhoneId } = props.location.state;

        const getURL = resolveApiURL("getContactPhone", { contactId: contactId, contactPhoneId: contactPhoneId });
        axios.get(getURL)
            .then(function (response) {
                const { contactPhones, ...otherInfo } = response.data;

                initialEntryState = {
                    ...contactPhones[0],
                    ...otherInfo
                }

                setEntryState(initialEntryState);
            })
            .catch(function (error) {
                setHasError(true);
                setIsSuccessful(false);
            });
    }, []);

    const reinstateInitialValues = () => {
        setEntryState(prevState => {
            return { ...prevState, ...initialEntryState };
        });
    }

    const onValuesChange = (data) => {
        const { name, value } = data;
        setEntryState({ ...entryState, [name]: value });
    }

    const editEntry = () => {
        const { firstName, lastName, ...contactPhone } = { ...entryState };

        if (isEmpty(entryState) ||
            !isPhoneValid(contactPhone)
        )
            return;

        axios.put(resolveApiURL("put", entryState.contactId), {
            contactId: contactPhone.contactId,
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
    };

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
            <PhoneBookEntryForm entry={entryState} onClear={reinstateInitialValues} onChange={onValuesChange} onSubmit={editEntry} />
        </div>
    );
};

export default EditEntry;