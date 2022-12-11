import { environment } from "../environments/environment";

import ApiService from "./api-service";
import CategoryDto from "../dto/category-dto";
import Logger from "../utils/logger";

export default class CategoryService {
  constructor() {
    this._apiService = new ApiService();
    this._endpoint = environment.categoryEndpoint;
    this._logger = new Logger();
  }

  // READ

  /**
   * Gets all categories from the storage.
   * @returns a list of categories as an array of CategoryDto objects
   */
  async getAllCategoriesFromApi() {
    let response = await this._apiService.get(this._endpoint, "");
    let categories = response.map((ent) => CategoryDto.fromResponse(ent));
    return categories;
  }

  /**
   * Gets the category specified by the unique identifier
   * @param {*} id - a category unique identifier
   * @returns a category as a CategoryDto object
   */
  async getCategoryByIdFromApi(id) {
    let response = await this._apiService.getById(this._endpoint, id);
    let categoryDto = CategoryDto.fromResponse(response);
    return categoryDto;
  }

  /**
   * Checks if a category exists in the storage
   * @param {string} name - the name of the category
   * @returns a bolean
   */
  async isCategoryExist(name) {
    let category = await this._apiService.get(this._endpoint, {name: name});
    let isExist = category.some(c => c.name === name);
    if (isExist) {
      return true;
    }
    return false;
  }

  // CREATE
  
  /**
   * Creates a new category in the storage.
   * @param {CategoryDto} categoryDto - category to be created
   * @returns created category as a CategoryDto object
   */
  async createNewCategory(categoryDto) {
    try {
      let response = await this._apiService.post(this._endpoint, categoryDto);

      if (response !== undefined) {
        let result = CategoryDto.fromResponse(response);
        return result;
      }
      throw new Error("Failed to create category");
    } catch (error) {
      this._logger.error(error.message);
    }
  }
}
