
import axios from '../axios';

export default async function GetAccountInfo(token) {
  const response = await axios.get('api/Users/account', { headers: { Authorization: "Bearer " + token} })

  let account = response.data;
  account['photoURL'] =
    'https://serving.photos.photobox.com/1651493216a133738ce610dd288ef961463a44cafe827227e79ed1b5a635475d2b92c23e.jpg';
  return account;
}
