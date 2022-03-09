import axios from "../axios";

async function LoginUser(credentials) {
  try {
    const reponse = await axios.post(
      "api/Users/authenticate",
      JSON.stringify({
        ClientId: 'react-admin',
        ClientSecret: 'D013F030-0177-4F0D-AECA-1206D0608408',
        Scope: 'openid profile swaggerApi userInfor email phone',
        ...credentials
      }),
      {
        headers: {
          'Content-Type': 'application/json'
        }
      }
    );

    return reponse.data;
  }
  catch(err) {

    return err.response;
  }
}

export default LoginUser;
