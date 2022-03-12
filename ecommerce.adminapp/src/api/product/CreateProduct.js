import axios from '../axios'

async function CreateProduct(data) {
    try {
        const reponse = await axios.post(
          "api/products",
          data,
          {
            headers: {
              'Content-Type': 'application/json',
              Authorization:  sessionStorage.getItem("token")
            }
          }
        );
    
        return reponse.data;
      }
      catch(err) {
        return err.response;
      }
}

export default CreateProduct;