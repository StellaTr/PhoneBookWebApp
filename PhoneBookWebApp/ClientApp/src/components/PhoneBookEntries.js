import React from 'react';
import { Table } from 'reactstrap';

const TableRow = (props) => (
    <tr>
        <td>{props.entry.firstName}</td>
        <td>{props.entry.lastName}</td>
        <td>+{props.entry.countryCode} {props.entry.areaCode} {props.entry.phoneNumber}</td>        
    </tr>
);


const PhoneBookEntries = (props) => (
    <Table>
        <thead>
            <tr>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Phone Number</th>
                <th />
            </tr>
        </thead>
        <tbody>
            {
                props.entries.map((entry) => {
                    return (<TableRow key={entry.contactPhoneId} entry={entry} />)
                })
            }
        </tbody>
    </Table>
);

export default PhoneBookEntries;