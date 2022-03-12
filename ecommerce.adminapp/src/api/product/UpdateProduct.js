import axios from '../axios'

async function UpdateProduct(productId, languageId, data) {
    try {
        const response = await axios.put(
          `api/products/${productId}/${languageId}`,
          data,
          {
            headers: {
              'Content-Type': 'application/json',
              Authorization:  sessionStorage.getItem("token")
            }
          }
        );
    
        return response.status === 200 ? true : false;
      }
      catch(err) {
        return err.response;
      }
}

export default UpdateProduct