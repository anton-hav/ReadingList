import React, { Component } from 'react'
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';

import AddNewBookButton from './components/add-new-book-button/add-new-book-button'
import HorizontalLinearStepper from './components/add-new-book-stepper/add-new-book-stepper'

import './App.css';

// function App() {
//   return (
//     <div className="App">
//       <header className="App-header">
//         <img src={logo} className="App-logo" alt="logo" />
//         <p>
//           Edit <code>src/App.js</code> and save to reload.
//         </p>
//         <a
//           className="App-link"
//           href="https://reactjs.org"
//           target="_blank"
//           rel="noopener noreferrer"
//         >
//           Learn React
//         </a>
//       </header>
//     </div>
//   );
// }

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isAddBookFormActive: false,
    }

    this.handleActivateAddBookForm = this._handleActivateAddBookForm.bind(this);
  }

  render() {
    return (
        <div className="App">

            <header className="App-header">
                <h1 className="App-title">Reading list</h1>
            </header>

            <main className="App-main">
            <Container maxWidth="sm">
                <Grid container spacing={2}>
                    <Grid item xs={8}>                
                        <AddNewBookButton onClick={this.handleActivateAddBookForm}/>                             
                    </Grid>
                    <Grid item xs={12}>                
                        <HorizontalLinearStepper/>                             
                    </Grid>
                </Grid>
            </Container>                 
            </main>
        </div>
    )
  }

  _handleActivateAddBookForm() {
    this.setState({isAdditionFormActive: !this.state.isAddBookFormActive})
  }
}

export default App;
