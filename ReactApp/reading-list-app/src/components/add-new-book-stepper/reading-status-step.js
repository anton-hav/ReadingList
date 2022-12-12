import * as React from 'react';
import Box from '@mui/material/Box';
import Radio from '@mui/material/Radio';
import RadioGroup from '@mui/material/RadioGroup';
import FormControlLabel from '@mui/material/FormControlLabel';
import FormControl from '@mui/material/FormControl';
import FormLabel from '@mui/material/FormLabel';
import Typography from "@mui/material/Typography";

export default function ReadingStatusStep(props) {
  return (
    <Box
      >
    <FormControl>
      <FormLabel id="demo-row-radio-buttons-group-label">Reading status</FormLabel>
      <RadioGroup
        row
        value={props.value !== undefined ? props.value : ""}
        onChange={props.onChange}
        aria-labelledby="demo-row-radio-buttons-group-label"
        name="row-radio-buttons-group"
      >
        <FormControlLabel value="0" control={<Radio />} label="Scheduled" />
        <FormControlLabel value="1" control={<Radio />} label="In progress" />
        <FormControlLabel value="2" control={<Radio />} label="Completed" />
        <FormControlLabel value="3" control={<Radio />} label="Skipped" />
      </RadioGroup>
    </FormControl>
    <Typography  variant="body2">
        You will only be able to go to the next step after you have set the
        reading status.
        <br />
        Do not be afraid to make a mistake! You can always go
        back to this step or change the value later.
      </Typography>
    </Box>
  );
}