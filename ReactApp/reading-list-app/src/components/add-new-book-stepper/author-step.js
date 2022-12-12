import React, { useEffect } from "react";
import Box from "@mui/material/Box";
import { Typography } from "@mui/material";
// Import custom components
import AutocompleteWithAdd from "../autocompleate-with-add/autocomplete-with-add";
// Import services
import AuthorService from "../../services/author-service";
// Import data transfer objects and utils
import AuthorDto from "../../dto/author-dto";

import "./stepper.css";

const _authorService = new AuthorService();

export default function AuthorStep(props) {
  const { authorId, onSelect } = props;
  const [authors, setAuthors] = React.useState([]);
  const [value, setValue] = React.useState(null);

  useEffect(() => {
    async function setDataToAuthors() {
      const data = await _authorService.getAllAuthorsFromApi();
      setAuthors(data);
    }

    /**
     * Sets author specified by authorId to state.
     * @param {string} authorId - author id from the parent component
     */
    async function setValueByAuthorId(authorId) {
      let author = await _authorService.getAuthorByIdFromApi(authorId);
      if (author instanceof AuthorDto) {
        setValue(author);
      }
    }

    if (authors.length === 0) {
      setDataToAuthors();
    }
    if (authorId !== undefined && value === null) {
      setValueByAuthorId(authorId);
    }
  });

  /**
   * Handeles autocomplete input field change event.
   * Checks if author not exists and creates new author in the storage.
   * Sets new author to value state.
   * @param {CategoryDto} newValue - an author that the user selected in the input field
   */
  const handleAutocompleteChange = async (newValue) => {
    if (newValue !== null) {
      if (newValue.id === undefined && newValue.fullName !== null) {
        let result = await createNewAuthor(newValue.fullName);
        newValue = result;
      }
      onSelect(newValue.id);
    } else {
      onSelect();
    }
    updateAuthors();
    setValue(newValue);
  };

  /**
   * Create new author in the storage.
   * @param {string} fullName - full name of the author to be created.
   * @returns a created author as a AuthorDto object
   */
  async function createNewAuthor(fullName) {
    let authorDto = new AuthorDto();
    authorDto.fullName = fullName;
    let result = await _authorService.createNewAuthor(authorDto);
    return result;
  }

  /**
   * Cleans authors state.
   */
  function updateAuthors() {
    // Needed for correctly updating
    setAuthors([]);
  }

  return (
    <Box sx={{ minWidth: 120, marginLeft: 5, marginRight: 5, paddingTop: 2 }}>
      <Typography className="step-description" variant="body2">
        Select the desired option from those available. For a quick search,
        start typing the category name. If the item you need is not on the list,
        add it by selecting <b>"Add..."</b>.
        <br />
        You will be able to go to the next step only after selecting one of the
        values.
        <br />
        Don't be afraid to make a mistake! You can always go back to this step.
      </Typography>
      <AutocompleteWithAdd
        model={new AuthorDto()}
        title="Author"
        items={authors}
        value={value}
        onChange={handleAutocompleteChange}
      />
    </Box>
  );
}
