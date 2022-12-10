import React, { useEffect } from "react";
import Box from "@mui/material/Box";

import Typography from "@mui/material/Typography";
import Paper from "@mui/material/Paper";
import Rating from "@mui/material/Rating";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import IconButton from "@mui/material/IconButton";
import Button from "@mui/material/Button";
import { alpha } from "@mui/material/styles";
import Tooltip from "@mui/material/Tooltip";
import Toolbar from "@mui/material/Toolbar";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";

import BookNoteService from "../../services/book-notes-service";
import BookService from "../../services/book-service";

import BookNoteDto from "../../dto/book-note-dto";
import HumanReadableBookModel from "../../models/human-readable-book-model";

import Priorities from "../../utils/priorities-list";
import Statuses from "../../utils/statuses-list";

export default function EditBookForm(props) {
  const _bookNoteService = new BookNoteService();
  const _bookService = new BookService();

  const { bookId, onEditBookBackClick } = props;
  const [model, setModel] = React.useState({});

  useEffect(() => {
    async function setDataToBook() {
      let note = await _bookNoteService.getHumanReadableBookNoteByIdFromApi(
        bookId
      );
      setModel(note);
    }

    if (model.id !== bookId) {
      setDataToBook();
    }
  });

  /**
   * Handles change priority.
   * Sets new priority value to the book's priority state.
   * @param {Event} event - representing the React event
   */
  const handlePriorityChange = (event) => {
    console.log(event.target.value);
    let note = HumanReadableBookModel.clone(model);
    note.priority = Number(event.target.value);
    setModel(note);
  };

  /**
   * Handles click on save button.
   * Prepares and sends the modified book note to the storage to update the value.
   */
  const handleClickSave = async () => {
    let bookId = (await _bookService.getBookByBookNoteIdFromApi(model.id)).id;
    let note = BookNoteDto.fromHumanReadableBookModelAndBookId(model, bookId);
    let result = await _bookNoteService.updateBookNote(note);
  };

  return (
    <Box sx={{ width: "100%" }}>
      <Paper
        sx={{
          width: "100%",
          minHeight: props.tableHeight - 54 /*54 - offset chosen empirically*/,
          mb: 2,
        }}
      >
        <Toolbar
          sx={{
            pl: { sm: 2 },
            pr: { xs: 1, sm: 1 },
            marginBottom: 2,
            ...{
              bgcolor: (theme) => alpha(theme.palette.primary.main),
            },
          }}
        >
          <Typography
            sx={{ flex: "1 1 100%" }}
            variant="h6"
            id="tableTitle"
            component="div"
          >
            Edit book note
          </Typography>
          <Tooltip title="Back">
            <IconButton onClick={onEditBookBackClick}>
              <ArrowForwardIcon />
            </IconButton>
          </Tooltip>
        </Toolbar>

        <Card sx={{ minWidth: 275, margin: 2 }}>
          <CardContent>
            <Typography
              sx={{ fontSize: 14 }}
              color="text.secondary"
              gutterBottom
            >
              Book summary
            </Typography>
            <Typography variant="h5" component="div">
              {model.title}
            </Typography>
            <Typography sx={{ mb: 1.5 }} color="text.secondary">
              by {model.author}
            </Typography>
            <Typography variant="body2">
              <b>category:</b> {model.category}.
              <br />
              <br />
              <b>priority:</b> {Priorities[model.priority]}.
              <br />
              <b>status:</b> {Statuses[model.status]}.
            </Typography>
            <Rating
              name="simple-controlled"
              value={model.priority === undefined ? 0 : model.priority}
              size="large"
              onChange={(event) => handlePriorityChange(event)}
            />
          </CardContent>
        </Card>
        <Box sx={{ flex: "1 1 auto" }} />
        <Button onClick={handleClickSave}>Save</Button>
      </Paper>
    </Box>
  );
}
