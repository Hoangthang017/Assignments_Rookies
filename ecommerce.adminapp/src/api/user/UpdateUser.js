import axios from '../axios'

async function UpdateUser(userId, data) {
    try {
        const reponse = await axios.put(
          `api/Users/${userId}`,
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

export default UpdateUser