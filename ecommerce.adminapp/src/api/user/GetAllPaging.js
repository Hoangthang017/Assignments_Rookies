import axios from '../axios'

async function GetAllPaging(pageIndex, pageSize) {
    try {
        const response = await axios.get(
            `api/Users/paging?PageIndex=${pageIndex}&PageSize=${pageSize}`, 
        { 
            headers: { Authorization:  sessionStorage.getItem("token")} 
        })
    
        let users = response.data;
    
        return users;
      }
      catch(err) {
        console.log(err);
      }
}

export default GetAllPaging