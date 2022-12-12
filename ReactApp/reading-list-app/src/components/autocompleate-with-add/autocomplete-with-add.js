import React, { useEffect } from "react";
import TextField from "@mui/material/TextField";
import Autocomplete, { createFilterOptions } from "@mui/material/Autocomplete";

const filter = createFilterOptions();

export default function AutocompleteWithAdd(props) {
  // Model props is needed to correctly handle property
  // names such as name, title, full name, etc.
  const { model, title, items, value, onChange } = props;
  const [keyName, setKeyName] = React.useState();

  useEffect(() => {
    if (keyName === undefined) {
      let keyNames = Object.keys(model);
      // Sets to the keyName state variable 
      // the name of the next property after Id of the passed model.
      setKeyName(keyNames[1]);
    }
  }, [keyName, model]);

  return (
    <Autocomplete
      fullWidth={true}
      value={value}
      onChange={(event, newValue) => {
        if (typeof newValue === "string") {
          //onChange({ keyName: newValue });
          onChange(new model.constructor(undefined, newValue));
        } else if (newValue && newValue.inputValue) {
          //onChange({ keyName: newValue.inputValue });
          onChange(new model.constructor(undefined, newValue.inputValue));
        } else {
          onChange(newValue);          
        }
      }}
      filterOptions={(options, params) => {
        const filtered = filter(options, params);

        const { inputValue } = params;
        // Suggest the creation of a new value
        const isExisting = options.some(
          (option) => inputValue === option[keyName]
        );
        if (inputValue !== "" && !isExisting) {
          let filterItem = {};
          filterItem["inputValue"] = inputValue;
          filterItem[keyName] = `Add "${inputValue}"`;
          filtered.push(filterItem);
        }

        return filtered;
      }}
      selectOnFocus
      clearOnBlur
      handleHomeEndKeys
      id="free-solo-with-text-demo"
      options={items}
      getOptionLabel={(option) => {
        // Value selected with enter, right from the input
        if (typeof option[keyName] === "string") {
          return option[keyName];
        }
        // Add "xxx" option created dynamically
        if (option.inputValue) {
          return option.inputValue;
        }
        // Regular option
        if (option !== undefined) {
          return "";
        }
        return option[keyName];
      }}
      renderOption={(props, option) => <li {...props}>{option[keyName]}</li>}
      freeSolo
      renderInput={(params) => (
        <TextField {...params} label={title} sx={{ width: "100%" }} />
      )}
    />
  );
}
