import axios from '../axios'

async function GetAllProductPaging(pageIndex, pageSize,languageId,categoryId) {
    try {
        const response = await axios.get(
            `api/products/paging/${languageId}?PageIndex=${pageIndex}&PageSize=${pageSize}&CategoryId=${categoryId}`, 
        { 
            headers: { Authorization:  sessionStorage.getItem("token")} 
        })
    
        return response.data;
      }
      catch(err) {
        console.log(err);
      }
}

export default GetAllProductPaging