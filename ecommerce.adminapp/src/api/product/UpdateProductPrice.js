import axios from "../axios"
async function UpdateProductPrice(productId, newPrice) {
    try {
        const response = await axios.patch(
          `api/Products/${productId}/price/${newPrice}`,
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

export default UpdateProductPrice;