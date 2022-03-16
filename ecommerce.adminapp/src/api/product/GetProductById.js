import axios from "../axios"

async function GetProductById(productId,languageId) {
    try {
        const product = await axios.get(`api/products/${languageId}/${productId}`,
        { headers: { Authorization: sessionStorage.getItem("token")} 
       })

       return product.data;
    }
    catch (err){
        console.log(err);
    }
    
}

export default GetProductById