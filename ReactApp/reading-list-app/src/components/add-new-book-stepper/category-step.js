import React, { useEffect } from "react";
import Box from "@mui/material/Box";
// Import custom components
import AutocompleteWithAdd from "../autocompleate-with-add/autocomplete-with-add";
// Import services
import CategoryService from "../../services/category-service";
// Import data transfer objects and utils
import CategoryDto from "../../dto/category-dto";

const _categoryService = new CategoryService();

export default function CategoryStep(props) {
  const { categoryId, onSelect } = props;
  const [categories, setCategories] = React.useState([]);
  const [value, setValue] = React.useState(null);

  useEffect(() => {
    async function setDataToCategories() {
      const data = await _categoryService.getAllCategoriesFromApi();
      setCategories(data);
    }

    /**
     * Sets category specified by categoryId to state.
     * @param {string} categoryId - category id from the parent component
     */
    async function setValueByCategoryId(categoryId) {
      let category = await _categoryService.getCategoryByIdFromApi(categoryId);
      if (category instanceof CategoryDto) {
        setValue(category);
      }
    }

    if (categories.length === 0) {
      setDataToCategories();
    }
    if (categoryId !== undefined && value === null) {
      setValueByCategoryId(categoryId);
    }
  });

  /**
   * Handeles autocomplete input field change event.
   * Checks if category not exists and creates new category in the storage.
   * Sets new category to value state.
   * @param {CategoryDto} newValue - a category that the user selected in the input field
   */
  const handleAutocompleteChange = async (newValue) => {
    if (newValue !== null) {
      if (newValue.id === undefined && newValue.name !== null) {
        let result = await createNewCategory(newValue.name);
        newValue = result;
      }
      onSelect(newValue.id);
    } else {
      onSelect();
    }
    updateCategories();
    setValue(newValue);
  };

  /**
   * Create new category in the storage.
   * @param {string} name - name of category to be created.
   * @returns a created category as a CategoryDto object
   */
  async function createNewCategory(name) {
    let categoryDto = new CategoryDto();
    categoryDto.name = name;
    let result = await _categoryService.createNewCategory(categoryDto);
    return result;
  }

  /**
   * Cleans categories state.
   */
  function updateCategories() {
    // Needed for correctly updating
    setCategories([]);
  }

  return (
    <Box sx={{ minWidth: 120, marginLeft: 5, marginRight: 5, paddingTop: 2 }}>
      <AutocompleteWithAdd
        model={new CategoryDto()}
        title="Category"
        items={categories}
        value={value}
        onChange={handleAutocompleteChange}
      />
    </Box>
  );
}
