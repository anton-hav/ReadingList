import React, { Component} from "react";
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import Cube from "./components/cube/cube";

import "./App.css";

class App extends Component {
  render() {
    return (
      <div className="App">
        <header className="App-header">
          <h1 className="App-title">Reading list</h1>
        </header>

        <main className="app-main">
          <Container maxWidth="m">
            <Grid
              container
              direction="column"
              justifyContent="center"
              alignItems="center"
              pacing={2}
              className="app-grid"
            >
              <Grid item xs={10} className="grid-item-book-table">
                <Cube />
              </Grid>
            </Grid>
          </Container>
        </main>
      </div>
    );
  }
}

export default App;
