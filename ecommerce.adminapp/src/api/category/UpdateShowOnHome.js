import axios from "../axios"
async function UpdateShowOnHome(categoryId, isShowOnHome) {
    try {
        const reponse = await axios.patch(
          `api/categories/showOnHome/${categoryId}/${isShowOnHome}`,
          null,
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

export default UpdateShowOnHome