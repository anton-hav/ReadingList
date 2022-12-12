import React, { Component } from "react";
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import Cube from "./components/cube/cube";

import "./App.css";
import { Paper, Typography } from "@mui/material";

class App extends Component {
  render() {
    return (
      <div className="App">
        <header className="App-header">
          <Typography
            sx={{ padding: 2, align: "left" }}
            className="App-title"
            variant="h2"
          >
            Reading list
          </Typography>
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
                <Paper sx={{ maxWidth: 800, paddingX: 2, paddingY: 1 }}>
                  <Typography variant="h4">About app</Typography>
                  <Typography
                    className="app-description"
                    sx={{ align: "left" }}
                    variant="body1"
                  >
                    The app allows you to manage your list of books to read. You
                    can create new notes, edit existing notes, and delete notes
                    that are irrelevant to you.
                    <br />
                    The app supports reading priority and reading status
                    management. You can create and assign custom categories for
                    books. The app supports sorting by any criterion. It will
                    allow you to find the note you are interested in faster.
                  </Typography>
                </Paper>
              </Grid>
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
