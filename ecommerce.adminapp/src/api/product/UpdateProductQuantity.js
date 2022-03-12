import axios from "../axios"
async function UpdateProductQuantity(productId, quantity) {
    try {
        const response = await axios.patch(
          `api/Products/${productId}/quantity/${quantity}`,
          null,
          {
            headers: {
              'Content-Type': 'application/json',
              Authorization:  sessionStorage.getItem("token")
            }
          }
        );
    
        return response.status === 200 ? true : false;
      }  
      catch(err){
        return err.response;
      }
}

export default UpdateProductQuantity;