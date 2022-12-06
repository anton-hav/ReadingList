import React from 'react';
import Box from '@mui/material/Box';
import Rating from '@mui/material/Rating';
import Typography from '@mui/material/Typography';

export default function ReadingPriorityStep(props) {  
    return (
      <Box
      >
        <Typography component="legend">Reading priority</Typography>
        <Rating
          name="simple-controlled"
          value={props.value === undefined ? 0 : props.value}
          size="large"
          onChange={props.onChange}
        />        
      </Box>
    );
  }