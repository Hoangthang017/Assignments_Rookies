import axios from "../axios"
async function UpdateStatus(categoryId, status) {
    try {
        const reponse = await axios.patch(
          `api/categories/active/${categoryId}/${status}`,
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
      catch(err){
        return err.response;
      }
}

export default UpdateStatus;