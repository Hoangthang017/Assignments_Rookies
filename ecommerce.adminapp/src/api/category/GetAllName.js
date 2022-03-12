import axios from '../axios'

async function GetAllName(languageId) {
    try {
        const response = await axios.get(
            `api/categories/${languageId}`, 
        { 
            headers: { Authorization:  sessionStorage.getItem("token")} 
        })
    
        return response.data;
      }
      catch(err) {
        console.log(err);
      }
}

export default GetAllName