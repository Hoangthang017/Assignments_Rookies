
import axios from '../axios';
import GetUserImagePath from './GetUserImagePath';

export default async function GetAccountInfo(token) {
  try {
    const response = await axios.get('api/Users/account', { headers: { Authorization: "Bearer " + token} })

    if (response.data) {
      let account = response.data;
      const urlResponse = await GetUserImagePath(account.sub)
  
      account['photoURL'] = urlResponse.data;
      return account;
    }
  
  }
  catch(err) {
    return err.response
  }
  
}
