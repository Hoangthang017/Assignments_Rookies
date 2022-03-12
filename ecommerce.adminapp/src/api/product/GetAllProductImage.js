import axios from '../axios'

async function GetAllProductImage(pageIndex, pageSize,productId) {
    try {
        const response = await axios.get(
            `api/images/paging/${productId}?PageIndex=${pageIndex}&PageSize=${pageSize}`, 
        { 
            headers: { Authorization:  sessionStorage.getItem("token")} 
        })
    
        return response.data;
      }
      catch(err) {
        console.log(err);
      }
}

export default GetAllProductImage