import React from "react";
import TextField from "@mui/material/TextField";
import { Typography } from "@mui/material";
import Box from "@mui/material/Box";

import "./stepper.css";

export default function BookTitleStep(props) {
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        marginLeft: 5,
        marginRight: 5,
        paddingTop: 2,
      }}
      component="form"
      noValidate
      autoComplete="off"
    >
      <Typography className="step-description" variant="body2">
        You can go to the next step only after you enter the title. Enter the
        title of the book and leave the input field.
        <br />
        <br />
        <b>Important!</b> You can't have a book without a title. If you try to
        enter the title of the already existing book for the selected author -
        you will get a notification with an error.
        <br />
        <br />
        Do not be afraid to make a mistake! You can always go back to this step.
      </Typography>
      <TextField
        id="outlined-basic"
        defaultValue={props.value === "" ? "" : props.value}
        text={props.value}
        onBlur={props.onChange}
        error={props.errorMessage !== null}
        helperText={props.errorMessage ? props.errorMessage : ""}
        label="Title"
        variant="outlined"
      />
    </Box>
  );
}
