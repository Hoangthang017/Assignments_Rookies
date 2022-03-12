import axios from "../axios"

async function GetProductById(productId,languageId) {
    try {
        const product = await axios.get(`api/products/${productId}/${languageId}`,
        { headers: { Authorization: sessionStorage.getItem("token")} 
       })

       return product.data;
    }
    catch (err){
        console.log(err);
    }
    
}

export default GetProductById