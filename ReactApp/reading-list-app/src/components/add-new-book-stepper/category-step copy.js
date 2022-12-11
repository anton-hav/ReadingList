// import React, {useEffect} from "react";
// import InputLabel from '@mui/material/InputLabel';
// import MenuItem from '@mui/material/MenuItem';
// import FormControl from '@mui/material/FormControl';
// import Select from '@mui/material/Select';
// import Box from '@mui/material/Box';
// import CategoryService from "../../services/category-service";

// const _categoryService = new CategoryService();

// export default function CategoryStep(props) {
//   const [items, setItems] = React.useState([]);

//   useEffect(() => {
//       async function getItems() {   
//         const data = await _categoryService.getAllCategoriesFromApi();
//         setItems(data);
//       };

//       if (items.length === 0) {
//         getItems();
//       }
//   });    

//   return (
//     <Box sx={{ minWidth: 120, marginLeft: 5, marginRight: 5, paddingTop: 2 }}>
//       <FormControl fullWidth>
//         <InputLabel id="demo-simple-select-label">Category</InputLabel>
//         <Select
//           labelId="demo-simple-select-label"
//           id="demo-simple-select"
//           value={items.length === 0 ? '' : props.value}
//           label="Category"
//           onChange={props.onSelect}
//         >
//         {items.map((item) => <MenuItem key={item.id.toString()} value={item.id}>{item.name}</MenuItem>)}           
//         </Select>
//       </FormControl>
//     </Box>
//   );
// }