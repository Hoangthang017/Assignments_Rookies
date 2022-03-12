import axios from '../axios'

async function GetAllPaging(pageIndex, pageSize,languageId) {
    try {
        const response = await axios.get(
            `api/categories/paging/${languageId}?PageIndex=${pageIndex}&PageSize=${pageSize}`, 
        { 
            headers: { Authorization:  sessionStorage.getItem("token")} 
        })
    
        let category = response.data;
    
        return category;
      }
      catch(err) {
        console.log(err);
      }
}

export default GetAllPaging