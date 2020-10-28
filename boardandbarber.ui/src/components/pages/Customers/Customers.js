import React from 'react';
import './Customers.scss';
import customerData from '../../../helpers/data/customersData';
import SingleCustomer from '../../shared/SingleCustomer/SingleCustomer';

class Customers extends React.Component {
    state = { customers: [] };

    componentDidMount() {
        customerData.getAllCustomers()
            .then(customers => {this.setState({customers})});
    }

    render() {
        const {customers} = this.state;

        const buildCustomerList = customers.map((customer) => {
            return (
            <SingleCustomer key={customer.id} customer={customer} />   
            )
        });

        return (
            <div>
                {buildCustomerList}
            </div>
        )
    }
};

export default Customers;