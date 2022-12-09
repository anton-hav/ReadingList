import React, { useEffect, useRef} from "react";
import Box from "@mui/material/Box";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TablePagination from "@mui/material/TablePagination";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import Checkbox from "@mui/material/Checkbox";
import FormControlLabel from "@mui/material/FormControlLabel";
import Switch from "@mui/material/Switch";

import { EnhancedTableHead } from "./book-table-head";
import { EnhancedTableToolbar } from "./book-table-toolbar";

import BookNoteService from "../../services/book-notes-service";

import Priorities from "../../utils/priorities-list";
import Statuses from "../../utils/statuses-list";
import PaginationParameters from "../../utils/paginationParameters";

let _bookNoteService = new BookNoteService();

// export default function EnhancedTable(props) {
//   const [order, setOrder] = React.useState("asc"); // moved to cube
//   const [orderBy, setOrderBy] = React.useState("author"); // moved to cube
//   const [selected, setSelected] = React.useState([]); // moved to cube
//   const [page, setPage] = React.useState(0); // moved to cube
//   const [dense, setDense] = React.useState(false);
//   const [rowsPerPage, setRowsPerPage] = React.useState(5); // moved to cube
//   const [rows, setRows] = React.useState([]); // moved to cube
//   const [rowsCount, setRowsCount] = React.useState(0); // moved to cube

//   useEffect(() => {
//     async function setDataToRows() {
//       updateRowsCount();
//       updateRowsData();
//     }

//     if (rows.length === 0) {
//       setDataToRows();
//     }
//   });

//   // moved to cube
//   /**
//    * Sets the human readable book notes specified pagination parameters from the api to rows.
//    */
//   async function updateRowsData() {
//     let parameters = new PaginationParameters(
//       rowsPerPage,
//       page,
//       orderBy,
//       order
//     );
//     const data = await _bookNoteService.getHumanReadableBooksFromApi(
//       parameters
//     );
//     setRows(data);
//   }

//   // moved to cube
//   /**
//    * Sets the number of rows equal to the number of book notes in the storage.
//    */
//   const updateRowsCount = async () => {
//     const count = await _bookNoteService.getBookNoteCountFromApi();
//     setRowsCount(count);
//   };

//   // moved to cube
//   /**
//    * Sets the new value to order and orderBy states
//    * and clean up table data to default.
//    * @param {*} event - representing the React event
//    * @param {string} property - new order property value
//    */
//   const handleRequestSort = (event, property) => {
//     const isAsc = orderBy === property && order === "asc";
//     setOrder(isAsc ? "desc" : "asc");
//     setOrderBy(property);
//     setPage(0);
//     updatePageData();
//   };

//   // moved to cube
//   const handleSelectAllClick = (event) => {
//     if (event.target.checked) {
//       const newSelected = rows.map((n) => n.id);
//       setSelected(newSelected);
//       return;
//     }
//     setSelected([]);
//   };

//   // moved to cube
//   const handleClick = (event, id) => {
//     const selectedIndex = selected.indexOf(id);
//     let newSelected = [];

//     if (selectedIndex === -1) {
//       newSelected = newSelected.concat(selected, id);
//     } else if (selectedIndex === 0) {
//       newSelected = newSelected.concat(selected.slice(1));
//     } else if (selectedIndex === selected.length - 1) {
//       newSelected = newSelected.concat(selected.slice(0, -1));
//     } else if (selectedIndex > 0) {
//       newSelected = newSelected.concat(
//         selected.slice(0, selectedIndex),
//         selected.slice(selectedIndex + 1)
//       );
//     }

//     setSelected(newSelected);
//   };

//   // moved to cube
//   /**
//    * Sets the new value to page state and clean up rows state.
//    * @param {*} event - representing the React event
//    * @param {number} newPage - new page number
//    */
//   const handleChangePage = (event, newPage) => {
//     setPage(newPage);
//     updatePageData();
//   };

