import axios from '../axios'

async function CreateUserApi(data) {
    try {
        const reponse = await axios.post(
          "api/Users/register/admin",
          data,
          {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json',
              Authorization:  sessionStorage.getItem("token")
            }
          }
        );
    
        return reponse.data;
      }
      catch(err) {
        console.log(err)
        return err.response;
      }
}

export default CreateUserApi;