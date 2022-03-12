import axios from '../axios'

async function CreateCategory(data) {
    try {
        const reponse = await axios.post(
          "api/categories",
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

export default CreateCategory;