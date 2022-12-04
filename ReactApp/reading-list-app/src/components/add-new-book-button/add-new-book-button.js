import React, {Component} from "react";
import Button from '@mui/material/Button';

import './add-new-book-button.css';

class AddNewBookButton extends Component {
  render(props) {
    return (
      <Button className="PrimaryButton" 
        variant="contained"
        onClick={this.props.onClick}
        >
        Add new book
      </Button>
    )
  }
}

export default AddNewBookButton;