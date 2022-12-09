import * as React from "react";
import Box from "@mui/material/Box";

import Typography from "@mui/material/Typography";
import Paper from "@mui/material/Paper";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import IconButton from "@mui/material/IconButton";
import { alpha } from "@mui/material/styles";
import Tooltip from "@mui/material/Tooltip";
import Toolbar from "@mui/material/Toolbar";
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';

import Priorities from "../../utils/priorities-list";
import Statuses from "../../utils/statuses-list";

export default function EditBookForm(props) {
    const {
        bookId,
        onEditBookBackClick,
    } = props;
    let book = {};
    

  return (
    <Box sx={{ width: "100%" }}>
      <Paper
        sx={{ width: "100%", minHeight: props.tableHeight - 54 /*455*/, mb: 2 }}
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
              {book.title}
            </Typography>
            <Typography sx={{ mb: 1.5 }} color="text.secondary">
              by {book.author}
            </Typography>
            <Typography variant="body2">
              <b>category:</b> {book.category}.
              <br />
              <br />
              <b>priority:</b> {Priorities[book.priority]}.
              <br />
              <b>status:</b> {Statuses[book.status]}.
            </Typography>
          </CardContent>
        </Card>
      </Paper>
    </Box>
  );
}
