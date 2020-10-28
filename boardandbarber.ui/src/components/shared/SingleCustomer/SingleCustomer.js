import React from 'react';
import './SingleCustomer.scss';

class SingleCustomer extends React.Component {
    render() {
        const {customer} = this.props;
        return (
            <div>
                <strong>Name: {customer.name}</strong>
                <ul>
                    <li>Birthday: {customer.birthday}</li>
                    <li>Favorite Barber: {customer.favoriteBarber}</li>
                    <li>Notes: {customer.notes}</li>
                </ul>
            </div>
        )
    }
}

export default SingleCustomer;