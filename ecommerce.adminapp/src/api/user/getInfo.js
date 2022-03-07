import axios from "../axios";

export default async function getInfo() {
  try {
    const account = await axios.post('api/Users', null, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: sessionStorage.getItem('token')
      }
    });
    account['photoURL'] =
      'https://serving.photos.photobox.com/1651493216a133738ce610dd288ef961463a44cafe827227e79ed1b5a635475d2b92c23e.jpg';
    return account;
  } catch (err) {
    console.log(err);
  }
}
