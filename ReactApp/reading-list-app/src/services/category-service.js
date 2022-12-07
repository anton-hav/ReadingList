import { environment } from "../environments/environment";

import ApiService from "./api-service";
import CategoryDto from "../dto/category-dto";

export default class CategoryService {
  constructor() {
    this._apiService = new ApiService();
    this._endpoint = environment.categoryEndpoint;
  }

  async getAllCategoriesFromApi() {
    let response = await this._apiService.get(this._endpoint, "");
    let categories = response.map((ent) => CategoryDto.fromResponse(ent));
    return categories;
  }

  async getCategoryByIdFromApi(id) {
    let response = await this._apiService.getById(this._endpoint, id);
    let categoryDto = CategoryDto.fromResponse(response);
    return categoryDto;
  }
}
