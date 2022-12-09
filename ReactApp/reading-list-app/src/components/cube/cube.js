import React, { useEffect } from "react";
import Box from "@mui/material/Box";

import EnhancedTable from "../book-table/book-table";
import HorizontalLinearStepper from '../add-new-book-stepper/add-new-book-stepper'

import "./cube.scss";

export default function Cube(props) {
  const [currentClass, setCurrentClass] = React.useState("cube show-front");
  const [isAddBookFormActive, setIsAddBookFormActive] = React.useState(false);
  //const [height, setHeight] = React.useState("800px");
  //const [width, setWidth] = React.useState("800px");

  useEffect(() => {
    //const root = document.documentElement;
    //root?.style.setProperty("cube--width", height);
    //root?.style.setProperty("cube--height", width);
  });

  function changeCubeSide(value) {
    let showClass = "";
    if (value === "left") {
      showClass = "cube show-left";
    } else if (value === "right") {
      showClass = "cube show-right";
    } else {
      showClass = "cube show-front";
    }
    if (currentClass !== "cube") {
      setCurrentClass("cube");
    }
    setCurrentClass(showClass);
  }

  /**
   * Turns the cube to the front and collapses add book form
   */
  const handleCloseAddBookStepper = () => {
    changeCubeSide("front");
    // Adds a delay before deleting a form 
    // for the duration of the cube rotation animation.
    setTimeout(() => {setIsAddBookFormActive(false);}, 1000);
  }

  /**
   * Turns the cube to the right add shows (creates) book form.
   */
  const handleAddBookClick = () => {
    setIsAddBookFormActive(true);
    changeCubeSide("right");
  }

  /**
   * Turns the cube to the front and hiddes (not collapses) add book form.
   */
  const handleAddBookBackClick = () => {
    changeCubeSide("front");
  }

  return (
    <div>
      <Box
        className="scene"
        // sx={{
        //   width: width,
        //   height: height,
        // }}
      >
        <Box
          className={currentClass}
        //   sx={{
        //     width: width,
        //     height: height,
        //   }}
        >
          <Box className="cube__face cube__face--front">
            <EnhancedTable onAddBookClick = {handleAddBookClick}/>            
          </Box>
          <Box className="cube__face cube__face--left">left</Box>
          <Box className="cube__face cube__face--right">
          {isAddBookFormActive 
          ? <HorizontalLinearStepper 
          onAddBookBackClick = {handleAddBookBackClick}
          onAddBookClick = {handleCloseAddBookStepper}
          />
          : null}
          </Box>
        </Box>
      </Box>
    </div>
  );
}
