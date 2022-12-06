import React from "react";
import TextField from "@mui/material/TextField";

import Box from '@mui/material/Box';

export default function BookTitleStep(props) {

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',}}
      component="form"
      
      noValidate
      autoComplete="off"
    >
      <TextField         
        id="outlined-basic"
        defaultValue ={props.value === '' ? '' : props.value}
        text={props.value}        
        onBlur={props.onChange}
        error = {props.errorMessage !== null}
        helperText = {props.errorMessage ? props.errorMessage : ''}
        label="Title" 
        variant="outlined" />
    </Box>
  );
}