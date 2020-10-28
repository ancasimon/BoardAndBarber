import axios from 'axios';
import {baseUrl} from '../constants.json';

const getAllCustomers = () => new Promise((resolve, reject) => {
    axios.get(`${baseUrl}/Customers`)
    .then(response => {
        let customers = response.data;
        resolve(customers);
    })
    .catch((error) => reject(error));
});

export default {getAllCustomers};