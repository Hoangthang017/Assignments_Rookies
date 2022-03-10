import * as Yup from 'yup';
import { useState } from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { useFormik, Form, FormikProvider } from 'formik';
// material
import {
  Link,
  Stack,
  Checkbox,
  TextField,
  IconButton,
  InputAdornment,
  FormControlLabel,
  Alert
} from '@mui/material';
import { LoadingButton } from '@mui/lab';
// component
import Iconify from '../../../components/Iconify';

// Api
import LoginUser from '../../../api/user/LoginUser';
import GetAccountInfo from '../../../api/user/GetAccountInfo';
import { string } from 'prop-types';
// ----------------------------------------------------------------------

export default function LoginForm() {
  // states
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState('');

  // validation user pasword
  const LoginSchema = Yup.object().shape({
    username: Yup.string().required('Tài khoản không được bỏ trống'),
    password: Yup.string().required('Mật khẩu không được bỏ trống')
  });

  const formik = useFormik({
    initialValues: {
      username: '',
      password: '',
      remember: true
    },
    validationSchema: LoginSchema,
    onSubmit: async () => {
      const response = await LoginUser(
        {
          UserName: values.username,
          Password: values.password
        }
      );

      if (response.token) {
        var account = await GetAccountInfo(response.token);

        if (account.role === 'admin') {
          sessionStorage.setItem('token', 'Bearer ' + response.token);
          sessionStorage.setItem('account', JSON.stringify(account));
          navigate('/dashboard/app', { replace: true });
        } else {
          setError('Bạn không có quyền truy cập');
        }
      }
      else{
        if (response.status === 400){
          setError('Sai tài khoản hoặc mật khẩu');
        }
      }
    }
  });

  const { errors, touched, values, isSubmitting, handleSubmit, getFieldProps } = formik;

  const handleShowPassword = () => {
    setShowPassword((show) => !show);
  };

  return (
    <Stack>
      {error && <Alert severity="error" sx={{mb: '3rem'}}>{error}</Alert>} 
      <FormikProvider value={formik}>
        <Form autoComplete="off" noValidate onSubmit={handleSubmit}>
          <Stack spacing={3}>
            <TextField
              fullWidth
              label="Tên đăng nhập"
              {...getFieldProps('username')}
              error={Boolean(touched.username && errors.username)}
              helperText={touched.username && errors.username}
            />

            <TextField
              fullWidth
              autoComplete="current-password"
              type={showPassword ? 'text' : 'password'}
              label="Mật khẩu"
              {...getFieldProps('password')}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton onClick={handleShowPassword} edge="end">
                      <Iconify icon={showPassword ? 'eva:eye-fill' : 'eva:eye-off-fill'} />
                    </IconButton>
                  </InputAdornment>
                )
              }}
              error={Boolean(touched.password && errors.password)}
              helperText={touched.password && errors.password}
            />
          </Stack>

          <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ my: 2 }}>
            <FormControlLabel
              control={<Checkbox {...getFieldProps('remember')} checked={values.remember} />}
              label="Nhớ mật khẩu"
            />

            {/* <Link component={RouterLink} variant="subtitle2" to="#" underline="hover">
            Forgot password?
          </Link> */}
          </Stack>

          <LoadingButton
            fullWidth
            size="large"
            type="submit"
            variant="contained"
            loading={isSubmitting}
          >
            Đăng nhập
          </LoadingButton>
        </Form>
      </FormikProvider>
    </Stack>
  );
}
