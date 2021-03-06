import { filter } from 'lodash';
import { sentenceCase } from 'change-case';
import { useEffect, useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
// material
import {
  Card,
  Table,
  Stack,
  Avatar,
  Button,
  Checkbox,
  TableRow,
  TableBody,
  TableCell,
  Container,
  Typography,
  TableContainer,
  TablePagination
} from '@mui/material';
// components
import Page from '../components/Page';
import Label from '../components/Label';
import Scrollbar from '../components/Scrollbar';
import Iconify from '../components/Iconify';
import SearchNotFound from '../components/SearchNotFound';
import { UserListHead, UserListToolbar, UserMoreMenu } from '../sections/@dashboard/user';
import GetAllUser from 'src/api/user/GetAllUser';
import GetAllPaging from 'src/api/user/GetAllPaging';
import { LensTwoTone } from '@mui/icons-material';
import GetAllProductPaging from 'src/api/product/GetAllProductPaging';
//
// import USERLIST from '../_mocks_/user';

// ----------------------------------------------------------------------
// call api

// cài đặt các trường cho bảng
const TABLE_HEAD = [
  { id: 'name', label: 'Name', alignRight: false },
  { id: 'description', label: 'Description', alignRight: false },
  { id: 'categoryName', label: 'Category', alignRight: false }, 
  { id: 'price', label: 'Price', alignRight: false },
  { id: 'stock', label: 'Stock', alignRight: false },
  { id: 'viewCount', label: 'ViewCount', alignRight: false },
  // { id: 'isVerified', label: 'Verified', alignRight: false },
  // { id: 'status', label: 'Status', alignRight: false },
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
export default function Product() {
  // các state 
  const [idRemoveRow, setIdRemoveRow] = useState(false);
  const [USERLIST, setUSERLIST] = useState([]);
  const [page, setPage] = useState(0);
  const [order, setOrder] = useState('asc');
  const [selected, setSelected] = useState([]);
  const [orderBy, setOrderBy] = useState('name');
  const [filterName, setFilterName] = useState('');
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [count, setCount] = useState(0);
  const [categoryId, setCategoryId] = useState(0);

  // call api  
  useEffect(async () => {
    const response = await GetAllProductPaging(page + 1, rowsPerPage,"en-us",categoryId?categoryId:0); 
    if (response.items){
      setUSERLIST(response.items);
      setCount(response.totalRecords)
    }
    if (idRemoveRow) {
      setIdRemoveRow(false);
    }
  },[idRemoveRow,page, rowsPerPage,categoryId])

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

  // sắp xếp theo tên 
  const handleFilterByName = (event) => {
    setFilterName(event.target.value);
  };

  // thêm cột trống để căn chỉnh bảng
  const emptyRows = page > 0 ? Math.max(0, (1 + page) * rowsPerPage - count) : 0;

  // sắp xếp theo tên 
  const filteredUsers = applySortFilter(USERLIST, getComparator(order, orderBy), filterName);

  // không có dòng nào
  const isUserNotFound = filteredUsers.length === 0;

  return (
    <Page title="Product | Mystic Green">
      <Container>
        <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
          <Typography variant="h4" gutterBottom>
            Product
          </Typography>
          <Button
            variant="contained"
            component={RouterLink}
            to="/product/create"
            startIcon={<Iconify icon="eva:plus-fill" />}
          >
            New Product
          </Button>
        </Stack>

        <Card>
          <UserListToolbar
            selected = {selected}
            numSelected={selected.length}
            filterName={filterName}
            onFilterName={handleFilterByName}
            setSelected={setSelected}
            setIdRemoveRow={setIdRemoveRow} 
            type={'product'}
            setCategoryId = {setCategoryId}
          />

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
                      
                      const { id, 
                        imagePaths, 
                        name,
                        description,
                        price,
                        stock,
                        viewCount,
                        categoryName, } = row;
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
                          <TableCell component="th" scope="row" padding="none">
                            <Stack direction="row" alignItems="center" spacing={2}>
                              <Avatar alt={name} src={imagePaths[0]} sx={{borderRadius: 0}}/>
                              <Typography variant="subtitle2" noWrap>
                                {name}
                              </Typography>
                            </Stack>
                          </TableCell>

                          {/* cột company */}
                          <TableCell align="left">{description}</TableCell>

                          {/* cột role */}
                          <TableCell align="left">{categoryName}</TableCell>

                          {/* cột company */}
                          <TableCell align="left">{price}</TableCell>

                          {/* cột password */}
                          <TableCell align="left">{stock}</TableCell>
                        
                          {/* cột role */}
                          <TableCell align="left">{viewCount}</TableCell>

                          <TableCell align="right">
                            <UserMoreMenu id = {id} setIdRemoveRow={setIdRemoveRow} type="product" />
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
      </Container>
    </Page>
  );
}
