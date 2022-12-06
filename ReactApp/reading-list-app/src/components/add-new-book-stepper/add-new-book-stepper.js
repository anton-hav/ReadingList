import * as React from 'react';
import Box from '@mui/material/Box';
import Stepper from '@mui/material/Stepper';
import Step from '@mui/material/Step';
import StepLabel from '@mui/material/StepLabel';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';

import CategoryStep from '../category-step/category-step';
import AuthorStep from '../author-step/author-step';
import BookTitleStep from '../book-title-step/book-title-step';
import ReadingPriorityStep from '../reading-priority-step/reading-priority-step';
import ReadingStatusStep from '../reading-status-step/reading-status-step';
import BookSummaryStep from '../book-summary-step/book-summary-step';

import BookService from "../../services/books/book-service";



// export default function HorizontalLinearStepper() {
//   const [activeStep, setActiveStep] = React.useState(0);
//   const [skipped, setSkipped] = React.useState(new Set());

//   let stepComponent;

//   if (activeStep === 0){
//     stepComponent = <CategoryStep/>;
//   } else if (activeStep === 1) {
//     stepComponent = <AuthorStep/>;
//   } else {    
//   }

//   const handleNext = () => {
//     let newSkipped = skipped;

//     setActiveStep((prevActiveStep) => prevActiveStep + 1);
//     setSkipped(newSkipped);
//   };

//   const handleBack = () => {
//     setActiveStep((prevActiveStep) => prevActiveStep - 1);
//   };

//   const handleReset = () => {
//     setActiveStep(0);
//   };

//   return (
//     <Box sx={{ width: '100%' }}>
//       <Stepper activeStep={activeStep}>
//         {steps.map((label, index) => {
//           const stepProps = {};
//           const labelProps = {};

//           return (
//             <Step key={label} {...stepProps}>
//               <StepLabel {...labelProps}>{label}</StepLabel>
//             </Step>
//           );
//         })}
//       </Stepper>


//       {activeStep === steps.length ? (
//         <React.Fragment>
//           <Typography sx={{ mt: 2, mb: 1 }}>
//             All steps completed - you&apos;re finished
//           </Typography>
//           <Box sx={{ display: 'flex', flexDirection: 'row', pt: 2 }}>
//             <Box sx={{ flex: '1 1 auto' }} />
//             <Button onClick={handleReset}>Reset</Button>
//           </Box>
//         </React.Fragment>
//       ) : (
//         <React.Fragment>
//           <Typography component={'span'} variant={'body2'} sx={{ mt: 2, mb: 1 }}>
//           Step {activeStep + 1}
//           {stepComponent}
//           </Typography>
//           <Box sx={{ display: 'flex', flexDirection: 'row', pt: 2 }}>
//             <Button
//               color="inherit"
//               disabled={activeStep === 0}
//               onClick={handleBack}
//               sx={{ mr: 1 }}
//             >
//               Back
//             </Button>
//             <Box sx={{ flex: '1 1 auto' }} />
//             <Button onClick={handleNext}>
//               {activeStep === steps.length - 1 ? 'Finish' : 'Next'}
//             </Button>
//           </Box>
//         </React.Fragment>
//       )}
//     </Box>
//   );
// }




