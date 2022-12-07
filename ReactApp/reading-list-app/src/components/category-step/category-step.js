import React, {useEffect} from "react";
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import Box from '@mui/material/Box';
import CategoryService from "../../services/category-service";

const _categoryService = new CategoryService();

// class CategoryStep extends Component {

//   constructor(props) {
//     super(props);
//     this.state = {
//       category: '',
//       items: [] 
//     };

//     this._categoryService = new CategoryService();
//     (async () => this.setSelectItems())();
//   }

//   async setSelectItems(){
//     let categories = await this._categoryService.getAllCategoriesFromApi();
//     this.setState({items: categories.map((item) => <MenuItem key={item.id.toString()} value={item.id}>{item.name}</MenuItem>)})
//   }  
  
//   handleChange (sender, event) {
//     sender.setState({category: event.target.value});
//   }
  
//   render() {
//     return (
//       <FormControl fullWidth>
//         <InputLabel id="demo-simple-select-label">Category</InputLabel>
//         <Select
//           labelId="demo-simple-select-label"
//           id="demo-simple-select"
//           value={this.state.category}
//           label="Category"
//           onChange={(event) => this.handleChange(this, event)}
//         >
//           {this.state.items}
//         </Select>
//       </FormControl>
//     );
//   }  
// }


// export default CategoryStep;


export default function CategoryStep(props) {
  const [items, setItems] = React.useState([]);

  useEffect(() => {
      async function getItems() {   
        const data = await _categoryService.getAllCategoriesFromApi();
        setItems(data);
      };

      if (items.length === 0) {
        getItems();
      }
  });    

  return (
    <Box sx={{ minWidth: 120 }}>
      <FormControl fullWidth>
        <InputLabel id="demo-simple-select-label">Category</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={items.length === 0 ? '' : props.value}
          label="Category"
          onChange={props.onSelect}
        >
        {items.map((item) => <MenuItem key={item.id.toString()} value={item.id}>{item.name}</MenuItem>)}           
        </Select>
      </FormControl>
    </Box>
  );
}