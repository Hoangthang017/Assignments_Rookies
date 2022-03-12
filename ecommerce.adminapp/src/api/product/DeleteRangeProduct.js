import axios from "../axios"

async function DeleteRangeProduct({productIds}) {
    try {
        const response = await axios.patch("api/products/deleteRange",
        productIds, 
        { 
            'Content-Type': 'application/json',
            headers: { Authorization:  sessionStorage.getItem("token")} 
        })

        if (response.status === 200)
            return true;
    }
    catch(err) {
        console.log(err);
    }
}

export default DeleteRangeProduct;