const getAllCustomers = () => new Promise((resolve, reject) => {
    resolve([
        {
            id:1,
            name:"nathan",
            birthday:"5/27/1986", 
            favoriteBarber:"Jimbo", 
            notes:"fancy"
        },
        {
            id:2,
            name:"Billy",
            birthday:"5/27/1986", 
            favoriteBarber:"Jimbo", 
            notes:"fancy"
        },
        {
            id:3,
            name:"Tommy",
            birthday:"5/27/1986", 
            favoriteBarber:"Jimbo", 
            notes:"fancy"
        }
    ])
} );

export default {getAllCustomers};