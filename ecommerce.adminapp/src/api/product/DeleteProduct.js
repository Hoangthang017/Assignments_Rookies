import axios from "../axios"

async function DeleteProduct(productId) {
    try {
        const response = await axios.delete(`api/products/${productId}`, 
        { 
            headers: { Authorization:  sessionStorage.getItem("token")} 
        })

        if (response.status === 200)
            return true;
      }
      catch(err) {
        console.log(err);
      }
}

export default DeleteProduct;