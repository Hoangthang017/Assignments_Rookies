import React, { useEffect } from 'react';
import Page from '../../components/Page';
import * as Yup from 'yup';
import { useState } from 'react';
import { Link as RouterLink, useNavigate, useParams } from 'react-router-dom';
import { useFormik, Form, FormikProvider, yupToFormErrors } from 'formik';
import AdapterDateFns from '@mui/lab/AdapterDateFns';
import LocalizationProvider from '@mui/lab/LocalizationProvider';
// material
import {
  Link,
  Stack,
  TextField,
  IconButton,
  InputAdornment,
  FormControlLabel,
  Alert,
  Container,
  Typography,
  Card,
  Switch
} from '@mui/material';
import { LoadingButton } from '@mui/lab';
import CreateUserApi from '../../api/user/CreateUserApi';
// component
import Iconify from '../../components/Iconify';
import DatePicker from '@mui/lab/DatePicker';
import { values } from 'lodash';
import GetUserById from 'src/api/user/GetUserById';
import UpdateUser from 'src/api/user/UpdateUser';

function CreateUser() {
  const params = useParams();
  const navigate = useNavigate();
  const [dateOfBirth, setDateOfBirth] = useState(new Date('01/01/2000'));
  const [showPassword, setShowPassword] = useState(false);
  const [updateSuccess, setUpdateSuccess] = useState(false);

  const [initalUser, setInitalUser] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    userName: '',
    password: '',
    confirmPassword: '',
    isAdmin: false
  });

  const RegisterSchema = Yup.object().shape({
    firstName: Yup.string()
      .min(2, 'Too Short!')
      .max(50, 'Too Long!')
      .required('First name required'),
    lastName: Yup.string().min(2, 'Too Short!').max(50, 'Too Long!').required('Last name required'),
    userName: Yup.string().min(6, 'Too Short!').max(50, 'Too Long!').required('User name required'),
    email: Yup.string().email('Email must be a valid email address'),
    password: Yup.string().required('Password is required'),
    confirmPassword: Yup.string().oneOf([Yup.ref('password'), null], 'Passwords must match')
  });

  useEffect(async () => {
    if (params.id) {
      const response = await GetUserById(params.id);

      if (response.data) {
        if (response.data.role === 'admin') response.data['isAdmin'] = true;
        else response.data['isAdmin'] = false;
        response.data['password'] = 'Sample@123';
        response.data['confirmPassword'] = 'Sample@123';
        setDateOfBirth(new Date(response.data.dateOfBirth));
        setInitalUser(response.data);
      }
    }
  }, []);

  const formik = useFormik({
    initialValues: initalUser,
    validationSchema: RegisterSchema,
    enableReinitialize: true,
    onSubmit: async () => {
      var baseRequest = {
        firstName: values.firstName,
        lastName: values.lastName,
        dateOfBirth: dateOfBirth,
        email: values.email,
        phoneNumber: values.phoneNumber,
        isAdmin: values.isAdmin
      };
      if (params.id) {
        const responseUpdate = await UpdateUser(params.id, baseRequest);
        setUpdateSuccess(true);
      } else {
        const response = await CreateUserApi({
          ...baseRequest,
          userName: values.userName,
          password: values.password
        });

        if (response) {
          navigate('/user', { replace: true });
        } else {
          console.log('Faild to create account');
        }
      }
    }
  });

  const { errors, touched, values, handleSubmit, isSubmitting, getFieldProps } = formik;

  return (
    <Page title="New User | Minimal-UI">
      <Container>
        <Typography variant="h4" gutterBottom>
          {params.id ? 'Edit User' : 'New User'}
        </Typography>
        <Stack direction="row" justifyContent="center" sx={{ mt: '3rem' }}>
          {/* avatar */}
          {/* <Card>
              <img src="/static/illustrations/illustration_register.jpg" alt="register" />
            </Card> */}

          {/* information */}
          <Card sx={{ p: '3rem' }}>
            {updateSuccess && (
              <Alert severity="success" sx={{ mb: '3rem' }}>
                Success!!!
              </Alert>
            )}
            <FormikProvider value={formik}>
              <Form autoComplete="off" noValidate onSubmit={handleSubmit}>
                <Stack spacing={3}>
                  {/* information */}
                  <Stack direction="row">
                    {/* basic information */}
                    <Stack spacing={3} sx={{ mr: '1.5rem' }}>
                      <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
                        <TextField
                          fullWidth
                          label="First name"
                          {...getFieldProps('firstName')}
                          error={Boolean(touched.firstName && errors.firstName)}
                          helperText={touched.firstName && errors.firstName}
                        />

                        <TextField
                          fullWidth
                          label="Last name"
                          {...getFieldProps('lastName')}
                          error={Boolean(touched.lastName && errors.lastName)}
                          helperText={touched.lastName && errors.lastName}
                        />
                      </Stack>

                      <LocalizationProvider dateAdapter={AdapterDateFns}>
                        <DatePicker
                          label="Date Of Birth"
                          onChange={(dateOfBirth) => {
                            setDateOfBirth(dateOfBirth);
                          }}
                          //{...getFieldProps('dateOfBirth')}
                          renderInput={(params) => <TextField {...params} />}
                        />
                      </LocalizationProvider>

                      <TextField
                        fullWidth
                        type="email"
                        label="Email address"
                        {...getFieldProps('email')}
                        error={Boolean(touched.email && errors.email)}
                        helperText={touched.email && errors.email}
                      />

                      <TextField
                        fullWidth
                        label="Phone Number"
                        {...getFieldProps('phoneNumber')}
                        error={Boolean(touched.phoneNumber && errors.phoneNumber)}
                        helperText={touched.phoneNumber && errors.phoneNumber}
                      />
                    </Stack>

                    {/* account information */}

                    <Stack spacing={3} sx={{ width: '50%', ml: '1.5rem' }}>
                      <TextField
                        disabled={!!params.id}
                        fullWidth
                        type="userName"
                        label="User Name"
                        {...getFieldProps('userName')}
                        error={Boolean(touched.userName && errors.userName)}
                        helperText={touched.userName && errors.userName}
                      />
                      {!params.id && (
                        <>
                          <TextField
                            fullWidth
                            autoComplete="Admin@123"
                            type={showPassword ? 'text' : 'password'}
                            label="Password"
                            {...getFieldProps('password')}
                            InputProps={{
                              endAdornment: (
                                <InputAdornment position="end">
                                  <IconButton
                                    edge="end"
                                    onClick={() => setShowPassword((prev) => !prev)}
                                  >
                                    <Iconify
                                      icon={showPassword ? 'eva:eye-fill' : 'eva:eye-off-fill'}
                                    />
                                  </IconButton>
                                </InputAdornment>
                              )
                            }}
                            error={Boolean(touched.password && errors.password)}
                            helperText={touched.password && errors.password}
                          />

                          <TextField
                            fullWidth
                            autoComplete="Admin@123"
                            type={showPassword ? 'text' : 'password'}
                            label="Confirm Password"
                            {...getFieldProps('confirmPassword')}
                            InputProps={{
                              endAdornment: (
                                <InputAdornment position="end">
                                  <IconButton
                                    edge="end"
                                    onClick={() => setShowPassword((prev) => !prev)}
                                  >
                                    <Iconify
                                      icon={showPassword ? 'eva:eye-fill' : 'eva:eye-off-fill'}
                                    />
                                  </IconButton>
                                </InputAdornment>
                              )
                            }}
                            error={Boolean(touched.password && errors.password)}
                            helperText={touched.password && errors.password}
                          />
                        </>
                      )}

                      <FormControlLabel
                        sx={{ justifyContent: 'flex-end' }}
                        control={<Switch checked={values.isAdmin} {...getFieldProps('isAdmin')}/>}
                        label="Admin"
                      />
                    </Stack>
                  </Stack>

                  {/* register button */}
                  <LoadingButton
                    fullWidth
                    size="large"
                    type="submit"
                    variant="contained"
                    loading={isSubmitting}
                  >
                    {params.id ? 'Update' : 'Register'}
                  </LoadingButton>
                </Stack>
              </Form>
            </FormikProvider>
          </Card>
        </Stack>
      </Container>
    </Page>
  );
}

export default CreateUser;
