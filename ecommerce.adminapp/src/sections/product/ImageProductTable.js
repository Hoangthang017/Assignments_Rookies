import { filter, set, values } from 'lodash';
import { useEffect, useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
// material
import {
  Card,
  Table,
  Stack,
  Alert,
  Button,
  Checkbox,
  TableRow,
  TableBody,
  TableCell,
  Container,
  Typography,
  TableContainer,
  TablePagination,
  TextField,
  FormControlLabel,
  Switch
} from '@mui/material';
// components
import Page from '../../components/Page';
import Label from '../../components/Label';
import Scrollbar from '../../components/Scrollbar';
import Iconify from '../../components/Iconify';
import SearchNotFound from '../../components/SearchNotFound';
import { UserListHead, UserListToolbar, UserMoreMenu } from '../../sections/@dashboard/user';
import GetAllProductPaging from 'src/api/product/GetAllProductPaging';
import GetAllProductImage from 'src/api/product/GetAllProductImage';
import CreateProductImage from 'src/api/product/CreateProductImage';
import UpdateProductImage from 'src/api/product/UpdateProductImage';
import ProductImageMoreMenu from './ProductImageMoreMenu';
import AlertModal from 'src/sections/Modal/AlertModal';

// import USERLIST from '../_mocks_/user';

// ----------------------------------------------------------------------
// call api
const defaultImage = 'https://localhost:7195/user-content/product/default-product-image.png';
// cài đặt các trường cho bảng
const TABLE_HEAD = [
  { id: 'imagePath', label: 'Image', alignRight: false },
  { id: 'caption', label: 'Caption', alignRight: false },
  { id: 'isDefault', label: 'Defalut', alignRight: false },
  { id: '' }
];
// ----------------------------------------------------------------------

// sắp xếp
function descendingComparator(a, b, orderBy) {
  if (b[orderBy] < a[orderBy]) {
    return -1;
  }
  if (b[orderBy] > a[orderBy]) {
    return 1;
  }
  return 0;
}

function getComparator(order, orderBy) {
  return order === 'desc'
    ? (a, b) => descendingComparator(a, b, orderBy)
    : (a, b) => -descendingComparator(a, b, orderBy);
}

function applySortFilter(array, comparator, query) {
  const stabilizedThis = array.map((el, index) => [el, index]);
  stabilizedThis.sort((a, b) => {
    const order = comparator(a[0], b[0]);
    if (order !== 0) return order;
    return a[1] - b[1];
  });
  if (query) {
    return filter(array, (_user) => _user.name.toLowerCase().indexOf(query.toLowerCase()) !== -1);
  }
  return stabilizedThis.map((el) => el[0]);
}

//--------------------------------------------------------------------------------------------
export default function ImageProductTable({ productId }) {
  // các state
  const [idRemoveRow, setIdRemoveRow] = useState(false);
  const [IMAGE_LIST, setIMAGE_LIST] = useState([]);
  const [page, setPage] = useState(0);
  const [order, setOrder] = useState('asc');
  const [selected, setSelected] = useState([]);
  const [orderBy, setOrderBy] = useState('name');
  const [filterName, setFilterName] = useState('');
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [count, setCount] = useState(0);
  const [isNewDefault, setIsNewDefault] = useState(true);
  const [newCaption, setNewCaption] = useState('');
  const [expandUpload, setExpendUpload] = useState(false);
  const [updateAvatarSuccess, setUpdateAvatarSuccess] = useState(false);
  const [createImageSuccess, setCreateImageSuccess] = useState(false);
  const [imageId, setImageId] = useState(0);

  // call api
  useEffect(async () => {
    if (productId && productId != 0) {
      const response = await GetAllProductImage(page + 1, rowsPerPage, productId);
      if (response.items) {
        setIMAGE_LIST(response.items);
        setCount(response.totalRecords);
        setCreateImageSuccess(false);
        setSourceImage(defaultImage);
        setIsNewDefault(true);
        setNewCaption('');
      }
      if (idRemoveRow) {
        setIdRemoveRow(false);
      }
    }
  }, [idRemoveRow, page, rowsPerPage, createImageSuccess]);

  // xử lí sắp xếp tăng giảm
  const handleRequestSort = (event, property) => {
    const isAsc = orderBy === property && order === 'asc';
    setOrder(isAsc ? 'desc' : 'asc');
    setOrderBy(property);
  };

  // chọn tất cả checkbox
  const handleSelectAllClick = (event) => {
    if (event.target.checked) {
      const newSelecteds = USERLIST.map((n) => n.id);
      setSelected(newSelecteds);
      return;
    }
    setSelected([]);
  };

  // chọn 1 checkbox
  const handleClick = (event, name) => {
    const selectedIndex = selected.indexOf(name);
    let newSelected = [];
    if (selectedIndex === -1) {
      newSelected = newSelected.concat(selected, name);
    } else if (selectedIndex === 0) {
      newSelected = newSelected.concat(selected.slice(1));
    } else if (selectedIndex === selected.length - 1) {
      newSelected = newSelected.concat(selected.slice(0, -1));
    } else if (selectedIndex > 0) {
      newSelected = newSelected.concat(
        selected.slice(0, selectedIndex),
        selected.slice(selectedIndex + 1)
      );
    }
    setSelected(newSelected);
  };

  // thay đổ số trang
  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  // thay đổi số dòng
  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  // thêm cột trống để căn chỉnh bảng
  const emptyRows = page > 0 ? Math.max(0, (1 + page) * rowsPerPage - count) : 0;

  // sắp xếp theo tên
  const filteredUsers = applySortFilter(IMAGE_LIST, getComparator(order, orderBy), filterName);

  // không có dòng nào
  const isUserNotFound = filteredUsers.length === 0;

  // upload image
  const [sourceImage, setSourceImage] = useState(defaultImage);
  const [productImage, setProductImage] = useState([File]);
  const [isOpenModalConfirm, setIsOpenModalConfirm] = useState(false);

  function UploadHandler(e) {
    var file = e.target.files[0];
    setProductImage([file]);
    var reader = new FileReader();
    var url = reader.readAsDataURL(file);

    reader.onloadend = function (e) {
      setSourceImage(reader.result);
    }.bind(this);
  }

  // save image
  async function SaveImageHandler() {
    if (imageId != 0) {
      const responseUpdateImage = await UpdateProductImage({
        imageId,
        productId,
        caption: newCaption ? newCaption : productImage[0].name,
        isDefault: isNewDefault
      });
      setCreateImageSuccess(true);
    } else {
      const responseUpdateImage = await CreateProductImage({
        productId,
        isDefault: isNewDefault,
        caption: newCaption ? newCaption : productImage[0].name,
        imageFile: productImage[0]
      });
      setCreateImageSuccess(true);
    }
    setExpendUpload(false);
  }

  function expandNewImageHandler() {
    setExpendUpload(true);
    setImageId(0);
    setIsNewDefault(true);
    setNewCaption('');
    setSourceImage(defaultImage);
    window.scrollTo({ left: 0, top: document.body.scrollHeight, behavior: 'smooth' });
  }

  return (
    <>
      <Container>
        <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
          <Typography variant="h4" gutterBottom></Typography>
          <Button
            variant="contained"
            onClick={expandNewImageHandler}
            startIcon={<Iconify icon="eva:plus-fill" />}
          >
            New Image
          </Button>
        </Stack>

        <Card variant="outlined">
          <Scrollbar>
            <TableContainer sx={{ minWidth: 800 }}>
              <Table>
                {/* setting for table databae */}
                <UserListHead
                  order={order}
                  orderBy={orderBy}
                  headLabel={TABLE_HEAD}
                  rowCount={count}
                  numSelected={selected.length}
                  onRequestSort={handleRequestSort}
                  onSelectAllClick={handleSelectAllClick}
                />
                <TableBody>
                  {/* map to row of table */}
                  {filteredUsers
                    // .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                    .map((row) => {
                      const { id, imagePath, isDefault, caption } = row;
                      const isItemSelected = selected.indexOf(id) !== -1;

                      return (
                        <TableRow
                          hover
                          key={id}
                          tabIndex={-1}
                          role="checkbox"
                          selected={isItemSelected}
                          aria-checked={isItemSelected}
                        >
                          {/* cột checkbox */}
                          <TableCell padding="checkbox">
                            <Checkbox
                              checked={isItemSelected}
                              onChange={(event) => handleClick(event, id)}
                            />
                          </TableCell>

                          {/* cột avatar + names */}
                          <TableCell align="left" mx={{ m: 0 }}>
                            <img
                              alt={caption}
                              src={imagePath}
                              style={{
                                width: '20%',
                                margin: 0
                              }}
                            />
                          </TableCell>

                          {/* cột company */}
                          <TableCell align="left">{caption}</TableCell>

                          {/* status */}
                          <TableCell align="left">
                            <Label variant="ghost" color={(isDefault && 'success') || 'error'}>
                              {isDefault ? 'Default' : 'NON Default'}
                            </Label>
                          </TableCell>

                          <TableCell align="right">
                            <ProductImageMoreMenu
                              id={id}
                              IMAGE_LIST={IMAGE_LIST}
                              setIdRemoveRow={setIdRemoveRow}
                              setSourceImage={setSourceImage}
                              setNewCaption={setNewCaption}
                              setIsNewDefault={setIsNewDefault}
                              setExpendUpload={setExpendUpload}
                              setImageId={setImageId}
                            />
                          </TableCell>
                        </TableRow>
                      );
                    })}
                  {emptyRows > 0 && (
                    <TableRow style={{ height: 53 * emptyRows }}>
                      <TableCell colSpan={6} />
                    </TableRow>
                  )}
                </TableBody>
                {isUserNotFound && (
                  <TableBody>
                    <TableRow>
                      <TableCell align="center" colSpan={6} sx={{ py: 3 }}>
                        <SearchNotFound searchQuery={filterName} />
                      </TableCell>
                    </TableRow>
                  </TableBody>
                )}
              </Table>
            </TableContainer>
          </Scrollbar>

          <TablePagination
            rowsPerPageOptions={[5, 10, 25]}
            component="div"
            count={count}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
          />
        </Card>

        {expandUpload && (
          <Card variant="outlined" sx={{ mt: '2rem' }}>
            {createImageSuccess && (
              <Alert severity="success" sx={{ mb: '3rem' }}>
                Success!!!
              </Alert>
            )}
            <Stack
              sx={{ mt: '0.5rem', mr: '0.5rem' }}
              direction="row"
              alignItems="center"
              justifyContent="space-between"
              mb={5}
            >
              <Typography variant="h4" gutterBottom></Typography>
              <Button
                color="error"
                variant="contained"
                onClick={() => setExpendUpload(false)}
                startIcon={<Iconify icon="ep:close" />}
              >
                Hidden
              </Button>
            </Stack>
            <Stack direction="row">
              <Container>
                <Card
                  sx={{
                    m: '2rem',
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
                    {!imageId && (
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
                    )}

                    {productImage[0] &&
                      sourceImage !== defaultImage &&
                      (imageId === 0 ? (
                        <Button
                          fullWidth
                          size="large"
                          onClick={SaveImageHandler}
                          variant="contained"
                        >
                          Add
                        </Button>
                      ) : (
                        <Button
                          fullWidth
                          size="large"
                          onClick={() => setIsOpenModalConfirm(true)}
                          variant="contained"
                        >
                          Update
                        </Button>
                      ))}
                  </label>
                </Card>
              </Container>
              <Container sx={{ m: 'auto' }}>
                <TextField
                  sx={{ mb: '1.5rem' }}
                  fullWidth
                  label="Caption"
                  value={newCaption}
                  onChange={(e) => setNewCaption(e.target.value)}
                />
                <FormControlLabel
                  sx={{ justifyContent: 'flex-end' }}
                  control={
                    <Switch
                      checked={isNewDefault}
                      onChange={(e) => setIsNewDefault(e.target.checked)}
                    />
                  }
                  label="Thumbnail"
                />
              </Container>
            </Stack>
          </Card>
        )}
      </Container>
      <AlertModal
        isOpen={isOpenModalConfirm}
        setIsOpen={setIsOpenModalConfirm}
        title={'Update Confirm'}
        message={'Are you sure want to update your row'}
        setAgreeAction={SaveImageHandler}
      ></AlertModal>
    </>
  );
}
