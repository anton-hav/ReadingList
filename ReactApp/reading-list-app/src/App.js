import React, { Component } from 'react'
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';

import AddNewBookButton from './components/add-new-book-button/add-new-book-button'
import HorizontalLinearStepper from './components/add-new-book-stepper/add-new-book-stepper'
import EnhancedTable from './components/book-table/book-table'

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
    //this.handleCloseAddBookStepper = this._handleCloseAddBookStepper.bind(this);
  }

  handleCloseAddBookStepper = () => {
    this.setState({isAddBookFormActive: false});
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
                    {this.state.isAddBookFormActive ? null : <AddNewBookButton onClick={this.handleActivateAddBookForm}/>}            
                                                     
                    </Grid>
                    <Grid item xs={12}>
                    {this.state.isAddBookFormActive ? <HorizontalLinearStepper onClick={this.handleCloseAddBookStepper}/> : <div></div>}                
                                                     
                    </Grid>
                    <Grid item xs={12}>                                  
                      <EnhancedTable/>                               
                    </Grid>
                </Grid>
            </Container>                 
            </main>
        </div>
    )
  }

  _handleActivateAddBookForm() {
    this.setState({isAddBookFormActive: !this.state.isAddBookFormActive})
  }




}

export default App;
