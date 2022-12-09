import React, { useEffect } from "react";
import Box from "@mui/material/Box";

import EnhancedTable from "../book-table/book-table";
import HorizontalLinearStepper from "../add-new-book-stepper/add-new-book-stepper";
import EditBookForm from "../edit-book/edit-book";

import "./cube.scss";

import BookNoteService from "../../services/book-notes-service";
import PaginationParameters from "../../utils/paginationParameters";

let _bookNoteService = new BookNoteService();

export default function Cube(props) {
  const [currentClass, setCurrentClass] = React.useState("cube show-front");
  const [isAddBookFormActive, setIsAddBookFormActive] = React.useState(false);
  const [tableHeight, setTableHeight] = React.useState();
  const [isEditBookFormActive, setIsEditBookFormActive] = React.useState(false);

  useEffect(() => {
    // ++++++++++++++++++++++++++++++
    // FROM Book Table
    async function setDataToRows() {
      updateRowsCount();
      updateRowsData();
    }

    if (rows.length === 0) {
      setDataToRows();
    }

    // ++++++++++++++++++++++++++++++
  });

  // ++++++++++++++++++++++++++++++++
  // FROM Book Table
  const [order, setOrder] = React.useState("asc");
  const [orderBy, setOrderBy] = React.useState("author");
  const [selected, setSelected] = React.useState([]);
  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(5);
  const [rows, setRows] = React.useState([]);
  const [rowsCount, setRowsCount] = React.useState(0);

  /**
   * Sets the human readable book notes specified pagination parameters from the api to rows.
   */
  async function updateRowsData() {
    let parameters = new PaginationParameters(
      rowsPerPage,
      page,
      orderBy,
      order
    );
    const data = await _bookNoteService.getHumanReadableBooksFromApi(
      parameters
    );
    setRows(data);
  }

  /**
   * Sets the number of rows equal to the number of book notes in the storage.
   */
  const updateRowsCount = async () => {
    const count = await _bookNoteService.getBookNoteCountFromApi();
    setRowsCount(count);
  };

  /**
   * Sets the new value to order and orderBy states
   * and clean up table data to default.
   * @param {Event} event - representing the React event
   * @param {string} property - new order property value
   */
  const handleRequestSort = (event, property) => {
    const isAsc = orderBy === property && order === "asc";
    setOrder(isAsc ? "desc" : "asc");
    setOrderBy(property);
    setPage(0);
    updatePageData();
  };

  /**
   * Handles the click on select all button on table toolbar.
   * Gets ids of rows on the current page and sets it to the selected state.
   * If the selected contains some value, clear the selected.
   * @param {Event} event - representing the React event
   * @returns
   */
  const handleSelectAllClick = (event) => {
    if (event.target.checked) {
      const newSelected = rows.map((n) => n.id);
      setSelected(newSelected);
      return;
    }
    setSelected([]);
  };

  /**
   * Handles the click on table rows.
   * Gets the id of the selected row and set to selected state.
   * @param {*} event - representing the React event
   * @param {*} id - unique identifier of the selected row
   */
  const handleClick = (event, id) => {
    const selectedIndex = selected.indexOf(id);
    let newSelected = [];

    if (selectedIndex === -1) {
      newSelected = newSelected.concat(selected, id);
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

  /**
   * Sets the new value to page state and clean up rows state.
   * @param {*} event - representing the React event
   * @param {number} newPage - new page number
   */
  const handleChangePage = (event, newPage) => {
    setPage(newPage);
    updatePageData();
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
    updatePageData();
  };

  /**
   * Deletes the selected rows
   */
  const handleDeleteClick = async () => {
    console.log("Deleting selected rows");
    if (selected.length > 0) {
      let bookNoteIds = selected.slice();
      await _bookNoteService.deleteBookNotes(bookNoteIds);
    }
    changePageNumberAfterDeletion();
    updatePageData();
    setSelected([]);
  };

  /**
   * Changes the page number after deletion.
   */
  function changePageNumberAfterDeletion() {
    let count = rowsCount - selected.length;
    if (page > 0) {
      if (page * rowsPerPage >= count) {
        let newPage = page - 1;
        setPage(newPage);
      }
    }
  }

  /**
   * Resets the rows in the table.
   */
  function updatePageData() {
    // It is necessary to clear the rows
    // for the useEffect to work correctly.
    setRows([]);
  }

  /**
   * Handle change book table height.
   * Gets the height of the table and sets new value to the height state.
   * @param {number} height - new height value
   */
  const handleChangeHeight = (height) => {
    setTableHeight(height);
  };

  /**
   * Turns the cube to the left shows (creates) edit book form.
   */
  const handleEditBookClick = () => {
    setIsEditBookFormActive(true);
    changeCubeSide("left");
  };

  const handleEditBookBackClick = () => {
    changeCubeSide("front");
  };

  // ++++++++++++++++++++++++++++++++

  function changeCubeSide(value) {
    let showClass = "";
    if (value === "left") {
      showClass = "cube show-left";
    } else if (value === "right") {
      showClass = "cube show-right";
    } else {
      showClass = "cube show-front";
    }
    if (currentClass !== "cube") {
      setCurrentClass("cube");
    }
    setCurrentClass(showClass);
  }

  /**
   * Turns the cube to the front and collapses add book form
   */
  const handleCloseAddBookStepper = () => {
    changeCubeSide("front");
    updatePageData();
    // Adds a delay before deleting a form
    // for the duration of the cube rotation animation.
    setTimeout(() => {
      setIsAddBookFormActive(false);
    }, 1000);
  };

  /**
   * Turns the cube to the right shows (creates) add book form.
   */
  const handleAddBookClick = () => {
    setIsAddBookFormActive(true);
    changeCubeSide("right");
  };

  /**
   * Turns the cube to the front and hiddes (not collapses) add book form.
   */
  const handleAddBookBackClick = () => {
    changeCubeSide("front");
  };

  return (
    <div>
      <Box className="scene">
        <Box className={currentClass}>
          <Box className="cube__face cube__face--front">
            <EnhancedTable
              onAddBookClick={handleAddBookClick}
              page={page}
              rowsPerPage={rowsPerPage}
              rows={rows}
              rowsCount={rowsCount}
              selected={selected}
              order={order}
              orderBy={orderBy}
              onDeleteClick={handleDeleteClick}
              onSelectAllClick={handleSelectAllClick}
              onRequestSort={handleRequestSort}
              onSelectClick={handleClick}
              onPageChange={handleChangePage}
              onRowsPerPageChange={handleChangeRowsPerPage}
              onChangeTableSize={(height) => handleChangeHeight(height)}
              onEditBookClick={handleEditBookClick}
            />
          </Box>
          <Box className="cube__face cube__face--left">
            {isEditBookFormActive ? (
              <EditBookForm
                bookId={selected[0]}
                tableHeight={tableHeight}
                onEditBookBackClick={handleEditBookBackClick}
              />
            ) : null}
          </Box>
          <Box className="cube__face cube__face--right">
            {isAddBookFormActive ? (
              <HorizontalLinearStepper
                onAddBookBackClick={handleAddBookBackClick}
                onAddBookClick={handleCloseAddBookStepper}
                tableHeight={tableHeight}
              />
            ) : null}
          </Box>
        </Box>
      </Box>
    </div>
  );
}