//   // moved to cube
//   const handleChangeRowsPerPage = (event) => {
//     setRowsPerPage(parseInt(event.target.value, 10));
//     setPage(0);    
//     updatePageData();
//   };

//   //moved to cube
//   /**
//    * Deletes the selected rows
//    */
//   const handleDeleteClick = async () => {
//     console.log("Deleting selected rows");
//     if (selected.length > 0) {
//       let bookNoteIds = selected.slice();
//       await _bookNoteService.deleteBookNotes(bookNoteIds);
//     }
//     changePageNumberAfterDeletion();
//     updatePageData();
//     setSelected([]);
//   };

//   // moved to cube
//   /**
//    * Changes the page number after deletion.
//    */
//   function changePageNumberAfterDeletion() {
//     let count = rowsCount - selected.length;
//     if (page > 0) {
//       if ((page) * rowsPerPage >= count) {
//         let newPage = page - 1;
//         setPage(newPage);
//       }
//     }    
//   }

//   // moved to cube
//   /**
//    * Resets the rows in the table.
//    */
//   function updatePageData() {
//     // It is necessary to clear the rows
//     // for the useEffect to work correctly.
//     setRows([]);
//   }

//   const handleChangeDense = (event) => {
//     setDense(event.target.checked);
//   };

//   const isSelected = (id) => selected.indexOf(id) !== -1;

//   // Avoid a layout jump when reaching the last page with empty rows.
//   const emptyRows =
//     page > 0 ? Math.max(0, (1 + page) * rowsPerPage - rowsCount) : 0;

//   return (
//     <Box sx={{ width: "100%" }} /*ref={ref}*/>
//       <Paper sx={{ width: "100%", minHeight: 455, mb: 2 }}>
//         <EnhancedTableToolbar
//           numSelected={selected.length}
//           onAddBookClick={props.onAddBookClick}
//           onDeleteClick={handleDeleteClick}
//         />
//         <TableContainer>
//           <Table
//             sx={{ minWidth: 750 }}
//             aria-labelledby="tableTitle"
//             size={dense ? "small" : "medium"}
//           >
//             <EnhancedTableHead
//               numSelected={selected.length}
//               order={order}
//               orderBy={orderBy}
//               onSelectAllClick={handleSelectAllClick}
//               onRequestSort={handleRequestSort}
//               rowCount={rows.length}
//             />
//             <TableBody>

//               {
//                 rows.map((row, index) => {
//                   const isItemSelected = isSelected(row.id);
//                   const labelId = `enhanced-table-checkbox-${index}`;

//                   return (
//                     <TableRow
//                       hover
//                       onClick={(event) => handleClick(event, row.id)}
//                       role="checkbox"
//                       aria-checked={isItemSelected}
//                       tabIndex={-1}
//                       key={row.id}
//                       selected={isItemSelected}
//                     >
//                       <TableCell padding="checkbox">
//                         <Checkbox
//                           color="primary"
//                           checked={isItemSelected}
//                           inputProps={{
//                             "aria-labelledby": labelId,
//                           }}
//                         />
//                       </TableCell>
//                       <TableCell
//                         component="th"
//                         id={labelId}
//                         scope="row"
//                         padding="none"
//                       >
//                         {row.title}
//                       </TableCell>
//                       <TableCell align="right">{row.author}</TableCell>
//                       <TableCell align="right">{row.category}</TableCell>
//                       <TableCell align="right">
//                         {Priorities[row.priority]}
//                       </TableCell>
//                       <TableCell align="right">
//                         {Statuses[row.status]}
//                       </TableCell>
//                     </TableRow>
//                   );
//                 })
//               }
//               {emptyRows > 0 && (
//                 <TableRow
//                   style={{
//                     height: (dense ? 33 : 53) * emptyRows,
//                   }}
//                 >
//                   <TableCell colSpan={6} />
//                 </TableRow>
//               )}
//             </TableBody>
//           </Table>
//         </TableContainer>
//         <TablePagination
//           rowsPerPageOptions={[5, 10, 25]}
//           component="div"
//           count={rowsCount}
//           rowsPerPage={rowsPerPage}
//           page={page}
//           onPageChange={handleChangePage}
//           onRowsPerPageChange={handleChangeRowsPerPage}
//         />
//       </Paper>
//       <FormControlLabel
//         control={<Switch checked={dense} onChange={handleChangeDense} />}
//         label="Dense padding"
//       />
//     </Box>
//   );
// }


