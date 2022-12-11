// import React, {useEffect} from "react";
// import InputLabel from '@mui/material/InputLabel';
// import MenuItem from '@mui/material/MenuItem';
// import FormControl from '@mui/material/FormControl';
// import Select from '@mui/material/Select';
// import Box from '@mui/material/Box';
// import AuthorService from "../../services/author-service";


// const _authorService = new AuthorService();

// export default function AuthorStep(props) {    
//     const [items, setItems] = React.useState([]);    

//     useEffect(() => {
//         async function getItems() {   
//           const data = await _authorService.getAllAuthorsFromApi();
//           setItems(data);
//         };

//         if (items.length === 0) {
//           getItems();
//         }
//     });    
  
//     return (
//       <Box sx={{ minWidth: 120, marginLeft: 5, marginRight: 5, paddingTop: 2 }}>
//         <FormControl fullWidth>
//           <InputLabel id="demo-simple-select-label">Author</InputLabel>
//           <Select
//             labelId="demo-simple-select-label"
//             id="demo-simple-select"
//             value={items.length === 0 ? '' : props.value}
//             label="Author"
//             onChange={props.onSelect}
//           >
//           {items.map((item) => <MenuItem key={item.id.toString()} value={item.id}>{item.fullName}</MenuItem>)}           
//           </Select>
//         </FormControl>
//       </Box>
//     );
//   }