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
  Switch,
  Input,
  Button,
  Hidden
} from '@mui/material';
import { LoadingButton } from '@mui/lab';
import CreateUserApi from '../../api/user/CreateUserApi';
// component
import Iconify from '../../components/Iconify';
import DatePicker from '@mui/lab/DatePicker';
import { values } from 'lodash';
import GetUserById from 'src/api/user/GetUserById';
import UpdateUser from 'src/api/user/UpdateUser';
import { set } from 'date-fns';
import CreateUserAvatar from 'src/api/user/CreateUserAvatar';
import UpdateUserAvatar from 'src/api/user/UpdateUserAvatar';

function CreateUser() {
  // states
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
  const [sourceImage, setSourceImage] = useState(
    '../static/illustrations/illustration_register.jpg'
  );
  const [userAvatar, setUserAvatar] = useState([]);
  const [hasChangeValue, SetHasChangeValue] = useState(false);

  // validation
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

  // call api
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
        setSourceImage(response.data.avatarUrl);
      }
    }
  }, []);

  // use formik to validate from
  const formik = useFormik({
    initialValues: initalUser,
    validationSchema: RegisterSchema,
    enableReinitialize: true,
    // function handle submit
    onSubmit: async () => {
      // base request
      var baseRequest = {
        firstName: values.firstName,
        lastName: values.lastName,
        dateOfBirth: dateOfBirth,
        email: values.email,
        phoneNumber: values.phoneNumber,
        isAdmin: values.isAdmin
      };
      // update
      if (params.id) {
        const responseUpdate = await UpdateUser(params.id, baseRequest);

        if (userAvatar[0]) {
          const responseCreateImage = await CreateUserAvatar({
            userId: params.id,
            caption: userAvatar[0].name,
            imageFile: userAvatar[0]
          });
        }
        setUpdateSuccess(true);
        SetHasChangeValue(false);
      }
      // create
      else {
        const response = await CreateUserApi({
          ...baseRequest,
          userName: values.userName,
          password: values.password,
          userAvatar: userAvatar[0]
        });

        if (userAvatar[0]) {
          const responseCreateImage = await CreateUserAvatar({
            userId: response.id,
            caption: userAvatar[0].name,
            imageFile: userAvatar[0]
          });
        }

        if (response) {
          navigate('/user', { replace: true });
        } else {
          console.log('Faild to create account');
        }
      }
    }
  });

  // initial formik
  const { errors, touched, values, handleSubmit, isSubmitting, getFieldProps } = formik;

  // upload image
  const [updateAvatarSuccess, setUpdateAvatarSuccess] = useState(false);
  function UploadHandler(e) {
    var file = e.target.files[0];
    setUserAvatar([file]);
    var reader = new FileReader();
    var url = reader.readAsDataURL(file);

    reader.onloadend = function (e) {
      setSourceImage(reader.result);
    }.bind(this);
  }

  // save image
  async function SaveImageHandler() {
    if (params.id) {
      const responseUpdateImage = await UpdateUserAvatar({
        userId: params.id,
        caption: userAvatar[0].name,
        imageFile: userAvatar[0]
      });
      if (responseUpdateImage === 200) {
        setUserAvatar([]);
        setUpdateAvatarSuccess(true);
      }
    }
  }

  return (
    <Page title={`${params.id ? 'Edit User' : 'New User'} | CHAPTER-INFINITY`}>
      <Container>
        <Typography variant="h4" gutterBottom>
          {params.id ? 'Edit User' : 'New User'}
        </Typography>
        <Stack direction="row" justifyContent="center" sx={{ mt: '3rem' }}>
          {/* avatar */}
          <Container
            sx={{
              width: '40%'
            }}
            spacing={3}
          >
            {updateAvatarSuccess && (
              <Alert
                onClose={() => {
                  setUpdateAvatarSuccess(false);
                }}
              >
                Success to update your avatar !!!
              </Alert>
            )}
            <Card
              sx={{
                m: 'auto',
                p: '2rem'
              }}
              spacing={2}
            >
              <img
                style={{
                  borderRadius: '0.5rem',
                  marginBottom: '2rem'
                }}
                src={sourceImage}
                alt="your avatar"
              />
              <label htmlFor="contained-button-file">
                <input
                  hidden
                  accept="image/*"
                  id="contained-button-file"
                  multiple
                  type="file"
                  onChange={UploadHandler}
                />
                <Button
                  color="secondary"
                  sx={{
                    mb: '1rem'
                  }}
                  fullWidth
                  size="large"
                  variant="contained"
                  component="span"
                >
                  Upload
                </Button>

                {params.id && userAvatar[0] && (
                  <Button fullWidth size="large" onClick={SaveImageHandler} variant="contained">
                    save
                  </Button>
                )}
              </label>
            </Card>
          </Container>

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
                          value={dateOfBirth}
                          onChange={(newValue) => {
                            setDateOfBirth(newValue);
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
                        control={<Switch checked={values.isAdmin} {...getFieldProps('isAdmin')} />}
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
