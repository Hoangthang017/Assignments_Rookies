import axios from '../axios'

async function UpdateCategory(categoryId,languageId, data) {
    try {
        const reponse = await axios.put(
          `api/categories/${categoryId}/${languageId}`,
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

export default UpdateCategory;