export default function HorizontalLinearStepper() {
  const [activeStep, setActiveStep] = React.useState(0);
  const [authorId, setAuthorId] = React.useState('');
  const [categoryId, setCategoryId] = React.useState('');
  const [title, setTitle] = React.useState('');
  const [priority, setPriority] = React.useState(2);
  const [status, setStatus] = React.useState(0);
  const [errorMessages, setErrorMessages] = React.useState(Array(3).fill(null));
  const [isNextAllowed, setIsNextAllowed] = React.useState(false);
  // const [skipped, setSkipped] = React.useState(new Set());

  const _bookService = new BookService();
  const steps = ['Category', 'Author', 'Title', 'Priority', 'Status', 'Summary'];

  const handleNext = () => {       
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
    console.log(`New author: ${authorId}`);
    console.log(`New cateory: ${categoryId}`);  
    console.log(`New title: ${title}`);  
    console.log(`New priority: ${priority}`);
    console.log(`New status: ${status}`);
    if (activeStep < steps.length - 2) {
      setIsNextAllowed(false);
    }        
  };

  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
    setIsNextAllowed(true); 
  };

  const handleReset = () => {
    setActiveStep(0);
  };

  const handleAddBook = async () => {
    let book = {
      'title': title, 
      'authorId': authorId, 
      'categoryId': categoryId,
    };

    let result = await _bookService.createNewBook(book);
    
    if (result) {
      setActiveStep((prevActiveStep) => prevActiveStep + 1);
    }
  }

  const handleAuthorSelect = (event) => {    
    setAuthorId(event.target.value);
    setIsNextAllowed(true);
  }

  const handleCategorySelect = (event) => {
    setCategoryId(event.target.value);
    setIsNextAllowed(true);
  }

  const checkBookForExisting = async (newTitle) => {    
    if (newTitle !== ''){
      const isExist = await _bookService.isBookExist(newTitle, authorId);
      if (isExist) {
        errorMessages[activeStep] = 'Book already exists';
        let error = errorMessages.slice();
        setErrorMessages(error);
      }
      else {
        errorMessages[activeStep] = null;
        let error = errorMessages.slice();
        setErrorMessages(error);
        setIsNextAllowed(true);
      }
    } else {
      errorMessages[activeStep] = 'Title is required';
      let error = errorMessages.slice();
      setErrorMessages(error);
    }    
  }

  const handleTitleChange = (event) => {    
    setTitle(event.target.value);        
    checkBookForExisting(event.target.value);
  }

  const handlePriorityChange = (event) => {
    setPriority(event.target.value);
    setIsNextAllowed(true);
  }

  const handleStatusChange = (event) => {
    setStatus(event.target.value);
    setIsNextAllowed(true);
  }

  const renderFinalStep = () => {
    return (
    <React.Fragment>
      <Typography sx={{ mt: 2, mb: 1 }}>
        All steps completed - you&apos;re add new book.
      </Typography>
      <Box sx={{ display: 'flex', flexDirection: 'row', pt: 2 }}>
        <Box sx={{ flex: '1 1 auto' }} />
        <Button onClick={handleReset}>Ok</Button>
      </Box>
    </React.Fragment>);
  }

  const renderStep = (stepComponent, onClickHandle) => {
    return (
      <React.Fragment>
          <Typography component={'span'} variant={'body2'} sx={{ mt: 2, mb: 1 }}>
          Step {activeStep + 1}
          {stepComponent}
          </Typography>
          <Box sx={{ display: 'flex', flexDirection: 'row', pt: 2 }}>
            <Button
              color="inherit"
              disabled={activeStep === 0}
              onClick={handleBack}
              sx={{ mr: 1 }}
            >
              Back
            </Button>
            <Box sx={{ flex: '1 1 auto' }} />
            <Button onClick={onClickHandle}
              disabled = {!isNextAllowed}
            >
              {activeStep === steps.length - 1 ? 'Add' : 'Next'}
            </Button>
          </Box>
        </React.Fragment>
    );
  }

  let stepComponent;

  if (activeStep === 0){
    stepComponent = <CategoryStep
      value = {categoryId} 
      onSelect={(event) => handleCategorySelect(event)}/>;
  } else if (activeStep === 1) {
    stepComponent = <AuthorStep 
      value = {authorId}
      onSelect={(event) => handleAuthorSelect(event)}/>;    
  } else if (activeStep === 2) {
    stepComponent = <BookTitleStep
      value = {title}
      onChange={(event) => handleTitleChange(event)}
      errorMessage = {errorMessages[activeStep]}/>;    
  } else if (activeStep === 3) {
    stepComponent = <ReadingPriorityStep
      value = {priority}
      onChange={(event) => handlePriorityChange(event)}
    />;    
  } else if (activeStep === 4) {
    stepComponent = <ReadingStatusStep
      value = {status}
      onChange={(event) => handleStatusChange(event)}
    />;    
  } else if (activeStep === 5) {
    let book = {
      'title': title, 
      'authorId': authorId, 
      'categoryId': categoryId, 
      'priority': priority, 
      'status': status
    };
    stepComponent = <BookSummaryStep
      value = {book}      
    />;    
  }else {    
  }

  let stepFragment;
  if (activeStep === steps.length){
    stepFragment = renderFinalStep();
  } else if (activeStep === steps.length - 1){
    stepFragment = renderStep(stepComponent, handleAddBook);
  } else {
    stepFragment = renderStep(stepComponent, handleNext);
  }


  return (
    <Box sx={{ width: '100%' }}>
      <Stepper activeStep={activeStep}>
        {steps.map((label, index) => {
          const stepProps = {};
          const labelProps = {};

          return (
            <Step key={label} {...stepProps}>
              <StepLabel {...labelProps}>{label}</StepLabel>
            </Step>
          );
        })}
      </Stepper>

      {stepFragment}      
    </Box>
  );


  // return (
  //   <Box sx={{ width: '100%' }}>
  //     <Stepper activeStep={activeStep}>
  //       {steps.map((label, index) => {
  //         const stepProps = {};
  //         const labelProps = {};

  //         return (
  //           <Step key={label} {...stepProps}>
  //             <StepLabel {...labelProps}>{label}</StepLabel>
  //           </Step>
  //         );
  //       })}
  //     </Stepper>


  //     {activeStep === steps.length ? (
  //       <React.Fragment>
  //         <Typography sx={{ mt: 2, mb: 1 }}>
  //           All steps completed - you&apos;re finished
  //         </Typography>
  //         <Box sx={{ display: 'flex', flexDirection: 'row', pt: 2 }}>
  //           <Box sx={{ flex: '1 1 auto' }} />
  //           <Button onClick={handleReset}>Reset</Button>
  //         </Box>
  //       </React.Fragment>
  //     ) : (
  //       <React.Fragment>
  //         <Typography component={'span'} variant={'body2'} sx={{ mt: 2, mb: 1 }}>
  //         Step {activeStep + 1}
  //         {stepComponent}
  //         </Typography>
  //         <Box sx={{ display: 'flex', flexDirection: 'row', pt: 2 }}>
  //           <Button
  //             color="inherit"
  //             disabled={activeStep === 0}
  //             onClick={handleBack}
  //             sx={{ mr: 1 }}
  //           >
  //             Back
  //           </Button>
  //           <Box sx={{ flex: '1 1 auto' }} />
  //           <Button onClick={handleNext}>
  //             {activeStep === steps.length - 1 ? 'Finish' : 'Next'}
  //           </Button>
  //         </Box>
  //       </React.Fragment>
  //     )}
  //   </Box>
  // );
}