export default function EnhancedTable(props) {

  const {onAddBookClick, 
    page, 
    rowsPerPage,
    rows,
    rowsCount, 
    selected, 
    order, 
    orderBy, 
    onDeleteClick, 
    onSelectAllClick, 
    onRequestSort, 
    onSelectClick, 
    onPageChange, 
    onRowsPerPageChange,
    onChangeTableSize   
  } = props;

  useEffect(() => {
    let height = ref.current.clientHeight;
    //console.log(height);
    onChangeTableSize(height);
  });

  const ref = useRef(null);

  const [dense, setDense] = React.useState(false);

  const handleChangeDense = (event) => {
    setDense(event.target.checked);
  };

  const isSelected = (id) => selected.indexOf(id) !== -1;

  // Avoid a layout jump when reaching the last page with empty rows.
  const emptyRows =
    page > 0 ? Math.max(0, (1 + page) * rowsPerPage - rowsCount) : 0;

  return (
    <Box sx={{ width: "100%" }} ref={ref}>
      <Paper sx={{ width: "100%", minHeight: 455, mb: 2 }}>
        <EnhancedTableToolbar
          numSelected={selected.length}
          onAddBookClick={onAddBookClick}
          onDeleteClick={onDeleteClick}
        />
        <TableContainer>
          <Table
            sx={{ minWidth: 750 }}
            aria-labelledby="tableTitle"
            size={dense ? "small" : "medium"}
          >
            <EnhancedTableHead
              numSelected={selected.length}
              order={order}
              orderBy={orderBy}
              onSelectAllClick={onSelectAllClick}
              onRequestSort={onRequestSort}
              rowCount={rows.length}
            />
            <TableBody>

              {
                rows.map((row, index) => {
                  const isItemSelected = isSelected(row.id);
                  const labelId = `enhanced-table-checkbox-${index}`;

                  return (
                    <TableRow
                      hover
                      onClick={(event) => props.onSelectClick(event, row.id)}
                      role="checkbox"
                      aria-checked={isItemSelected}
                      tabIndex={-1}
                      key={row.id}
                      selected={isItemSelected}
                    >
                      <TableCell padding="checkbox">
                        <Checkbox
                          color="primary"
                          checked={isItemSelected}
                          inputProps={{
                            "aria-labelledby": labelId,
                          }}
                        />
                      </TableCell>
                      <TableCell
                        component="th"
                        id={labelId}
                        scope="row"
                        padding="none"
                      >
                        {row.title}
                      </TableCell>
                      <TableCell align="right">{row.author}</TableCell>
                      <TableCell align="right">{row.category}</TableCell>
                      <TableCell align="right">
                        {Priorities[row.priority]}
                      </TableCell>
                      <TableCell align="right">
                        {Statuses[row.status]}
                      </TableCell>
                    </TableRow>
                  );
                })
              }
              {emptyRows > 0 && (
                <TableRow
                  style={{
                    height: (dense ? 33 : 53) * emptyRows,
                  }}
                >
                  <TableCell colSpan={6} />
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
        <TablePagination
          rowsPerPageOptions={[5, 10, 25]}
          component="div"
          count={props.rowsCount}
          rowsPerPage={props.rowsPerPage}
          page={props.page}
          onPageChange={props.onPageChange}
          onRowsPerPageChange={props.onRowsPerPageChange}
        />
      </Paper>
      <FormControlLabel
        control={<Switch checked={dense} onChange={handleChangeDense} />}
        label="Dense padding"
      />
    </Box>
  );
}