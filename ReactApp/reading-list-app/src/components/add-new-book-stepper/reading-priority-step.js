import React from "react";
import Box from "@mui/material/Box";
import Rating from "@mui/material/Rating";
import Typography from "@mui/material/Typography";

import Priorities from "../../utils/priorities-list";

const _priorities = Priorities;

export default function ReadingPriorityStep(props) {
  return (
    <Box
      sx={{
        marginLeft: 5,
        marginRight: 5,
        paddingTop: 2,
      }}
    >
      <Typography component="legend">Reading priority</Typography>
      <Typography variant="body1">
        <b>{props.value === undefined ? "" : _priorities[props.value]}</b>
      </Typography>
      <Rating
        name="simple-controlled"
        value={props.value === undefined ? 0 : props.value}
        size="large"
        onChange={props.onChange}
      />
      <Typography  variant="body2">
        You will only be able to go to the next step after you have set the
        reading priority.
        <br />
        Do not be afraid to make a mistake! You can always go
        back to this step or change the value later.
      </Typography>
    </Box>
  );
}
