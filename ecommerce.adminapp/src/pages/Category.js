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
import GetAllPaging from 'src/api/category/GetAllPaging';

// ----------------------------------------------------------------------
// call api

// cài đặt các trường cho bảng
const TABLE_HEAD = [
  { id: 'name', label: 'Name', alignRight: false },
  { id: 'seoTitle', label: 'Title', alignRight: false },
  { id: 'seoAlias', label: 'Alias', alignRight: false },
  { id: 'seoDescription', label: 'Description', alignRight: false },
  { id: 'language', label: 'Language', alignRight: false },
  { id: 'isShowOnHome', label: 'Show On Home', alignRight: false },
  { id: 'status', label: 'Status', alignRight: false },
  { id: '' }
];

// sắp xếp
// ----------------------------------------------------------------------

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
export default function Category() {
  // các state
  const [ELEMENT_LIST, setELEMENT_LIST] = useState([]);
  const [page, setPage] = useState(0);
  const [order, setOrder] = useState('asc');
  const [selected, setSelected] = useState([]);
  const [orderBy, setOrderBy] = useState('name');
  const [filterName, setFilterName] = useState('');
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [count, setCount] = useState(0);
  const [idRemoveRow, setIdRemoveRow] = useState(false);

  // call api
  useEffect(async () => {
    const response = await GetAllPaging(page + 1, rowsPerPage, 'en-us');
    if (response.items) {
      setELEMENT_LIST(response.items);
      setCount(response.totalRecords);
    }
    if (idRemoveRow) {
      setIdRemoveRow(false);
    }
  }, [idRemoveRow, page, rowsPerPage]);

  // xử lí sắp xếp tăng giảm
  const handleRequestSort = (event, property) => {
    const isAsc = orderBy === property && order === 'asc';
    setOrder(isAsc ? 'desc' : 'asc');
    setOrderBy(property);
  };

  // chọn tất cả checkbox
  const handleSelectAllClick = (event) => {
    if (event.target.checked) {
      const newSelecteds = ELEMENT_LIST.map((n) => n.id);
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
  const filteredUsers = applySortFilter(ELEMENT_LIST, getComparator(order, orderBy), filterName);

  // không có dòng nào
  const isUserNotFound = filteredUsers.length === 0;

  return (
    <Page title="Category | Mystic Green">
      <Container>
        <Stack direction="row" alignItems="center" justifyContent="space-between" mb={5}>
          <Typography variant="h4" gutterBottom>
            Category
          </Typography>
          <Button
            variant="contained"
            component={RouterLink}
            to="/category/create"
            startIcon={<Iconify icon="eva:plus-fill" />}
          >
            New Category
          </Button>
        </Stack>

        <Card>
          <UserListToolbar
            selected={selected}
            numSelected={selected.length}
            filterName={filterName}
            onFilterName={handleFilterByName}
            setSelected={setSelected}
            setIdRemoveRow={setIdRemoveRow}
            type = {'category'}
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
                      const {
                        id,
                        name,
                        seoTitle,
                        seoAlias,
                        seoDescription,
                        language,
                        isShowOnHome,
                        status
                      } = row;
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

                          {/* cột name */}
                          <TableCell align="left">{name}</TableCell>

                          {/* cột title */}
                          <TableCell align="left">{seoTitle}</TableCell>

                          {/* cột description */}
                          <TableCell align="left">{seoAlias}</TableCell>

                          {/* cột description */}
                          <TableCell align="left">{seoDescription}</TableCell>

                          {/* cột language */}
                          <TableCell align="left">{language}</TableCell>

                          {/* show on home */}
                          <TableCell align="left">{isShowOnHome ? 'Yes' : 'No'}</TableCell>

                          {/* status */}
                          <TableCell align="left">
                            <Label variant="ghost" color={(status === 0 && 'error') || 'success'}>
                              {/* {sentenceCase(status)} */}
                              {status === 0 ? 'INACTIVE' : 'ACTIVE'}
                            </Label>
                          </TableCell>

                          <TableCell align="right">
                            <UserMoreMenu id={id} setIdRemoveRow={setIdRemoveRow} type="category"/>
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
