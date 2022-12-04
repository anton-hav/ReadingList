import React, {Component} from "react";
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import CategoryService from "../../services/categories/category-service";



// export default function CategoryStep() {
//   const [age, setAge] = useState('');

//   const handleChange = (event) => {
//     setAge(event.target.value);
//   };

//   const items = [
//     <MenuItem value={10}>Ten</MenuItem>,
//     <MenuItem value={20}>Twenty</MenuItem>,
//     <MenuItem value={30}>Thirty</MenuItem>,
//     <MenuItem value={40}>Forty</MenuItem>
//   ]
  
//   return (
//     <FormControl fullWidth>
//       <InputLabel id="demo-simple-select-label">Age</InputLabel>
//       <Select
//         labelId="demo-simple-select-label"
//         id="demo-simple-select"
//         value={age}
//         label="Category"
//         onChange={handleChange}
//       >
//         {items}
//       </Select>
//     </FormControl>
//   );
  
// }

class CategoryStep extends Component {

  constructor(props) {
    super(props);
    this.state = {
      category: '',
      items: [] 
    };

    this._categoryService = new CategoryService();
    (async () => this.setSelectItems())();
  }

  async setSelectItems(){
    let categories = await this._categoryService.getAllCategoriesFromApi();
    this.setState({items: categories.map((item) => <MenuItem key={item.id.toString()} value={item.id}>{item.name}</MenuItem>)})
  }  
  
  handleChange (sender, event) {
    sender.setState({category: event.target.value});
  }
  
  render() {
    return (
      <FormControl fullWidth>
        <InputLabel id="demo-simple-select-label">Category</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={this.state.category}
          label="Category"
          onChange={(event) => this.handleChange(this, event)}
        >
          {this.state.items}
        </Select>
      </FormControl>
    );
  }  
}


export default CategoryStep;